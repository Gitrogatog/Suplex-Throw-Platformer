using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamageScript : MonoBehaviour
{
    private ObjectHealthScript healthScript;
    private ObjectEnablerScript enablerScript;
    private EnemyScript enemyScript;
    public bool weakAllProjectiles = true; //Enemy will be damaged by any projectile
    public string[] weaknesses;    //List of projectile types the enemy is weak to (only use if weakAllProjectile = false)

    void Awake()
    {
        healthScript = GetComponentInParent<ObjectHealthScript>();
        enablerScript = GetComponentInParent<ObjectEnablerScript>();
        enemyScript = GetComponent<EnemyScript>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        BulletScript bulletScript = collision.gameObject.GetComponent<BulletScript>();
        if (bulletScript != null)
        {
            if (!bulletScript.enemyProjectile || enemyScript == null)
            {
                if (weakAllProjectiles)
                {
                    TakeDamage(bulletScript.damage);
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
                        TakeDamage(bulletScript.damage);
                    }
                }
                if (bulletScript.destroyOnHitEnemy)
                {
                    bulletScript.DisableObject();
                }
            }
        }
    }

    private void TakeDamage(int damage)
    {
        if(healthScript != null)
        {
            healthScript.UpdateHealth(damage);
        }
        else if(enablerScript != null)
        {
            enablerScript.DisableObject();
        }
    }
}
