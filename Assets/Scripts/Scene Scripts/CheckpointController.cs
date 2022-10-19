using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    private Transform currentCheckpoint;
    public GameObject reseter;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCheckpoint(Transform newPoint)
    {
        currentCheckpoint = newPoint;
    }

    public Transform ResetLevel()
    {
        reseter.BroadcastMessage("EnableObject");
        return currentCheckpoint;
    }
}
