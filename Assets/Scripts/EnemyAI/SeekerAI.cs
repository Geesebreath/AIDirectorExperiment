using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SeekerAI : EnemyAI
{
    public GameObject checkPoint;
    NavMeshHit hit;
    protected override void Update()
    {
        base.Update();
    }

    protected void FixedUpdate()
    {
        //Check for player
        int lMask = 9;
        lMask = 1 << lMask;
        Collider[] allCols = Physics.OverlapSphere(checkPoint.transform.position, 10f);
        Collider[] foundCols = Physics.OverlapSphere(checkPoint.transform.position, 10f, lMask);
        if (foundCols.Length>0)
        {
            HunterAI hunt;

            foreach(Collider col in allCols)
            {
                hunt = col.transform.root.GetComponent<HunterAI>();
                if(hunt !=null)
                {
                    if(NavMesh.SamplePosition(foundCols[0].transform.position, out hit, 5, NavMesh.AllAreas))
                    hunt.GoTo(hit.position);
                }
            }
            //Actions for finding Player
            Debug.Log("Found Player at " + foundCols[0].gameObject.transform.position);
        }

        lMask = 1<<8;
        foundCols = Physics.OverlapSphere(checkPoint.transform.position, 10f, lMask);
        //Check for Pickups
        if (foundCols.Length > 0)
        {
            CollectorAI collect;

            foreach (Collider col in allCols)
            {
                collect = col.transform.root.GetComponent<CollectorAI>();
                if (collect != null)
                {
                    if (NavMesh.SamplePosition(foundCols[0].transform.position, out hit, 5, NavMesh.AllAreas))
                        collect.GoTo(hit.position);
                }
            }
            //Actions for finding Pickup
            //Debug.Log("Found Battery at " + foundCols[0].gameObject.transform.position);
        }
    }

}
