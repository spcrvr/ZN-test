using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour {
    public float day_speed;
    public float night_speed;
    void FixedUpdate () {
        if ((transform.rotation.eulerAngles.x > 30f) && (transform.rotation.eulerAngles.x < 160f))
        { transform.Rotate(Vector3.right, day_speed); }
        else
        {
            transform.Rotate(Vector3.right, night_speed);
        }
       // Debug.Log("Frametime: " + Time.fixedDeltaTime);
	}
}
