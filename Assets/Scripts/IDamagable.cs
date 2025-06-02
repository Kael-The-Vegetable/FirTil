using UnityEngine;

public interface IDamagable
{
	public void TakeDamage(float damage);
	public void TakeDotDamage(float damagePerTick, int numOfTicks, float duration);
	public void HealDamage(float healAmount);

	public void Tether(float speedMult);
	public void UnTether();
}
