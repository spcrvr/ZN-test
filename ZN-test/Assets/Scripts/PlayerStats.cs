using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
	
	void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Walker_enemy")
        {
            Physics.IgnoreCollision(collision.collider, this.gameObject.GetComponent<Collider>());
        }
    }
	
}
