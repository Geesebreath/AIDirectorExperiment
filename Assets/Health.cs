using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

	[SerializeField]
	float maxHealth, currentHealth;

	public float MaxHealth
	{
		get { return maxHealth; }
		set { maxHealth = value; }
	}

	public float CurrentHealth
	{
		get { return currentHealth; }
		set { currentHealth = value; }
	}

	// Start is called before the first frame update
	public virtual void Start()
	{
		CurrentHealth = MaxHealth;
	}

	// Update is called once per frame
	public virtual void Update()
	{
		if (CurrentHealth <= 0)
		{
			CurrentHealth = 0;
			Death();
		}
	}

	public virtual void Death()
	{
		gameObject.SetActive(false);
	}

	public virtual void ApplyDamage(float amount)
	{
		CurrentHealth -= amount;
	}

	public void Heal(float amount)
	{
		if (CurrentHealth < MaxHealth)
		{
			CurrentHealth += amount;
			if (CurrentHealth > MaxHealth)
			{
				CurrentHealth = MaxHealth;
			}
		}
	}
}
