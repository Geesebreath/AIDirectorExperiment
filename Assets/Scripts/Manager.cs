using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Manager : MonoBehaviour
{
    public static Manager SharedInstance;
    protected int totalBatteries, currentBatteries;
    public float incrementTime, maxIncrementTime, maxTime, minTime, spawnTime, currentTime, second, pointIncrement, pointIncrementIncrement, points;
    public int colCost, hunCost;
    bool dead;
    public GameObject floor;

    // Start is called before the first frame update
    private void Awake()
    {
        SharedInstance = this;
        dead = false;
    }
    void Start()
    {
        Time.timeScale = 1;
        second = 1;
        totalBatteries = GameObject.FindGameObjectsWithTag("Pickup").Length;
        currentBatteries = totalBatteries;
        spawnTime = Random.Range(minTime, maxTime);
    }
    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            currentTime += Time.deltaTime;
            second -= Time.deltaTime;
            incrementTime -= Time.deltaTime;
            if (incrementTime < 0)
            {
                pointIncrement += pointIncrementIncrement;
                incrementTime = maxIncrementTime;
            }

            if (second < 0)
            {
                points += pointIncrement;
                second = 1;
            }
            if (currentTime > spawnTime)
            {
                currentTime = 0;
                SpawnEnemies();
            }
        }
        else
        {
            //Do Nothing
        }

        // SpawnCollector();
        // SpawnHunter();
    }

    public void Score()
    {
        currentBatteries--;
        if (currentBatteries <= 0)
            GameOver();
    }

    void SpawnEnemies()
    {
        int chance = Random.Range(1, 11);
        while (points > 0)
        {
            if (chance < 7)
            {
                SpawnCollector();
                points -= 2;
            }
            else
            {
                int secChance = Random.Range(1, 3);
                if(secChance ==1)
                {
                    SpawnHunter();
                    
                }
                else
                {
                    SpawnSeeker();
                }
                points -= 5;
            }
        }
        ////Always spawn 2 collectors
        //SpawnCollector();
        //SpawnCollector();
        //points -= 2;

        //for (int i = 1; i <= (int)(points / hunCost); i++)
        //{
        //    SpawnHunter();
        //}

        //for (int i = 1; i <= (int)(points % hunCost); i++)
        //{
        //    SpawnCollector();
        //}
        points = 0;
        spawnTime = Random.Range(minTime, maxTime);
    }

    void SpawnCollector()
    {
        GameObject col = ObjectPool.SharedInstance.GetCollector();
        if (col != null)
        {
            col.transform.position = RandomPoint();
            col.transform.rotation = Quaternion.identity;
            col.SetActive(true);
            col.GetComponent<EnemyAI>().Start();
        }
    }

    void SpawnHunter()
    {
        GameObject col = ObjectPool.SharedInstance.GetHunter();
        if (col != null)
        {
            col.transform.position = RandomPoint();
            col.transform.rotation = Quaternion.identity;
            col.SetActive(true);
            col.GetComponent<EnemyAI>().Start();
        }
    }

    void SpawnSeeker()
    {
        GameObject col = ObjectPool.SharedInstance.GetSeeker();
        if (col != null)
        {
            col.transform.position = RandomPoint();
            col.transform.Translate(Vector3.up * 10);
            col.transform.rotation = Quaternion.identity;
            col.SetActive(true);
            col.GetComponent<EnemyAI>().Start();
        }
    }
    protected Vector3 RandomPoint()
    {
        NavMeshHit nmHit;
        Vector3 bCentre, bExtents, randPoint;
        bCentre = floor.GetComponent<BoxCollider>().bounds.center;
        bExtents = floor.GetComponent<BoxCollider>().bounds.extents;
        bool con = true;
        do
        {
            randPoint = new Vector3(bCentre.x + Random.Range(-bExtents.x, bExtents.x), 0f, bCentre.z + Random.Range(-bExtents.z, bExtents.z));
            if (NavMesh.SamplePosition(randPoint, out nmHit, 4f, NavMesh.AllAreas))
            {
                randPoint = nmHit.position;
                con = false;
            }
        } while (con);
        return randPoint;
    }

    public void GameOver()
    {
        dead = true;
        PauseMenu.SharedInstance.GameOver();
    }

}
