using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOffWallsScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 pastVelo = Vector2.zero;
    private float currentBounces;
    public float minVelocity;
    public int maxBounces = -1; //negative bounces means the projectile will live forever
    public float xBounceMult = 1;
    public float yBounceMult = 1;

    public float xSlowDown = 0;
    public float xMaxVelocity = -1;

    public bool useFallSpeed = false;
    public float maxFallSpeed = 10;
    //public bool noYBounce;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentBounces = maxBounces;
        pastVelo = rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        pastVelo = rb.velocity;
    }

    void FixedUpdate()
    {
        if (xSlowDown > 0 && xMaxVelocity >= 0 && Mathf.Abs(rb.velocity.x) > xMaxVelocity)
        {
            float newXVelo = rb.velocity.x + (xMaxVelocity - rb.velocity.x) * Time.fixedDeltaTime * xSlowDown;
            rb.velocity = new Vector2(newXVelo, rb.velocity.y);
        }
        if(useFallSpeed && rb.velocity.y < -1 * maxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, -1 * maxFallSpeed);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(maxBounces >= 0)
        {
            if (currentBounces <= 0)
            {
                Destroy(gameObject);
            }
            currentBounces--;
        }
        float speed = pastVelo.magnitude;
        Vector2 direction = Vector2.Reflect(pastVelo.normalized, collision.contacts[0].normal);
        Vector2 newVelo = direction * Mathf.Max(speed, minVelocity);
        /*
        if (noYBounce && direction.y > 0 && pastVelo.y < 0 && Mathf.Sign(direction.y) != Mathf.Sign(pastVelo.y))
        {
            direction = new Vector2(direction.x, 0);
        }
        */
        if (newVelo.x != pastVelo.x)
        {
            newVelo = new Vector2(newVelo.x * xBounceMult, newVelo.y);
        }
        if (newVelo.y != pastVelo.y)
        {
            newVelo = new Vector2(newVelo.x, newVelo.y * yBounceMult);
        }
        rb.velocity = newVelo;
    }
}
