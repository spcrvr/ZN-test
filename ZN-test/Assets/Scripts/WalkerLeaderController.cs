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
    private float despawnDuration = 1f;
    private Color colorA;
    private Stats stats;
 	void Awake () {
        Players = GameObject.FindGameObjectsWithTag("Player");
		PointsOfInterest = GameObject.FindGameObjectsWithTag("PointOfInterest");
        zomb_rigidbody = GetComponent<Rigidbody>();
        _navmeshagent = GetComponent<NavMeshAgent>();
        stats = GetComponent<Stats>(); 
    }
 	void Start() {
        stats.SetHP(Random.Range(150f,250f)); 
        colorA = GetComponent<Renderer>().material.color;
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
        DespawnCheck();
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
    { isPathSet = false; }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy") {
            Physics.IgnoreCollision(collision.collider, this.gameObject.GetComponent<Collider>());
        }
    }

    public void TakeDamage(float damage)
    {
        stats.DecreaseHP(damage);
        Color colorB = new Color(1f, colorA.g, colorA.b, 0.6f);      
        GetComponent<Renderer>().material.SetColor("_Color", colorB);
        Invoke("RestoreColor", 0.3f);   
    }

    void RestoreColor()
    { GetComponent<Renderer>().material.SetColor("_Color", colorA); }

    void DespawnCheck() //Should be done by server
    {
        if((closestPlayerDistance > despawnDistance) || (stats.GetHP() <= 0))
        {
            Invoke("DespawnEnemy", despawnDuration);
        }
    }

    void DespawnEnemy()
    { Destroy(this.gameObject);}
}