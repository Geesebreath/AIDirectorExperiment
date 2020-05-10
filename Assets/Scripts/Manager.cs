using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Manager : MonoBehaviour
{
	public static Manager SharedInstance;
	protected int totalBatteries, currentBatteries;
	protected float currentTime, second; //Used to make calculation each second easier and more readable

	public float pointIncrement, // How many points the manager gets each second.
	pointIncrementIncrement, // How many points the point increment increases by each increment Time.
	points, // Current number of points Manager has to spawn enemies.

	currentPointIncrementTime, //Current Time until the amount of points the manager gets to spend on enemies increases
	pointIncrementTime, // Time until the amount of points the manager gets to spend on enemies increases

	//Enemies are spawned after a random amount of time between maximum and minimum, set into currentSpawnTime
	maximumSpawnTime,
	minimumSpawnTime,
	currentSpawnTime;

	public int collectorCost, otherEnemyCost; // Enemy point costs to spawn
	bool dead;
	public GameObject floor;

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
		currentSpawnTime = Random.Range(minimumSpawnTime, maximumSpawnTime);
	}

	void Update()
	{
		if (!dead)
		{// Increment current time and check if points can be spent to spawn in new enemies
			currentTime += Time.deltaTime;
			second -= Time.deltaTime;
			currentPointIncrementTime -= Time.deltaTime;

			if (currentPointIncrementTime < 0)
			{
				pointIncrement += pointIncrementIncrement;
				currentPointIncrementTime = pointIncrementTime;
			}

			if (second < 0)
			{
				points += pointIncrement;
				second = 1;
			}

			if (currentTime > currentSpawnTime)
			{
				currentTime = 0;
				SpawnEnemies();
			}
		}
	}

	public void Score()
	{ // Called when enemy Collector brings a Battery to the score area.
		currentBatteries--;
		if (currentBatteries <= 0)
		{
			GameOver();
		}
	}

	void SpawnEnemies() 
	{ // Called when the set currentSpawnTime is surpassed by the current time.
		int chance = Random.Range(1, 11); // Hardcoded chance to spawn different kind of enemy. Can be increased and adjusted for any amount of enemy types.
		while (points > 0)
		{// Spawn enemies while the manager still has points to spend.
			if (chance < 7)
			{
				SpawnCollector();
				points -= collectorCost;
			}
			else
			{// If a more powerful enemy will be spawned, randomly choose what type.
				int secondChance = Random.Range(1, 3); 
				if (secondChance == 1)
				{
					SpawnHunter();

				}
				else
				{
					SpawnSeeker();
				}
				points -= otherEnemyCost;
			}
		}
		points = 0;
		// Randomly set the amount of time it will take until the manager can spawn more enemies (longer will result in harder waves).
		currentSpawnTime = Random.Range(minimumSpawnTime, maximumSpawnTime); 
	}

	void SpawnCollector()
	{
		GameObject col = ObjectPool.SharedInstance.GetCollector();
		if (col != null)
		{
			col.transform.position = RandomPointOnFloor();
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
			col.transform.position = RandomPointOnFloor();
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
			col.transform.position = RandomPointOnFloor();
			col.transform.Translate(Vector3.up * 10);
			col.transform.rotation = Quaternion.identity;
			col.SetActive(true);
			col.GetComponent<EnemyAI>().Start();
		}
	}
	protected Vector3 RandomPointOnFloor()
	{// Get a random point on the floor navmesh to spawn the enemy in.
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
