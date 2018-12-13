using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombMovementSimple : MonoBehaviour {
	public  GameObject[] Players;
    public  GameObject closestPlayer;
    private GameObject _leader;
    private bool isLeaderInRange = false;
    private bool hasReceivedCall = false;
    private bool isPathSet = false;
    public  float AttackDistance = 5.0f;
    public  float FollowDistance = 20.0f;
    private float roamRadius = 20.0f;
    public  float closestPlayerDistance = Mathf.Infinity;
    private float despawnDistance = 256f;
    private float despawnDuration = 1f;
    private NavMeshAgent _navmeshagent;
    private Rigidbody zomb_rigidbody;
    private Vector3 finalPosition;
    private Color colorA;
    private Stats stats;

 	void Awake () {
        Players = GameObject.FindGameObjectsWithTag("Player");
        zomb_rigidbody = GetComponent<Rigidbody>();
        _navmeshagent = GetComponent<NavMeshAgent>();
        stats = GetComponent<Stats>();
    }

    void Start() {
        stats.SetHP(Random.Range(50f,100f));
        colorA = GetComponent<Renderer>().material.color;
        _navmeshagent.speed = Random.Range(0.5f,1.5f);
        this.transform.SetParent(GameObject.FindGameObjectWithTag("Enemies_parent").transform);
    }
	
	void FixedUpdate() {

        if (_navmeshagent.enabled)
        {
            closestPlayer = GetClosestPlayer();
            bool chase = (closestPlayerDistance < FollowDistance);
            bool idle = (closestPlayerDistance > FollowDistance);
            if(idle)
            {
                if(isPathSet == false)
                    { 
                        FreeRoam();
                    }
            }
            if (closestPlayerDistance < AttackDistance)
            {
                _navmeshagent.SetDestination(this.gameObject.transform.position);
            }
            if (chase)
            { _navmeshagent.SetDestination(closestPlayer.transform.position);} }
        DespawnCheck();
    }
/* 
    public void FollowLeader(GameObject leader)
    {
           // hasReceivedCall = true;
            _leader = leader;
            _navmeshagent.SetDestination(leader.transform.position);
           // Invoke("LeaderPresenceTimer", 0.1f);
    }

    private IEnumerator LeaderPresenceTimer()
    {
        yield return new WaitForSecondsRealtime(10f);
        isLeaderInRange = false;
        _leader = null;
    }

    private float GetDistanceToLeader(GameObject leader)
    {
        float distance = 0f;
        Vector3 walkerPosition = transform.position;
        Vector3 leaderPosition = leader.transform.position; 
        Vector3 directionToTarget = leaderPosition - walkerPosition;
        distance = directionToTarget.magnitude;
        float distanceSqrToTarget = directionToTarget.sqrMagnitude;
        Debug.Log("distance: " + distance + " distanceSqr: " + distanceSqrToTarget);
        return distance;
    }
*/
    private GameObject GetClosestPlayer()
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

    private void FreeRoam()
    {    
        Vector3 randomDirection;
        NavMeshHit hit;
        // Give freeroam a 75% to follow the leader (if present)
            finalPosition = Vector3.zero;
            randomDirection = Random.insideUnitSphere * roamRadius;
            randomDirection += transform.position; // The game itself is a big terrarium for bugs
            NavMesh.SamplePosition(randomDirection, out hit, roamRadius, 1);
            finalPosition = hit.position;
            if(!isPathSet)
            { 
                _navmeshagent.SetDestination(finalPosition);
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

    

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
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
    {
        GetComponent<Renderer>().material.SetColor("_Color", colorA);
    }
    
    void DespawnCheck() //Should be done by server
    {
        if((closestPlayerDistance > despawnDistance) || (stats.GetHP() <= 0))
        {
            Invoke("DespawnEnemy", despawnDuration);
        }
    }

    void DespawnEnemy()
    {
        Destroy(this.gameObject);
    }
}
