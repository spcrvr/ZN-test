using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    enum DoorState
    {
        OpenLeft,
        OpenRight,
        Closed
    }
    public bool isOpen = false;
    
    private void Awake()
    {
        
    }

    public void ToggleDoor()
    {
        if (isOpen)
        {
            CloseDoor();
            isOpen = false;
        }
        else {
            OpenDoor();
            isOpen = true;
        }
    }

    private void OpenDoor()
    {

    }

    private void CloseDoor()
    {

    }
}
