using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyAI;

public class CollectorAI : EnemyAI
{
	public GameObject battery = null, target = null;
	GameObject goal;
	public float detectionRadius;
	// Start is called before the first frame update
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
        if(state == State.Flee)
        {
            agent.speed = moveSpeed * 2;
        }
        else
        {
            agent.speed = moveSpeed;
        }

        switch (state)
        {
            case State.Wander:
                
                break;
            case State.GoTo:
                break;
            case State.Flee:
                break;
            case State.Extra:
                break;
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

    protected override void FixedUpdate()
	{
		Collider[] col;
		if (battery == null)
		{//If ur not carrying a battery, check if there is a battery in your detection sphere and go to the closest one.
			col = Physics.OverlapSphere(transform.position, detectionRadius, LayerMask.GetMask("Pickup"));

			NavMeshHit nmHit;
			int layer = 0;
			layer = layer >> 3;
			if (col.Length >= 1)
			{
				//Debug.Log(col[0].gameObject.transform.parent.name);
				float dis = 10000, closest = 1000;
				for (int i = 0; i < col.Length; i++)
				{
					dis = Vector3.Distance(transform.position, col[i].gameObject.transform.position);
					if (dis < closest)
					{
						if (NavMesh.SamplePosition(col[i].gameObject.transform.position, out nmHit, detectionRadius / 2, NavMesh.AllAreas))
						{
							GoTo(nmHit.position);
						}
					}
				}
			}
			else
			{
				//goToEnabled = false;
			}
		}


		col = Physics.OverlapSphere(transform.position, detectionRadius);
		fleeObject = null;
		foreach (Collider c in col)
		{
			if (c.gameObject.tag == "Player")
			{
				fleeObject = c.gameObject;
				state = State.Flee;
			}
		}
		base.FixedUpdate();
	}
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