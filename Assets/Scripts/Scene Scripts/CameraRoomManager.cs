﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRoomManager : MonoBehaviour
{
    public GameObject virtualCam;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !other.isTrigger)
        {
            virtualCam.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && !other.isTrigger)
        {
            virtualCam.SetActive(false);
        }
    }
}