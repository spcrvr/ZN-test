using UnityEngine;
using UnityEngine.AI; 
using UnityEngine.Networking;
using System.Collections;
public class ZombMovement : MonoBehaviour {
    public GameObject[] Players;
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
  //  [SerializeField] Vector3 rounded_NPC;
  //  [SerializeField] Vector3 rounded_Destination;
    private float despawnDistance = 128f;
    private float despawnDuration = 5f;

 	void Awake () {
        Players = GameObject.FindGameObjectsWithTag("Player");
        m_Audio = GetComponent<AudioSource>();
        zomb_rigidbody = GetComponent<Rigidbody>();
        _navmeshagent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }
	
	void FixedUpdate() {
        // _navmeshagent.SetDestination(player.gameObject.transform.position);
        if (_navmeshagent.enabled)
        {
            closestPlayer = GetClosestPlayer();
            bool attack = false;
            bool follow = (closestPlayerDistance < FollowDistance);
            bool idle = (closestPlayerDistance > FollowDistance);
            if(idle)
            {
                // Debug.Log("Attempting to freeroam..."+this.name);
                if(isPathSet == false)
                    { FreeRoam(); }
            }
            if(zomb_rigidbody.velocity.magnitude > 0)
            {
                _animator.SetBool("Idle", false);
            }

            if (follow)
            {
                if (closestPlayerDistance < AttackDistance)
                {
                    attack = true;
                }
            }

            if (follow)
            { _navmeshagent.SetDestination(closestPlayer.transform.position);}

           // if (!follow || attack)
           if(attack)
            {
                _navmeshagent.SetDestination(this.gameObject.transform.position);
                Attack();
            }
        
            _animator.SetBool("Attack", attack);
            _animator.SetBool("Follow", follow);
            _animator.SetBool("Idle",   idle  );
        }
        DespawnCheck();
    }

    public void Attack()
    {
      // if (m_Audio != null){ m_Audio.PlayOneShot(AttackSound); }
        float random = Random.Range(0.0f, 1.0f);
        // The higher the accuracy is, the more likely the player will be hit
        bool isHit = random > 1.0f - HitAccuracy;
        if (isHit)
        {
            closestPlayer.SendMessage("Damage", DamagePoints, SendMessageOptions.DontRequireReceiver);
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
    void FreeRoam()
    {    
        Vector3 randomDirection;
        NavMeshHit hit;
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
               // Debug.Log("Destination reached by "+this.name);
         //   }else { hasReachedDestination = false; }   
/* 
        if(_navmeshagent.velocity == Vector3.zero)
            {
                hasReachedDestination = true;
                Invoke("HaltMovement", 4f);
            }*/
    }

    private void HaltMovement()
    {
        isPathSet = false; 
    //    hasReachedDestination = true;
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
