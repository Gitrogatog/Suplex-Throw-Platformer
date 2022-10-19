using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    //public Camera cam;

    private bool jump = false;
    private bool endJump = false;
    private bool holdJump = false;
    private bool dash = false;
    private bool toss = false;

    private float horizontal = 0;
    private float vertical = 0;
    //private Vector2 mousePos;

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        //mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            endJump = true;
        }
        if (Input.GetButton("Jump"))
        {
            holdJump = true;
        }
        if (Input.GetButtonDown("Dash"))
        {
            dash = true;
        }
        if (Input.GetButtonDown("Throw"))
        {
            toss = true;
        }
    }

    void FixedUpdate()
    {
        //Vector2 lookDir = mousePos - transform.position;
        //float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        controller.Move(horizontal, vertical, jump, endJump, holdJump, dash, toss);
        jump = false;
        endJump = false;
        holdJump = false;
        dash = false;
        toss = false;
    }
}
