using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyAI;

public class CollectorAI : EnemyAI
{
	public GameObject battery = null, target = null;

	GameObject goal;
	public float detectionRadius = 2;

	public enum MyState
	{
		Default,
		Pickup,
		Dropoff
	};
	public MyState myState;

	public override void Start()
	{
		myState = MyState.Default;
		goal = GameObject.Find("Goal");
		base.Start();
	}
	protected override void Update()
	{
		if (myState != MyState.Default)
		{
			state = State.Extra;
		}
		if (battery != null && state != State.Flee)
		{
			GoTo(goal.transform.position);
		}
		if (state == State.Flee)
		{
			agent.speed = moveSpeed * 2;
		}
		else
		{
			agent.speed = moveSpeed;
		}


		base.Update();
		if (state == State.Extra)
		{
			switch (myState)
			{
				case MyState.Default:
					break;
				case MyState.Pickup:
					DoAction();
					break;
				case MyState.Dropoff:
					break;
			}
		}

	}

	protected void FixedUpdate()
	{
		Collider[] colliders;

		colliders = Physics.OverlapSphere(transform.position, detectionRadius);
		fleeObject = null;
		float dis = 10000, closest = 1000;
		for (int i = 0; i < colliders.Length; i++)
		{
			Collider c = colliders[i];
			if (c.gameObject.tag == "Player")
			{// Check if player is nearby
				fleeObject = c.gameObject;
				state = State.Flee;
				i = colliders.Length;
			}
			else if (battery != null && c.gameObject.tag == "Pickup")
			{// If player is not nearby, check if battery is nearby
				
				//int layer = 0;
				//layer = layer >> 3;
				dis = Vector3.Distance(transform.position, colliders[i].gameObject.transform.position);
				if (dis < closest)
				{
					NavMeshHit nmHit;
					if (NavMesh.SamplePosition(colliders[i].gameObject.transform.position, out nmHit, detectionRadius / 2, NavMesh.AllAreas))
					{
						GoTo(nmHit.position);
					}
				}
			}
		}
	}


	//if (battery == null)
	//{//If not carrying a battery, check if there is a battery in your detection sphere and go to the closest one.
	//	colliders = Physics.OverlapSphere(transform.position, detectionRadius, LayerMask.GetMask("Pickup"));


	//	else
	//	{
	//		//goToEnabled = false;
	//	}
	//}


	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 8)
		{//Set pickup to be a child of collector and rotate it to be easier to see.
			target = other.gameObject.transform.parent.gameObject;
			myState = MyState.Pickup;
			state = State.Extra;
		}
	}

	protected override void DoAction()
	{
		switch (myState)
		{
			case MyState.Default:
				break;
			case MyState.Pickup:
				target.gameObject.layer = 0;
				foreach (Transform t in target.transform)
				{
					t.gameObject.layer = 0;
				}
				battery = target;
				battery.transform.position = transform.position + Vector3.up * 2;
				battery.transform.rotation = Quaternion.Euler(0, 0, 0);
				battery.transform.parent = transform;
				myState = MyState.Default;
				break;
			case MyState.Dropoff:
				break;
		}
	}

	public bool RemoveBattery()
	{
		bool removed = false;
		if (battery != null)
		{
			battery.transform.parent = null;
			battery.transform.rotation = Quaternion.Euler(90, 0, 0);
			battery.transform.position = transform.position;
			battery.layer = 8;
			foreach (Transform t in battery.transform)
			{
				t.gameObject.layer = 8;
			}
			battery = null;
			removed = true;
		}
		return removed;
	}


}