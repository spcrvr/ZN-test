using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class WalkerLeaderController : MonoBehaviour {
	public GameObject[] PointsOfInterest;
	public GameObject[] Players;
    private GameObject _leader;
    private bool isLeaderInRange = false;
    public GameObject closestPlayer;
    public float AttackDistance = 5.0f;
    public float FollowDistance = 20.0f;
    private float roamRadius = 20.0f;
    public float closestPlayerDistance = Mathf.Infinity;
    public NavMeshAgent _navmeshagent;
    private Rigidbody zomb_rigidbody;
    [SerializeField] Vector3 finalPosition;
    [SerializeField] bool isPathSet = false;
    private float despawnDistance = 386f;
    private float despawnDuration = 20f;

 	void Awake () {
        Players = GameObject.FindGameObjectsWithTag("Player");
		PointsOfInterest = GameObject.FindGameObjectsWithTag("PointOfInterest");
        zomb_rigidbody = GetComponent<Rigidbody>();
        _navmeshagent = GetComponent<NavMeshAgent>();
    }
 	void Start() {
        _navmeshagent.speed = Random.Range(0.2f,0.5f);
        this.transform.SetParent(GameObject.FindGameObjectWithTag("Enemies_parent").transform);
    }
	private void FixedUpdate() {
		if (_navmeshagent.enabled)
        {
            closestPlayer = GetClosestPlayer();
            bool chase = (closestPlayerDistance < FollowDistance);
            bool idle = (closestPlayerDistance > FollowDistance);
            if(idle)
            {
                if(isPathSet == false)
                    { 
                        Roam();
                    }  
            }
            if (closestPlayerDistance < AttackDistance)
            {
                _navmeshagent.SetDestination(this.gameObject.transform.position);
            }
            if (chase)
            { 
                _navmeshagent.SetDestination(closestPlayer.transform.position);
            } 
        }
	}
    
	GameObject GetClosestPlayer()
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
        return bestTarget.gameObject;
    }
    private void Roam()
    {    
        Vector3 randomDirection;
        NavMeshHit hit;
        // Give freeroam a 50% chance to go towards a point of interest (can be far away)
            finalPosition = Vector3.zero;
            randomDirection = Random.insideUnitSphere * roamRadius;
            randomDirection += transform.position; // The game itself is a big terrarium for bugs
            NavMesh.SamplePosition(randomDirection, out hit, roamRadius, 1);
			if(Random.Range(0f,100f)<=50f){
            	finalPosition = hit.position;
			}
			else {
				finalPosition = PointsOfInterest[(Random.Range(0,PointsOfInterest.Length))].transform.position;
			}
            if(!isPathSet)
            { 
                _navmeshagent.SetDestination(finalPosition);
               // hasReachedDestination = false;
                isPathSet = true;
            }
            Vector3 Marker = new Vector3(finalPosition.x,finalPosition.y+2,finalPosition.z);
            Debug.DrawLine(finalPosition,Marker,Color.green, 12f);
            Invoke("HaltMovement", Random.Range(10f,30f));
	}

    private void HaltMovement()
    {
        isPathSet = false; 
    }

    private void DespawnCheck() //Should be done by server
    {
        if(closestPlayerDistance > despawnDistance)
        {
            Invoke("DespawnEnemy", despawnDuration);
        }else
		{
			if(IsInvoking("DespawnEnemy")){
				CancelInvoke("DespawnEnemy");
			}
		}
    }
    private void DespawnEnemy()
    {
        Destroy(this.gameObject, 5f);
    }
}