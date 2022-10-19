using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletScript : MonoBehaviour
{
    public float bulletSpeed;
    public int damage;
    private Rigidbody2D rb;
    public bool invertDir = true;
    public bool destroyOnHitEnemy = false;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.y != 0 || rb.velocity.x != 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 180 * Mathf.Atan2(rb.velocity.y, rb.velocity.x) / Mathf.PI);
        }
    }

    public void SetBulletDirection(float hori, float vert)
    {
        Debug.Log("H: " + hori + " V: " + vert);
        float multi = 1;
        if (invertDir)
        {
            multi *= -1;
        }
        if(hori != 0 && vert != 0)
        {
            multi = 1 / Mathf.Sqrt(2);
        }
        Vector2 bulletVelocity = new Vector3(multi * bulletSpeed * hori, multi * bulletSpeed * vert);
        rb.velocity = bulletVelocity;
    }

    public void DisableObject()
    {
        Destroy(gameObject);
    }
}
