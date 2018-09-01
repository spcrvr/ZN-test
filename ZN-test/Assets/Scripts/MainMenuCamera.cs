using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour {

    public Transform platform;

    void LateUpdate()
    {
        Vector3 angles = transform.eulerAngles;
        angles.y = 0;
        transform.eulerAngles = angles;
        transform.RotateAround(platform.position, Vector3.up, 0.2f);
        transform.LookAt(platform);
    }
}
