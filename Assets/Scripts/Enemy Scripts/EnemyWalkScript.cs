using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkScript : MonoBehaviour
{
    [SerializeField] private LayerMask WhatIsWall;
    [SerializeField] private BoxCollider2D boxCol;
    private Rigidbody2D rb;
    public float walkVelo;
    //public float yStartVelo;

    public bool facingRight = true;

    private ObjectEnablerScript enablerScript;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enablerScript = GetComponent<ObjectEnablerScript>();
        rb.velocity = new Vector2(walkVelo, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enablerScript.IsEnabled())
        {
            Vector2 facingVector = new Vector2(GetFacingInt(), 0);
            RaycastHit2D wallRaycastHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0f, facingVector, .1f, WhatIsWall);
            if (wallRaycastHit.collider != null)
            {
                Flip();
            }
            rb.velocity = new Vector2(walkVelo * GetFacingInt(), rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private int GetFacingInt()
    {
        if (facingRight)
        {
            return 1;
        }
        return -1;
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
