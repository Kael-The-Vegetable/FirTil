using UnityEngine;

public class EconomyManager : Singleton<EconomyManager>
{
	[SerializeField] private int _balance = 0;
	public int Balance => _balance;
	protected override void Initialize() { }

	public bool ValidPurchase(int purchase) => purchase <= _balance;
	public bool MakePurchase(int purchase)
	{
		if (ValidPurchase(purchase))
		{
			_balance -= purchase;
			return true;
		}
		return false;
	}

	public void AddPoints(float amount)
	{
		_balance += ((int)amount);
	}
}
