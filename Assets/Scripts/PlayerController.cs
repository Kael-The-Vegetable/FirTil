using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	private Vector2 _moveDir;
	private Rigidbody2D _rb;

	[SerializeField] private float _moveForce = 100;
	[SerializeField] private float _maxSpeed = 4;

	[SerializeField] GameObject sunShot;
	[SerializeField] Transform sunShotTransform;

	bool canFire = true;
	float timer;
	[SerializeField] float timeBetweenFiring;

	bool canHeadbutt = true;
	float timer2;
	[SerializeField] float timeBetweenHeadbutting;

	[SerializeField] GameObject headbuttHitbox;

	[Header("Planting")]
	//[SerializeField] List<PlantData> plants = new();
	private SeedInventory seedBag;
	[SerializeField] LayerMask plotMask;
	[SerializeField] LayerMask plantMask;

	[Header("Animations")]
	[SerializeField] Animator anim;
	[SerializeField] SpriteRenderer body;
	[SerializeField] private bool performingAction = false;

	private void Awake()
	{
		seedBag = GetComponent<SeedInventory>();
		anim = GetComponentInChildren<Animator>();
	}
	private void Start()
	{
		InputManager.OnMove.AddListener(Move);
		InputManager.SunShot.AddListener(ShootSunShot);
		InputManager.Headbutt.AddListener(Headbutt);
		InputManager.TilPlant.AddListener(TilPlant);
		InputManager.WaterCan.AddListener(WaterPlant);
		InputManager.Next.AddListener(seedBag.NextSeed);
		InputManager.Previous.AddListener(seedBag.PreviousSeed);
		_rb = GetComponent<Rigidbody2D>();
		performingAction = false;
		seedBag.CheckEquippedSeed();
	}
	private void OnDestroy()
	{
		InputManager.OnMove?.RemoveListener(Move);
		InputManager.SunShot?.RemoveListener(ShootSunShot);
		InputManager.Headbutt?.RemoveListener(Headbutt);
		InputManager.TilPlant?.RemoveListener(TilPlant);
		InputManager.WaterCan?.RemoveListener(WaterPlant); 
		InputManager.Next?.RemoveListener(seedBag.NextSeed);
		InputManager.Previous?.RemoveListener(seedBag.PreviousSeed);
	}

	private void FixedUpdate()
	{
		

		if (_moveDir != Vector2.zero && !performingAction)
		{
			// Set Animations
			anim.SetFloat("LookX", _moveDir.x);
			anim.SetFloat("LookY", _moveDir.y);
			if (_moveDir.x < 0)
			{
				body.flipX = true;
			}
			else body.flipX = false;


			anim.SetBool("IsWalking", true);
			Vector2 levelOfForce = Vector2.one;
			Vector2 movementScalar = GridRatio.Instance.MovementScalar;

			if ((_rb.linearVelocity + _moveDir).sqrMagnitude >= _rb.linearVelocity.sqrMagnitude)
			{ // force won't help stop target
				levelOfForce = new Vector2(
					Mathf.Clamp01((_maxSpeed - _rb.linearVelocity.magnitude) / _maxSpeed) * movementScalar.x,
					Mathf.Clamp01((_maxSpeed - _rb.linearVelocity.magnitude) / _maxSpeed) * movementScalar.y);
			}
			_rb.AddForce(_moveForce * _moveDir * movementScalar * levelOfForce);
		}
		else
		{
			anim.SetBool("IsWalking", false);
		}

		if (!canFire)
		{
			timer += Time.deltaTime;
			if (timer > timeBetweenFiring)
			{
				canFire = true;
				timer = 0;
			}
		}

		if (!canHeadbutt)
		{
			timer2 += Time.deltaTime;
			if (timer2 > timeBetweenHeadbutting)
			{
				canHeadbutt = true;
				timer2 = 0;
			}
		}
	}

	private void Move(Vector2 dir) => _moveDir = dir;

	private void ShootSunShot()
	{
		if (performingAction) return;

		if (canFire)
		{
			StartCoroutine(FiringSunShot(sunShotTransform.position));
		}
		
	}

	private void Headbutt()
	{
		if (performingAction) return;

		if (canHeadbutt)
		{
			StartCoroutine(Headbutting());
		}
	}

	private void TilPlant()
	{
		if (performingAction) return;

		var p = PathGenerator.Instance.GetPathSectionFromFloatPosition(transform.position);
		var plot = p.ActivePlot.GetComponent<TilePlot>();
        if (p.IsOccupied || !plot.IsAlreadyTilled())
        { // Plant on it
			StartCoroutine(Digging(plot));
		}
		else if(plot.IsAlreadyTilled() && plot.TypeMatches(seedBag.GetPlant()))
		{
			if (!seedBag.IsInventoryEmpty())
			{
				StartCoroutine(Planting(plot));
			}
		}
	}

	private void WaterPlant()
	{
		if (performingAction) return;

		Collider2D plantHit = Physics2D.OverlapPoint(PathGenerator.Instance.GetPathSectionFromFloatPosition(transform.position).transform.position, plantMask);
		if (plantHit != null)
		{
			StartCoroutine(WateringPlant(plantHit));
		}
	}

	IEnumerator WateringPlant(Collider2D plantHit)
	{
		anim.SetTrigger("Grow");
		performingAction = true;
		_rb.linearVelocity = Vector2.zero;
		yield return new WaitForSeconds(1.1f);
		if (plantHit.TryGetComponent<IPlant>(out IPlant plant))
		{
			plant.WaterPlant();
		}
		performingAction = false;
	}

	IEnumerator Digging(TilePlot plot)
	{
		anim.SetTrigger("Dig");
		performingAction = true;
		_rb.linearVelocity = Vector2.zero;
		yield return new WaitForSeconds(2f);
		plot.Dig();
		performingAction = false;
	}

	IEnumerator Planting(TilePlot plot)
	{
		anim.SetTrigger("Plant");
		performingAction = true;
		_rb.linearVelocity = Vector2.zero;
		yield return new WaitForSeconds(1.1f);
		plot.PlaceNewPlant(seedBag.GetPlant());
		seedBag.RemoveEquippedSeed(1);
		performingAction = false;
	}

	IEnumerator Headbutting()
	{
		// Rotate player to where their aiming
		Vector3 animDirection = (sunShotTransform.position - transform.position).normalized;
		anim.SetFloat("LookX", animDirection.x);
		anim.SetFloat("LookY", animDirection.y);
		if (animDirection.x < 0)
		{
			body.flipX = true;
		}
		else body.flipX = false;

		anim.SetTrigger("Attack1");
		performingAction = true;
		_rb.linearVelocity = Vector2.zero;
		yield return new WaitForSeconds(.5f);
		canHeadbutt = false;
		headbuttHitbox.SetActive(true);
		performingAction = false;
		
	}

	IEnumerator FiringSunShot(Vector3 firePosition)
	{
		// Rotate player to where their aiming
		Vector3 animDirection = (sunShotTransform.position - transform.position).normalized;
		anim.SetFloat("LookX", animDirection.x);
		anim.SetFloat("LookY", animDirection.y);
		if (animDirection.x < 0)
		{
			body.flipX = true;
		}
		else body.flipX = false;

		anim.SetTrigger("Attack2");
		performingAction = true;
		_rb.linearVelocity = Vector2.zero;
		yield return new WaitForSeconds(.5f);
		canFire = false;
		Instantiate(sunShot, firePosition, Quaternion.identity);
		performingAction = false;
	}
}
