using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpeedBoost : MonoBehaviour
{
    public float boostSpeed;
    public float boostAngle;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("v"))
        {
            rb.velocity = new Vector2(boostSpeed * Mathf.Cos(Mathf.PI * boostAngle / 180), boostSpeed * Mathf.Sin(Mathf.PI * boostAngle / 180));
        }
    }
}
