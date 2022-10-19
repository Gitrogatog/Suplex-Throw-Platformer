using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHoldingScript : MonoBehaviour
{
    public CharacterController2D controller;

    private Collider2D dashCollider;

    private bool isHolding = false;
    private bool isDashing = false;
    private int dashDir = 0;

    private float horizontal = 0;
    private float vertical = 0;

    private string itemName = "";
    private string scriptOnGrab = "none";
    private string scriptOnThrow = "none";
    private float currentLaunchSpeed = 0;
    private int currentPriority = 0; //higher priority objects will replace lower priority objects
    private bool refreshDash = false;

    private Sprite currentSprite;
    public SpriteRenderer rend;
    public Transform bulletSource;

    private GameObject projectile;
    private List<GrabbableObjectScript> grabList = new List<GrabbableObjectScript>();

    private void Awake()
    {
        dashCollider = GetComponent<Collider2D>();
    }

    public bool RecievePlayerState(float hori, float vert, bool dash, int dir, bool toss, bool facing, bool tookDamage)
    {
        /*
        if (projectile == null)
        {
            Debug.Log("No Held Item");
        }
        else
        {
            Debug.Log("Holding");
        }
        */
        dashDir = dir;
        isDashing = dash;
        dashCollider.enabled = isDashing;
        horizontal = hori;
        vertical = vert;
        if (horizontal == 0 && vertical == 0)
        {
            horizontal = 1;
            if (!facing)
            {
                horizontal *= -1;
            }
        }
        if (grabList.Count > 0)
        {
            int prio = 0;
            int index = 0;
            int chosenIndex = 0;
            if (!tookDamage)
            {
                foreach (var script in grabList)
                {
                    if (script.priority >= prio)
                    {
                        prio = script.priority;
                        chosenIndex = index;
                    }
                    index++;
                }
                GrabbableObjectScript chosenItem = grabList[chosenIndex];
                //Debug.Log("Second Check: " + chosenItem != null);
                if (!isHolding || currentPriority <= prio)
                {
                    UpdateHeldItem(chosenItem.itemName, chosenItem.scriptOnGrab, chosenItem.scriptOnThrow, chosenItem.launchSpeed, chosenItem.priority, chosenItem.refreshDash, chosenItem.holdSprite, chosenItem.throwProjectile);
                    Debug.Log("Grabbed Item " + itemName);
                }
                else
                {
                    Debug.Log("Item " + chosenItem.itemName + " Had Lower Priority");
                }
            }
            for (int i = grabList.Count - 1; i >= 0; i--)
            {
                GrabbableObjectScript removedItem = grabList[i];
                grabList.RemoveAt(i);
                removedItem.DisableObject();
            }
            controller.EndDash();
            isDashing = false;
            dashCollider.enabled = false;
        }
        if(toss && isHolding && !dash)
        {
            ThrowHeldItem();
            return true;
        }
        return false;
    }

    private void UpdateHeldItem(string name, string grabScript, string throwScript, float launchSpeed, int priority, bool refresh, Sprite newSprite, GameObject bullet)
    {
        isHolding = true;
        itemName = name;
        scriptOnGrab = grabScript;
        scriptOnThrow = throwScript;
        currentLaunchSpeed = launchSpeed;
        currentPriority = priority;
        refreshDash = refresh;
        currentSprite = newSprite;
        projectile = bullet;

        rend.sprite = currentSprite;
        
        Debug.Log(projectile != null);
    }

    private void ThrowHeldItem()
    {
        if(projectile != null)
        {
            GameObject proj = Instantiate(projectile, bulletSource.position, transform.rotation);
            float angle = Mathf.Atan2(vertical, horizontal);
            proj.BroadcastMessage("SpawnBullet", angle);
            Destroy(proj, .2f);
        }
        //PlayerBulletScript pScript = proj.GetComponent<PlayerBulletScript>();
        //pScript.SetBulletDirection(horizontal, vertical);
        ResetHeldObject();
    }

    /*
    void OnCollisionEnter2D(Collision2D collision)
    {
        GrabbableObjectScript itemScript = collision.gameObject.GetComponent<GrabbableObjectScript>();
        if (isDashing && itemScript != null && itemScript.isGrabbable)
        {
            bool isUnique = true;
            foreach (var script in grabList)
            {
                if (script.Equals(itemScript))
                {
                    isUnique = false;
                }
            }
            if (isUnique)
            {
                grabList.Add(itemScript);
            }
            ///////Start Old Stuff
            if (!isHolding || currentPriority <= itemScript.priority)
            {
                UpdateHeldItem(itemScript.itemName, itemScript.scriptOnGrab, itemScript.scriptOnThrow, itemScript.launchSpeed, itemScript.priority, itemScript.holdSprite, itemScript.throwProjectile);
                Debug.Log("Grabbed Item " + itemName);
            }
            else
            {
                Debug.Log("Item " + itemScript.itemName + " Had Lower Priority");
            }
            itemScript.DisableObject();
            ///////End Old Stuff
            controller.EndDash();
        }
        else
        {
            Debug.Log("Couldn't Grab Object");
        }
    }
    */

    void OnTriggerEnter2D(Collider2D col)
    {
        
        GrabbableObjectScript itemScript = col.gameObject.GetComponent<GrabbableObjectScript>();
        EnemyScript enemyScript = col.gameObject.GetComponent<EnemyScript>();
        if(itemScript == null)
        {
            GrabbableSubsetScript subsetScript = col.gameObject.GetComponent<GrabbableSubsetScript>();
            if(subsetScript != null)
            {
                itemScript = subsetScript.grabScript;
            }
        }
        if (isDashing && itemScript != null && itemScript.isGrabbable)
        {
            bool couldGrab = true;
            /*
            if (enemyScript != null)
            {
                if (dashDir == 1 && enemyScript.dashShieldLeft)
                {
                    couldGrab = false;
                }
                else if (dashDir == -1 && enemyScript.dashShieldRight)
                {
                    couldGrab = false;
                }
            }
            */
            if (couldGrab)
            {
                if(itemScript.throwProjectile != null)
                {
                    bool isUnique = true;
                    foreach (var script in grabList)
                    {
                        if (script.Equals(itemScript))
                        {
                            isUnique = false;
                        }
                    }
                    if (isUnique)
                    {
                        grabList.Add(itemScript);
                    }
                }
                else
                {
                    Debug.Log("No Item");
                    itemScript.DisableObject();
                }
            }
            else
            {
                Debug.Log("Blocked by DashShield");
            }
        }
        else
        {
            Debug.Log("Couldn't Grab Object");
        }
    }

    public float GetLaunchSpeed()
    {
        return currentLaunchSpeed;
    }

    public bool GetRefreshDash()
    {
        return refreshDash;
    }

    private void ResetHeldObject()
    {
        isHolding = false;
        itemName = "";
        scriptOnGrab = "none";
        scriptOnThrow = "none";
        currentPriority = 0;
        currentSprite = null;
        projectile = null;

        rend.sprite = currentSprite;
    }

    public void ResetHolding()
    {
        ResetHeldObject();
        for (int i = grabList.Count - 1; i >= 0; i--)
        {
            grabList.RemoveAt(i);
        }
    }
}
