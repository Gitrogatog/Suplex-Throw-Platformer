using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickstartVelocityScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float xStartVelo;
    public float yStartVelo;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(xStartVelo, yStartVelo);
    }
}
