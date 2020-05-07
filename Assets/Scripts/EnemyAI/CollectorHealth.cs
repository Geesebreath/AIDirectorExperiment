using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorHealth : Health
{
	public override void Death()
	{
		GetComponent<CollectorAI>().RemoveBattery();
		base.Death();
	}

}
