using UnityEngine;
using UnityEngine.AI; 
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
public class ZombMovement : MonoBehaviour {
    public GameObject[] Players;
    private GameObject _leader;
    private bool isLeaderInRange = false;
    public GameObject closestPlayer;
    public float AttackDistance = 5.0f;
    public float FollowDistance = 20.0f;
    private float roamRadius = 20.0f;
    public float closestPlayerDistance = Mathf.Infinity;
    [Range(0.0f, 1.0f)] public float AttackProbability = 0.5f;
    [Range(0.0f, 1.0f)] public float HitAccuracy = 0.5f;
    public float DamagePoints = 2.0f;
    public Animator _animator;
    public NavMeshAgent _navmeshagent;
    private Rigidbody zomb_rigidbody;
    public AudioClip AttackSound = null;
    public AudioSource m_Audio;
    [SerializeField] Vector3 finalPosition;
    [SerializeField] bool isPathSet = false;
    private float despawnDistance = 256f;
    private float despawnDuration = 3f;

 	void Awake () {
        Players = GameObject.FindGameObjectsWithTag("Player");
        m_Audio = GetComponent<AudioSource>();
        zomb_rigidbody = GetComponent<Rigidbody>();
        _navmeshagent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    void Start() {
        _navmeshagent.speed = Random.Range(0.5f,1.5f);
        this.transform.SetParent(GameObject.FindGameObjectWithTag("Enemies_parent").transform);
    }
	
	void FixedUpdate() {
        // _navmeshagent.SetDestination(player.gameObject.transform.position);
        if (_navmeshagent.enabled)
        {
            closestPlayer = GetClosestPlayer();
            bool chase = (closestPlayerDistance < FollowDistance);
            bool idle = (closestPlayerDistance > FollowDistance);
            if(idle)
            {
                // Debug.Log("Attempting to freeroam..."+this.name);
                if(isPathSet == false)
                    { 
                        if(_leader!=null)
                        {
                            FollowLeader(_leader);
                        } else {
                            FreeRoam();
                        }
                        
                    }
            }
            if(zomb_rigidbody.velocity.magnitude > 0)
            {
                _animator.SetBool("Idle", false);
                _animator.SetBool("IdleWalk", true);
            }else
            {
                _animator.SetBool("Idle", true);
                _animator.SetBool("IdleWalk", false);
            }
            
            if (closestPlayerDistance < AttackDistance)
            {
                Attack();
                _animator.ResetTrigger("Attack");
                _navmeshagent.SetDestination(this.gameObject.transform.position);
                _animator.SetTrigger("Attack");
            }

            if (chase)
            { _navmeshagent.SetDestination(closestPlayer.transform.position);}

              _animator.SetBool("Chase", chase);
              _animator.SetBool("Idle",   idle  );
            }
        DespawnCheck();
    }

    public void Attack()
    {
      // if (m_Audio != null){ m_Audio.PlayOneShot(AttackSound); }
        float random = Random.Range(0.0f, 1.0f);
        bool isHit = random > 1.0f - HitAccuracy;
        if (isHit)
        {
            // SendMessage -> poor performance
           // closestPlayer.SendMessage("TakeDamage", DamagePoints, SendMessageOptions.DontRequireReceiver);
           // closestPlayer.GetComponent<Health>().TakeDamage();
        }
    }
    
    public void FollowLeader(GameObject leader)
    {
        if(_leader != leader){
            _leader = leader;
        }
        isLeaderInRange = true;
        _navmeshagent.SetDestination(leader.transform.position);
        CancelInvoke("LeaderPresenceTimer");
        
    }

    private IEnumerator LeaderPresenceTimer()
    {
        yield return new WaitForSecondsRealtime(10f);
        isLeaderInRange = false;
        _leader = null;
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
    void FreeRoam()
    {    
        Vector3 randomDirection;
        NavMeshHit hit;
        // Give freeroam a 75% to actually follow the leader (if present)
            finalPosition = Vector3.zero;
            randomDirection = Random.insideUnitSphere * roamRadius;
            randomDirection += transform.position; // The game itself is a big terrarium for bugs
            NavMesh.SamplePosition(randomDirection, out hit, roamRadius, 1);
            finalPosition = hit.position;
               // _navmeshagent.destination = finalPosition;
            if(!isPathSet)
            { 
                _navmeshagent.SetDestination(finalPosition);
               // hasReachedDestination = false;
                isPathSet = true;
             }
               // Debug.Log("New position: "+ finalPosition.ToString());
            Vector3 Marker = new Vector3(finalPosition.x,finalPosition.y+2,finalPosition.z);
            Debug.DrawLine(finalPosition,Marker,Color.green, 12f);
            Invoke("HaltMovement", Random.Range(10f,30f));
    }

    private void HaltMovement()
    {
        isPathSet = false; 
    }
/*
    private void RoundValues()
    {
        rounded_NPC = new Vector3(Mathf.Round(this.transform.position.x*10f)/10f,
                                          Mathf.Round(this.transform.position.y*10f)/10f,
                                          Mathf.Round(this.transform.position.z*10f)/10f);
        rounded_Destination = new Vector3(Mathf.Round(finalPosition.x*10f)/10f,
                                          Mathf.Round(finalPosition.y*10f)/10f,
                                          Mathf.Round(finalPosition.z*10f)/10f);
    }
*/
    void DespawnCheck() //Should be done by server
    {
        if(closestPlayerDistance > despawnDistance)
        {
            Invoke("DespawnEnemy", despawnDuration);
        }

    }
    void DespawnEnemy()
    {
        Destroy(this.gameObject,0.5f);
    }
}