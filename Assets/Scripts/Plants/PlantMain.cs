using System.Collections;
using UnityEngine;

public class PlantMain : MonoBehaviour, IPlant, IDamagable
{
    public PlantData PlantData
    {
        get
        {
            return plantData;
        }
        set
        {
            plantData = value;
        }
    }
    public PlantData plantData;

    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
        }
    }
    public float currentHealth;

    public IPlant.GrowthStage currentStage = IPlant.GrowthStage.Seed;

    public float nextTimeToFire = 0;

    float growthProgress = 0;
    [SerializeField] bool watered = false;

    internal float currentFireRate, currentRange, currentGrowthRate;
    [Header("Sunshot Flash")]
    [SerializeField] Color sunshotFlashColor = Color.yellow;
    [SerializeField] float sunshotFlashDuration = 0.1f;

    [Header("Damage Flash")]
    [SerializeField] Color damageFlashColor = Color.darkRed;
    [SerializeField] float damageFlashDuration = 0.1f;

    [Space]
    [SerializeField] internal SpriteRenderer bodySprite;
    [SerializeField] internal Animator bodyAnim;
    [SerializeField] internal GameObject sunIcon;
    

	public virtual void Awake()
	{
		
	}

	public virtual void Start()
    {
		currentHealth = plantData.MaxHealth;
		currentFireRate = plantData.BaseFireRate;
		currentRange = plantData.BaseRange;
		currentGrowthRate = plantData.BaseGrowthRate;

        if (currentStage == IPlant.GrowthStage.Seed)
        {
            //bodyAnim.enabled = false;
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            currentHealth = plantData.MaxHealth / 3;
            sunIcon.SetActive(true);
        }
        else if (currentStage == IPlant.GrowthStage.Half)
        {
            //bodyAnim.enabled = false;
            transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            currentHealth = plantData.MaxHealth / 2;
            sunIcon.SetActive(true);
        }
        else bodyAnim.SetBool("IsGrown", true);
	}
	public virtual void Update()
	{
        if ( growthProgress < plantData.BaseGrowthTime && watered)
        {
			growthProgress += Time.deltaTime * currentGrowthRate;
		}
        
        // Advance to the next Growth Stage
        if (currentStage != IPlant.GrowthStage.Full && growthProgress >= plantData.BaseGrowthTime )
        {
            AdvanceGrowth();
        }
        
        
	}

    public void AdvanceGrowth()
    {
        switch( currentStage )
        {
            case IPlant.GrowthStage.Seed:
                currentStage = IPlant.GrowthStage.Half;
                transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                currentHealth = plantData.MaxHealth / 2;
                watered = false;
                sunIcon.SetActive(true);
                growthProgress = 0;
                break;
            case IPlant.GrowthStage.Half:
                currentStage = IPlant.GrowthStage.Full;
				transform.localScale = new Vector3(1f, 1f, 1f);
                currentHealth = plantData.MaxHealth;
                bodyAnim.SetBool("IsGrown", true);
                growthProgress = plantData.BaseGrowthTime;
                break;
        }
    }

    // Check if the ability is on cooldown
	public virtual void TryActivate()
    {
		if (Time.time > nextTimeToFire)
		{
			nextTimeToFire = Time.time + (1 / currentFireRate);
            Debug.Log(currentFireRate);
            Activate();
		}
	}

    public virtual void Activate() { }


	internal GameObject GetTarget(Collider2D[] enemies, Vector2 referencePosition)
	{
		GameObject target = null;
		switch (plantData.targetPriority)
        {
            case PlantData.TargetPriority.ClosestToPlant:
				float minDistance = 1000;

				foreach (Collider2D enemy in enemies)
				{
					if (enemy == null) continue;

					float distance = Vector2.Distance(referencePosition, enemy.transform.position);
					if (distance < minDistance)
					{
						minDistance = distance;
						target = enemy.gameObject;
					}
				}
				break;
            case PlantData.TargetPriority.ClosestToTree:
                // Just gets the enemy closest to plant
				minDistance = 1000;
                Vector3 treePosition = TreeManager.Instance.Position;

				foreach (Collider2D enemy in enemies)
				{
					if (enemy == null) continue;

					float distance = Vector2.Distance(treePosition, enemy.transform.position);
					if (distance < minDistance)
					{
						minDistance = distance;
						target = enemy.gameObject;
					}
				}
				break;
            case PlantData.TargetPriority.Strongest:
                float mostHealh = 0;

                foreach (Collider2D enemy in enemies)
                {
                    float health = enemy.GetComponent<IEnemy>().EnemyData.health;
                    if (health > mostHealh)
                    {
                        mostHealh = health;
                        target = enemy.gameObject;
                    }
                }
                break;
            case PlantData.TargetPriority.None:
                break;
        }
		
		

		return target;
	}

	// Accelerate plant growth for a set duration
	public void AccelerateGrowth(float newGrowthRate, float duration)
	{
        if (currentStage == IPlant.GrowthStage.Full || currentStage == IPlant.GrowthStage.Dead) return;

        StopCoroutine(SunshotFlash());
        StartCoroutine(SunshotFlash());

        StopCoroutine(nameof(AcceleratedGrowth));
		StartCoroutine(AcceleratedGrowth(newGrowthRate, duration));

	}
    IEnumerator AcceleratedGrowth(float newGrowthRate, float duration)
    {
        currentGrowthRate = newGrowthRate;
        yield return new WaitForSeconds(duration);
        currentGrowthRate = plantData.BaseGrowthRate;
    }

    IEnumerator SunshotFlash()
    {
        bodySprite.color = sunshotFlashColor;
        yield return new WaitForSeconds(sunshotFlashDuration);
        bodySprite.color = Color.white;
    }

    IEnumerator DamageFlash()
    {
        bodySprite.color = damageFlashColor;
        yield return new WaitForSeconds(damageFlashDuration);
        bodySprite.color = Color.white;
    }

    public void WaterPlant()
    {
        watered = true;
        sunIcon.SetActive(false);
    }

	#region IDamagable Methods
    public void TakeDamage(float damage)
    {
        if (currentStage == IPlant.GrowthStage.Dead) return;

        // Flash
        StopCoroutine(SunshotFlash());
        StopCoroutine(DamageFlash());
        StartCoroutine(DamageFlash());

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void TakeDotDamage(float damagePerTick, int numOfTicks, float duration)
    {
        StartCoroutine(DamageOverTime(damagePerTick, numOfTicks, duration));
    }

    public void HealDamage(float healAmount)
    {
        if (currentStage == IPlant.GrowthStage.Dead) return;

        currentHealth += healAmount;
        if (currentHealth > plantData.MaxHealth) currentHealth = plantData.MaxHealth;
    }

    private void Die()
    {
        transform.parent.GetComponent<TilePlot>().Dig();
    }

	public void Tether(float SpeedMult) { }
	public void UnTether() { }

	IEnumerator DamageOverTime(float damagePerTick, int numOfTicks, float duration)
	{
		int currentTick = 0;
		float timePerTick = numOfTicks / duration;
		while (currentTick < numOfTicks)
		{
			yield return new WaitForSeconds(timePerTick);
            TakeDamage(damagePerTick);
			currentTick++;
		}
	}
	#endregion

	// For Testing
	void ChangeColor(Color color)
    {
        GetComponentInChildren<SpriteRenderer>().color = color;
    }

	private void OnDrawGizmosSelected()
	{
        Gizmos.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
		Gizmos.DrawSphere(transform.position, plantData.BaseRange);
	}
}
