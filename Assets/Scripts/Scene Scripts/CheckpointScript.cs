using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public Transform checkpointPos;
    private CheckpointController checkController;
    // Start is called before the first frame update
    void Start()
    {
        checkController = FindObjectOfType<CheckpointController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (checkpointPos != null)
            {
                checkController.SetCheckpoint(checkpointPos);
            }
            else{
                checkController.SetCheckpoint(transform);
            }
            
        }
    }
}
