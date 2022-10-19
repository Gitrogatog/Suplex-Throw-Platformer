using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int damage = 1;
    private GrabbableObjectScript itemScript;
    //public bool weakAllProjectiles = true; //Enemy will be damaged by any projectile
    //public string[] weaknesses;    //List of projectile types the enemy is weak to (only use if weakAllProjectile = false)
    //public bool killOnTouchPlayer = false;

    void Awake()
    {
        itemScript = GetComponentInParent<GrabbableObjectScript>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerHealthScript playerHealth = other.gameObject.GetComponent<PlayerHealthScript>();
            bool canBeGrabbed = itemScript != null;
            if (canBeGrabbed)
            {
                canBeGrabbed = itemScript.isGrabbable;
            }
            playerHealth.HitEnemy(damage, canBeGrabbed);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealthScript playerHealth = collision.gameObject.GetComponent<PlayerHealthScript>();
            bool canBeGrabbed = itemScript != null;
            if (canBeGrabbed)
            {
                canBeGrabbed = itemScript.isGrabbable;
            }
            playerHealth.HitEnemy(damage, canBeGrabbed);
        }
    }

    /*
    void OnCollisionEnter2D(Collision2D collision)
    {
        BulletScript bulletScript = collision.gameObject.GetComponent<BulletScript>();
        if(bulletScript != null)
        {
            if (!bulletScript.enemyProjectile)
            {
                if (weakAllProjectiles)
                {
                    healthScript.UpdateHealth(bulletScript.damage);
                }
                else
                {
                    bool shouldTakeDamage = false;
                    for (int i = 0; i < weaknesses.Length; i++)
                    {
                        if (collision.gameObject.tag == weaknesses[i])
                        {
                            shouldTakeDamage = true;
                        }
                    }
                    if (shouldTakeDamage)
                    {
                        healthScript.UpdateHealth(bulletScript.damage);
                    }
                }
                if (bulletScript.destroyOnHitEnemy)
                {
                    bulletScript.DisableObject();
                }
            }
        }
    }
    */
}
