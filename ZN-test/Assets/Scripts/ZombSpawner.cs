using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ZombSpawner : MonoBehaviour {
	public bool shouldSpawn = true;
	private int walker_limit = 64;
	private int leader_limit = 10;
	private int leader_count = 0;
	private int walkers_per_leader = 10;
	private int walkers_count = 0;
	private GameObject[] walkers;
	private GameObject[] leaders;
	public GameObject[] Players;
	public GameObject walkerPrefab;
	public GameObject leaderPrefab;
	// public GameObject closestPlayer;
	public float closestPlayerDistance = Mathf.Infinity;
	private float spawn_range = 50f;
	private float minSpawnDist = 10f;
	private float maxSpawnDist = 512f;
	private float SpawnTime;
	private Vector3 point;
	[SerializeField] private float spawnDelay = 0.25f;
	void Start()
	{
		Players = GameObject.FindGameObjectsWithTag("Player");
		SpawnTime = Time.time;
	}
	
	void FixedUpdate () {
		walkers = GameObject.FindGameObjectsWithTag("Walker_enemy");
		leaders = GameObject.FindGameObjectsWithTag("Walker_leader");
		if(shouldSpawn == false)
		{
			return;
		}
		if(walkers.Length >= walker_limit)
		{
			if(leaders.Length >= leader_limit)
			{
				return;
			}
		}
		if (RandomPoint(transform.position, spawn_range, out point)) {
			if((Time.time - SpawnTime)>=spawnDelay)
			{
				if(AreaCheck())
				{
					Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
					SpawnEnemy(point);
					SpawnTime = Time.time;
				}
			}		
		}
	}

	bool RandomPoint(Vector3 center, float spawn_range, out Vector3 result) {
		for (int i = 0; i < 5; i++) {
			Vector3 randomPoint = center + Random.insideUnitSphere * spawn_range;
			NavMeshHit hit;
			if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) {
				result = hit.position;
				return true;
			}
		}
		result = Vector3.zero;
		return false;
	}

	bool AreaCheck() //checks if there are players nearby
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in Players)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            closestPlayerDistance = directionToTarget.magnitude;
            float distanceSqrToTarget = directionToTarget.sqrMagnitude;
            if (distanceSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }
        if((closestPlayerDistance > minSpawnDist)&&(closestPlayerDistance < maxSpawnDist))
		{
			return true;
		}
		else
		{
			return false;
		}
    }

	private void SpawnEnemy(Vector3 point)
	{
		if(Random.Range(0,100) > 12){
			if(walkers_count < walkers_per_leader){
			GameObject walker = Instantiate(walkerPrefab,point,Quaternion.identity);
			walkers_count ++;
			}
		}
		else {
			if(leader_count < leader_limit){
				GameObject leader = Instantiate(leaderPrefab,point,Quaternion.identity);
				walkers_count = 0;
			}
		}
	}
}
