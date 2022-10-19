using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePadScript : MonoBehaviour
{
    public float bounceSpeed;
    public bool refreshDash = false;
    public bool disableOnBounce = true;
    private bool bounceable = true;
    private ObjectEnablerScript enablerScript;
    private GrabbableObjectScript grabScript;
    // Start is called before the first frame update
    void Awake()
    {
        enablerScript = GetComponentInParent<ObjectEnablerScript>();
        grabScript = GetComponentInParent<GrabbableObjectScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && bounceable)
        {
            CharacterController2D charController = other.gameObject.GetComponent<CharacterController2D>();
            if (grabScript == null || !grabScript.isGrabbable || !charController.GetDashState())
            {
                charController.EndDash();
                charController.Bounce(bounceSpeed, refreshDash);
                if(disableOnBounce && enablerScript != null)
                {
                    enablerScript.DisableObject();
                }
            }
        }
    }
}
