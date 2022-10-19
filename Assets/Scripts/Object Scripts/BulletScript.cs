using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    //public float bulletSpeed;
    public int damage;
    private Rigidbody2D rb;
    public bool invertDir = true;
    public bool enemyProjectile = false;
    public bool destroyOnHitEnemy = false;
    public bool doesTurn = true;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (doesTurn && (rb.velocity.y != 0 || rb.velocity.x != 0))
        {
            transform.eulerAngles = new Vector3(0, 0, 180 * Mathf.Atan2(rb.velocity.y, rb.velocity.x) / Mathf.PI);
        }
    }
    /*
    public void SetBulletDirection(float hori, float vert)
    {
        Debug.Log("H: " + hori + " V: " + vert);
        float multi = 1;
        if (invertDir)
        {
            multi *= -1;
        }
        if (hori != 0 && vert != 0)
        {
            multi = 1 / Mathf.Sqrt(2);
        }
        Vector2 bulletVelocity = new Vector3(multi * bulletSpeed * hori, multi * bulletSpeed * vert);
        rb.velocity = bulletVelocity;
    }
    */
    public void DisableObject()
    {
        Destroy(gameObject);
    }
}
