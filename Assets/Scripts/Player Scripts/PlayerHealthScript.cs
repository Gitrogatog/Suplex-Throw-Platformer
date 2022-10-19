using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthScript : MonoBehaviour
{
    private CharacterController2D charController;
    private bool stunned = false;
    private bool isDashing = false;
    //private int dashDir = 0;
    public int maxHealth = 1;
    private int currentHealth = 1;
    //public int hazardDamage = 1;
    private CheckpointController checkpoint;
    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        charController = GetComponent<CharacterController2D>();
        checkpoint = FindObjectOfType<CheckpointController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        stunned = charController.GetStunnedState();
        isDashing = charController.GetDashState();
        //dashDir = charController.GetDashDir();
    }

    /*
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Kill")
        {
            UpdateHealth(currentHealth);
        }
        else if (!stunned)
        {
            EnemyScript enemyScript = collision.gameObject.GetComponent<EnemyScript>();
            BulletScript bulletScript = collision.gameObject.GetComponent<BulletScript>();
            GrabbableObjectScript itemScript = collision.gameObject.GetComponent<GrabbableObjectScript>();
            if (enemyScript != null)
            {
                if (itemScript == null)
                {
                    UpdateHealth(enemyScript.damage);
                }
                else if (!isDashing || !itemScript.isGrabbable)
                {
                    UpdateHealth(enemyScript.damage);
                }
            }
            else if (bulletScript != null && bulletScript.enemyProjectile)
            {
                if (itemScript == null)
                {
                    UpdateHealth(enemyScript.damage);
                }
                else if (!isDashing || !itemScript.isGrabbable)
                {
                    UpdateHealth(enemyScript.damage);
                }
            }
            else if (collision.gameObject.tag == "Hazard")
            {
                UpdateHealth(hazardDamage);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Gamer");
        if (other.tag == "Kill")
        {
            UpdateHealth(currentHealth);
        }
        else if (!stunned)
        {
            EnemyScript enemyScript = other.gameObject.GetComponent<EnemyScript>();
            BulletScript bulletScript = other.gameObject.GetComponent<BulletScript>();
            GrabbableObjectScript itemScript = other.gameObject.GetComponent<GrabbableObjectScript>();
            if (enemyScript != null)
            {
                if (itemScript == null)
                {
                    UpdateHealth(enemyScript.damage);
                }
                else if (!isDashing || !itemScript.isGrabbable)
                {
                    UpdateHealth(enemyScript.damage);
                }
            }
            else if (bulletScript != null && bulletScript.enemyProjectile)
            {
                if (itemScript == null)
                {
                    UpdateHealth(enemyScript.damage);
                }
                else if (!isDashing || !itemScript.isGrabbable)
                {
                    UpdateHealth(enemyScript.damage);
                }
            }
            else if (other.tag == "Hazard")
            {
                UpdateHealth(hazardDamage);
            }
        }
    }
    */

    public void HitEnemy(int damage, bool isGrabbable)
    {
        if((isGrabbable && isDashing) || stunned)
        {
            Debug.Log("Damage Prevented");
        }
        else
        {
            UpdateHealth(damage);
        }
    }

    private void UpdateHealth(int damageTaken)
    {
        currentHealth -= damageTaken;
        Debug.Log(currentHealth);
        stunned = true;
        if (currentHealth <= 0)
        {
            Transform check = checkpoint.ResetLevel();
            charController.ResetPlayer(check);
            currentHealth = maxHealth;
        }
        else
        {
            charController.RecieveDamage();
        }
    }
}
