using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private RaycastHit hit;
    private int clipSize;
    private int loadedAmmo;
    private int ammoPool;
    public AudioSource firingSoundSource;
    public AudioClip firingSoundClip;
    public AudioClip emptyFireClip;
    public AudioClip reloadClip;
    public Transform FirePos;
    private float _bulletDamage;
    private float _fireDelay = 0.7f;
    private float _startTime;
    private bool canFire;
    private bool waitingCheck;

    private void Awake()
    {
        _startTime = Time.time;
        canFire = false;
        _bulletDamage = 30f;
        clipSize = 12;
        ammoPool = 36;
        loadedAmmo = 0;
        firingSoundSource = GetComponent<AudioSource>();
        firingSoundSource.playOnAwake = false;
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(loadedAmmo == 0)
            {
                canFire = false;
                firingSoundSource.PlayOneShot(emptyFireClip);
            }
            else if((canFire)&&(loadedAmmo > 0))
            {
                if ((Time.time - _startTime) > _fireDelay)
                {
                    firingSoundSource.PlayOneShot(firingSoundClip);
                    Fire();
                }
            }
        }
        if ((Input.GetKeyDown(KeyCode.R))&&(!firingSoundSource.isPlaying))
        { 
            firingSoundSource.PlayOneShot(reloadClip);
            StartCoroutine(Reload()); 
        }
            
    }

    void Fire()
    {
        loadedAmmo -= 1;
        _startTime = Time.time;
        Vector3 direction = FirePos.forward.normalized;
        if (Physics.Raycast(FirePos.position, direction, out hit))
        {
            if ((hit.transform.gameObject != null) && (hit.transform.gameObject.CompareTag("Enemy")))
            {
                if (hit.transform.GetComponent<ZombMovementSimple>())
                {
                    hit.transform.GetComponent<ZombMovementSimple>().TakeDamage(_bulletDamage);
                }
                else
                {
                    hit.transform.GetComponent<WalkerLeaderController>().TakeDamage(_bulletDamage);
                }

            }
        }
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSecondsRealtime(3.2f);
        if ((ammoPool > 0)&&(loadedAmmo < clipSize))
        {
            if ((ammoPool-clipSize) >= 0)
            {
                loadedAmmo += clipSize;
                ammoPool -= clipSize;
            }
            else
            {
                loadedAmmo += ammoPool;
            }
        }
        canFire = true;
    }

    private bool Wait()
    {
        waitingCheck = false;
        StartCoroutine(SoundDelay());
        return waitingCheck;
    }

    private IEnumerator SoundDelay()
    {
        yield return new WaitForSeconds(2f);
        waitingCheck = true;
    }

}
