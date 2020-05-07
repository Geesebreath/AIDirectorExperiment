using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    // Start is called before the first frame update
    CollectorAI enemy;
    GameObject bat; 
    Manager manager;

    private void Start()
    {
        manager = Manager.SharedInstance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.tag == "Enemy")
        {//Check if parent gameobject is an enemy
            enemy = other.transform.parent.gameObject.GetComponent<CollectorAI>();
            if (enemy != null && enemy.battery != null)
            {//null check for correctly getting the script
                bat = enemy.battery;
                if (enemy.RemoveBattery())
                {
                    bat.SetActive(false);
                    enemy.goToEnabled = false;
                    manager.Score();
                }

            }
        }
    }
}
