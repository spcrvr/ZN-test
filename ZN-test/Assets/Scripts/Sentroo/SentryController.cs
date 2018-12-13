using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    - Find a way to reuse code -> different enemy types, common behavior with different values
    - Redo Player FPS controller
 */
public class SentryController : MonoBehaviour {
    [SerializeField] private bool _isActive;
    [SerializeField] private int _loadedAmmoCount = 0;
    private float _fireDelay = 0.1f;
    private float _startTime; // For firedDelay
    [SerializeField] private float _lastScanTime = 0; // For FindEnemies()
    private float _bulletDamage = 12f;
    private float range = 64f;
    
    public Transform firingPosition;
    public AudioSource firingSoundSource;
    public AudioClip firingSoundClip;
    private RaycastHit hit;
    private Vector3 currentPosition;
    private Vector3 distanceToEnemy;
    private Transform _bestTarget = null;
    private Transform bestTarget;
    private Quaternion lookRotation;
    private List<GameObject> EnemiesInRange;
    private GameObject[] Enemies;
    [SerializeField] private int enemyCount = 0;
    [SerializeField] private float _currentDistance = 0;
    private Stats stats;
    private void Awake()
    {
        EnemiesInRange = new List<GameObject>();
        stats = GetComponent<Stats>();
        firingSoundSource.playOnAwake = false;
        _isActive = true;
        firingSoundSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        stats.SetHP(100f);
        this.gameObject.transform.rotation = Quaternion.identity;
        LoadAmmo(10000);
    }
    private void FixedUpdate(){
        // _isActive = ((_loadedAmmoCount > 0)&&(_hp > 0));
        currentPosition = this.transform.position;
        
        if (_isActive){
            FindEnemies(range); // Find enemies by tag
            if (EnemiesInRange.Count > 0)
            { 
                GetClosestEnemy();
                Transform target = _bestTarget;
                if (target != null)
                {
                    RotateTurret(target);
                    Fire(target); 
                }
            }
        } else{
            this.gameObject.transform.rotation = Quaternion.identity;
        }
        if (EnemiesInRange.Count > 0) { enemyCount = EnemiesInRange.Count;}
    }
    
    private void FindEnemies(float range)
    {
        if ((Time.time - _lastScanTime) > 3f ) {
            _lastScanTime = Time.time;
            if (GameObject.FindGameObjectsWithTag("Enemy").Length != 0)
            {   
                // Purge Enemies array here
                Enemies = new GameObject[]{};
                EnemiesInRange.Clear();
                Enemies = GameObject.FindGameObjectsWithTag("Enemy");
                if(Enemies.Length > 0)
                {
                    for (int i = 0; i < Enemies.Length; i++)
                    {
                        if (Enemies[i] != null)
                        {
                            distanceToEnemy = Enemies[i].transform.position - currentPosition;
                            if (distanceToEnemy.magnitude < range)
                            {
                                EnemiesInRange.Add(Enemies[i]);
                            }
                        }
                    }
                }
            }
        }
    }

    private void GetClosestEnemy()
    {
        float closestDistanceSqr = Mathf.Infinity;
       // Vector3 currentPosition = transform.position;
        bestTarget = null;
        Vector3 directionToTarget = Vector3.zero;
            foreach (GameObject potentialTarget in EnemiesInRange)
            {
                if ((potentialTarget == null)||(EnemiesInRange.Count == 0))
                {
                    return;
                }
                directionToTarget = potentialTarget.transform.position - currentPosition;
                float closestEnemyDistance = directionToTarget.magnitude;
                _currentDistance = directionToTarget.magnitude;
                if(directionToTarget.magnitude < range)
                {
                    float distanceSqrToTarget = directionToTarget.sqrMagnitude;
                    if (distanceSqrToTarget < closestDistanceSqr)
                    {
                        closestDistanceSqr = distanceSqrToTarget;
                        bestTarget = potentialTarget.transform;
                    }
                }
            }
        _currentDistance = directionToTarget.magnitude;
        if (bestTarget != null)
        {
            _bestTarget = bestTarget.transform;
        }
    }

    private void RotateTurret(Transform target)
    {
        if (target != null) {
        // Rotate turret from old to new rotation by certain amount with each update
           // Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position), 0.15F);
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.SetLookRotation(target.transform.position, Vector3.up), 0.15F);
            Vector3 direction = (target.position - currentPosition).normalized; 
            lookRotation = Quaternion.LookRotation(direction); 
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.15f);
        }
    }
    private void LoadAmmo(int count)
    {
        _loadedAmmoCount += count;
        // Return remaining ammo ?
    }

    private void Fire(Transform target)
    {
        // Fire here
        if ((target != null) && (_loadedAmmoCount > 0) && ((Time.time-_startTime)>_fireDelay)) {
            _startTime = Time.time;
            firingSoundSource.PlayOneShot(firingSoundClip);
            _loadedAmmoCount -= 1;
            TraceRound(target);
        }
    }
    private void TraceRound(Transform target)
    {
        if(target == null){
            return;
        }
        
        Vector3 direction = (target.position - transform.position).normalized;
        if(Physics.Raycast(firingPosition.transform.position,direction,out hit)){
            if((hit.transform.gameObject != null) && (hit.transform.gameObject.CompareTag("Enemy")))
            {
                if(hit.transform.GetComponent<ZombMovementSimple>()){
                    hit.transform.GetComponent<ZombMovementSimple>().TakeDamage(_bulletDamage);
                } else
                {
                    hit.transform.GetComponent<WalkerLeaderController>().TakeDamage(_bulletDamage);
                }
                
            } 
        } 
    }

    void DespawnCheck()
    {
        if (stats.GetHP() <= 0)
        { Destroy(this.gameObject); }
    }
}
