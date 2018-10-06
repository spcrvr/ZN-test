using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeControl : MonoBehaviour {
    public bool canGatherHorde = true;
    private void FixedUpdate()
    {
        InvokeRepeating("RefreshHordeTimer",0.5f,5f);
    }

	private void RefreshHordeTimer()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f, 10);
        int i = 0;
        while (i < hitColliders.Length)
        {
            hitColliders[i].GetComponent<ZombMovement>().FollowLeader(this.gameObject);
            i++;
        }
	}
}
