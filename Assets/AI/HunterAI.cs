using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class HunterAI : EnemyAI
{
    public float detectionRadius;
    public GameObject target;
    bool canAttack;
    RaycastHit hit;
    public enum MyState
    {
        Default,
        Attack
    };
    public MyState myState;
    public override void Start()
    {
        myState = MyState.Default;
        base.Start();
        canAttack = true;
    }

    protected override void Update()
    {

        if(state == State.GoTo)
        {
            agent.speed = moveSpeed * 1.5f;
        }
        else
        {
            agent.speed = moveSpeed;
        }

        base.Update();
    }

    protected override void FixedUpdate()
    {
        Collider[] col;
        col = Physics.OverlapSphere(transform.position, detectionRadius);
        fleeObject = null;
        myState = MyState.Default;
        foreach (Collider c in col)
        {
            if (c.gameObject.tag == "Player")
            {
                target = c.gameObject;
                myState = MyState.Attack;
                GoTo(target.transform.position+target.transform.forward);
            }
        }
        base.FixedUpdate();
    }

    protected override void DoAction()
    {
        if(myState==MyState.Attack)
        {
            if (canAttack)
            {
                Attack();
                canAttack = false;
                StartCoroutine("AttackTimer", 1);
            }
        }
        else
        {
            goToEnabled = false;
            state = State.Wander;
        }
        
    }

    protected void Attack()
    {
        target.SendMessageUpwards("ApplyDamage", 1);
    }

    private void ApplyDamage(float amount)
    {
        if (Physics.Raycast(transform.position + Vector3.up, GameObject.Find("Player").transform.position-transform.position, out hit, 100))
        {
            if (hit.transform.tag == "Player")
            {
                target = hit.transform.gameObject;
                GoTo(target.transform.position);
            }
        }
    }


    IEnumerator AttackTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        canAttack = true;
    }


}
