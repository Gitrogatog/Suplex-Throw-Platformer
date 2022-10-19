using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObjectScript : MonoBehaviour
{

    public bool isGrabbable = true;
    public string itemName = "";
    public string scriptOnGrab = "none";
    public string scriptOnThrow = "none";
    public float launchSpeed = 0; //the speed the player is launched after throwing the object.
    public int priority = 0; //higher priority objects will replace lower priority objects
    public bool refreshDash = false;

    //public bool dashShieldLeft = false;
    //public bool dashShieldRight = false;

    private ObjectEnablerScript objectEnabler;
    public Sprite holdSprite;
    public GameObject throwProjectile;

    void Awake()
    {
        objectEnabler = GetComponent<ObjectEnablerScript>();
    }

    public void ResetObject()
    {

    }

    public void DisableObject()
    {
        objectEnabler.DisableObject();
    }
}
