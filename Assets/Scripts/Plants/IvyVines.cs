using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IvyVines : PlantMain
{
	[SerializeField] LayerMask targetMask;

	[SerializeField] private List<Collider2D> TetheredEnemies = new List<Collider2D>();
	[SerializeField] private int maxTethers = 4;
	[SerializeField] private float tetherSlow = 0.2f;

	[SerializeField] private GameObject[] Tethers;

	public override void Start()
	{
		base.Start();
		foreach (GameObject t in Tethers)
		{
			t.GetComponent<Tether>().SetStem(transform);
		}
	}
	public override void Update()
	{
		base.Update();
		if (currentStage != IPlant.GrowthStage.Full) return;


		TryActivate();
		for (int i = 0; i < Tethers.Length; i++)
		{
			if (TetheredEnemies.Count >= i + 1)
			{
				Tethers[i].transform.position = TetheredEnemies[i].transform.position;
				Tethers[i].SetActive(true);
			}
			else Tethers[i].SetActive(false);
		}

	}
	public override void Activate()
	{
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, plantData.BaseRange, targetMask);

		if (TetheredEnemies.Count < maxTethers)
		{
			foreach (Collider2D hit in hitEnemies)
			{
				if (!TetheredEnemies.Contains(hit))
				{
					TetheredEnemies.Add(hit);
					hit.gameObject.GetComponent<IDamagable>().Tether(tetherSlow);
				}
			}
		}
		
		for (int i = 0; i < TetheredEnemies.Count; i++)
		{
			if (!hitEnemies.Contains(TetheredEnemies[i]))
			{
				TetheredEnemies[i].gameObject.GetComponent<IDamagable>().UnTether();
				TetheredEnemies.RemoveAt(i);
			}
		}

		//foreach (Collider2D enemy in TetheredEnemies)
		//{
		//	if (!hitEnemies.Contains(enemy))
		//	{
		//		enemy.gameObject.GetComponent<IDamagable>().UnTether();
		//		TetheredEnemies.Remove(enemy);
		//	}
		//}

	}
}
