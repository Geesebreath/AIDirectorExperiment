using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    Vector3 agentDestination;
    public GameObject fleeObject;
    protected NavMeshAgent agent;
    protected GameObject floor;
    public Vector3 savedDestination;
    public float moveSpeed;
    public bool goToEnabled;
    public float stoppingDistance = 5, fleeDistance = 10;
    public enum State
    {
        Wander,
        GoTo,
        Flee,
        Extra
    };
    public State state;
    // Start is called before the first frame update
    public virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        state = State.Wander;
        floor = GameObject.Find("NavFloor");
        GetComponent<Health>().Start();
        goToEnabled = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        agentDestination = agent.destination;
		if (fleeObject == null && goToEnabled)
		{
			state = State.GoTo;
			agent.stoppingDistance = 0f;
		}
		else if (fleeObject == null)
		{
			state = State.Wander;

			agent.stoppingDistance = stoppingDistance;
		}
		//else
		//{
		//    state = State.Flee;
		//    agent.stoppingDistance = stoppingDistance;
		//}
		float dist = agent.remainingDistance;
        //Debug.Log(gameObject.name + "  " + agent.destination);
        switch (state)
        {
            
            case State.Wander:
                goToEnabled = false;
                if (agent.destination == null)
                {//If no destination, set a random point on the navmesh as the destination
                    FindRandomNavPoint();
                }
                if (dist != Mathf.Infinity && (agent.pathStatus == NavMeshPathStatus.PathComplete || agent.pathStatus == NavMeshPathStatus.PathPartial) && agent.remainingDistance <= stoppingDistance)
                {
                    FindRandomNavPoint();
                }
                break;
            case State.GoTo:
                if (agent.destination != savedDestination)
                {//If you were fleeing but not anymore, set your destination to your saved destination.
                    agent.destination =savedDestination;
                }
                if (agent.remainingDistance != Mathf.Infinity && (agent.pathStatus == NavMeshPathStatus.PathComplete || agent.pathStatus == NavMeshPathStatus.PathPartial) && agent.remainingDistance <= stoppingDistance)
                {//Do something once you reach your destination
                    goToEnabled = false;
                    DoAction();
                    
                }
                    break;
            case State.Flee:
                float objDis = Vector3.Distance(fleeObject.transform.position, gameObject.transform.position);
                if(objDis>= fleeDistance)
                {
                    fleeObject = null;
                    state = State.Wander;
                }
                else
                {
                    //if (agent.destination == null)
                    //{//If no destination, set a random point on the navmesh as the destination
                       FindFleePoint();
					//}
					
				}
                break;
            case State.Extra:
                // For Children to use.
                break;
        }
    }
    protected virtual void FindRandomNavPoint()
    {
        NavMeshHit nmHit;
        Vector3 bCentre, bExtents, randPoint;
        bCentre = floor.GetComponent<BoxCollider>().bounds.center;
        bExtents = floor.GetComponent<BoxCollider>().bounds.extents;
        bool con = true;
        do
        {
            randPoint = new Vector3(bCentre.x + Random.Range(-bExtents.x, bExtents.x), transform.position.y, bCentre.z + Random.Range(-bExtents.z, bExtents.z));
            if (NavMesh.SamplePosition(randPoint, out nmHit, 4f, NavMesh.AllAreas))
            {
                randPoint = nmHit.position;
                con = false;
            }
        } while (con);
        agent.destination = randPoint;
    }
    protected virtual void FindFleePoint()
    {
        Vector3 fleePoint = transform.position - fleeObject.transform.position;
		fleePoint.Normalize();
        fleePoint *= fleeDistance;
        agent.destination = transform.position + fleePoint;
    }
    protected virtual void DoAction()
    {
        goToEnabled = false;
        state = State.Wander;
    }

    public virtual void GoTo(Vector3 destination)
    {

        goToEnabled = true;
        agent.stoppingDistance = 0.5f;
        savedDestination = destination;
        Debug.Log(gameObject.name + " going to " + savedDestination);
    }

    protected virtual void FixedUpdate()
    {
        switch (state)
        {
            case State.Wander:
                break;
            case State.GoTo:
                break;
            case State.Flee:
                break;
        }
    }
}