using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    //[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    //[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
    //[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private LayerMask m_WhatIsWall;                          // A mask determining what is ground to the character
    [SerializeField] private BoxCollider2D boxCol;
    [SerializeField] private Transform groundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform wallCheck;                           // A position marking where to check if the player is touching a wall

    const float k_GroundedRadius = .45f; // Radius of the overlap circle to determine if grounded .2
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Rigidbody2D rb;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;
    const float wallCheckRadius = .45f;
    private bool isTouchingWall = false;

    public float moveSpeed;
    public float runAccel = 10;
    public float runReduce = 10;
    public float runDiffSnap;
    public float airMult;

    public float jumpSpeed;
    public float endJumpMult;
    public float jumpHBoost;
    public float baseGravity; 
    public float maxFallSpeed;
    //public float fallReduce = 10;
    public float fallDiffSnap;
    public float upMaxSpeed;
    //public float upReduce = 20;
    //public float baseFallReduceTime;

    //public float horiAccel;
    public float upAccel;
    public float downAccel;

    private bool isWallSliding = false;
    public float wallJumpSpeedX;
    public float wallJumpSpeedY;
    public float wallJumpApplyTime;
    public float wallSlideFallSpeed;
    public float wallSlideMult;

    private float lockXDirTimer;
    private float lockXDir = 0;

    public float hangJumpSpeedX;
    public float hangJumpSpeedY;

    public float jumpInputHoldTime;
    private float jumpInputTimer;
    public float maxCoyoteTime;
    private float coyoteTimer;

    //private string currentState = "idle";
    enum PlayerState { Idle, Jump, Dash, Launch, Hang};
    PlayerState currentState = PlayerState.Idle;
    private bool stunned = false;
    public float maxStunTime;
    private float stunTimer;

    private float dashTimer;
    public float startDashTime;
    public float dashSpeed;
    public float endDashSpeed;
    private bool canDash = true;
    private int dashDir;

    private float launchTimer;
    public float launchDashTime;
    //public float launchSpeed;
    public float endLaunchSpeed;
    private Vector2 launchVector;

    public float upLaunchMultiplier = .75f;
    public float horiLaunchMultiplier = 1.5f;

    public Animator animator;

    public PlayerHoldingScript holdScript;
    private RingHangTileScript ringScript;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;
        isTouchingWall = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        RaycastHit2D groundRaycastHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0f, Vector2.down, .1f, m_WhatIsGround);
        if (groundRaycastHit.collider != null)
        {
            m_Grounded = true;
        }
        
        /*
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }

        }
        */
        if (wasGrounded && !m_Grounded && currentState != PlayerState.Jump)
        {
            coyoteTimer = maxCoyoteTime;
        }
        Vector2 facingVector = new Vector2(GetFacingInt(), 0);
        RaycastHit2D wallRaycastHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0f, facingVector, .1f, m_WhatIsWall);
        if (wallRaycastHit.collider != null)
        {
            isTouchingWall = true;
        }
        /*
        Collider2D[] wallCols = Physics2D.OverlapCircleAll(wallCheck.position, wallCheckRadius, m_WhatIsGround);
        for (int i = 0; i < wallCols.Length; i++)
        {
            if (wallCols[i].gameObject != gameObject)
            {
                isTouchingWall = true;
            }
        }
        */
    }


    public void Move(float hori, float vert, bool jump, bool endJump, bool holdJump, bool dash, bool toss)
    {
        float horizontalMove = hori;
        if(lockXDirTimer > 0)
        {
            lockXDirTimer -= Time.fixedDeltaTime;
            horizontalMove = lockXDir;
        }
        if (stunned)
        {
            stunTimer -= Time.fixedDeltaTime;
            if(stunTimer <= 0)
            {
                stunned = false;
            }
        }
        if(canDash && dash && currentState != PlayerState.Dash && !stunned) //begins dash
        {
            StartDash();
        }
        if (currentState == PlayerState.Dash) //moves player forwards if dashing
        {
            dashTimer -= Time.fixedDeltaTime;
            if(dashTimer <= 0)
            {
                EndDash();
            }
            else
            {
                Vector2 targetVelocity = new Vector2(dashSpeed * dashDir, 0);
                rb.velocity = targetVelocity;
            }
        }
        else if(!canDash && m_Grounded)
        {
            canDash = true;
        }
        if(isTouchingWall && !m_Grounded && !GetDashState() && !stunned) //isWallSliding check
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
        if(jumpInputTimer > 0)
        {
            jumpInputTimer -= Time.fixedDeltaTime;

        }
        if (coyoteTimer > 0)
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }
        if(currentState == PlayerState.Launch)
        {
            rb.velocity = launchVector;
            launchTimer -= Time.fixedDeltaTime;
            if(launchTimer <= 0)
            {
                currentState = PlayerState.Idle;
                rb.velocity = launchVector.normalized * endLaunchSpeed;
                if(rb.velocity.y > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * upLaunchMultiplier);
                }
            }
        }
        else if (currentState != PlayerState.Dash && currentState != PlayerState.Hang) //Checks if player is jumping
        {
            if (jump || jumpInputTimer > 0)
            {
                if (m_Grounded || coyoteTimer > 0)
                {
                    m_Grounded = false;
                    rb.velocity = new Vector2(rb.velocity.x + jumpHBoost * hori, jumpSpeed);
                    currentState = PlayerState.Jump;
                    jumpInputTimer = 0;
                    coyoteTimer = 0;
                }
                else if (isWallSliding)
                {
                    StartWallJump();
                    jumpInputTimer = 0;
                    coyoteTimer = 0;
                }
                else if(jumpInputTimer <= 0)
                {
                    jumpInputTimer = jumpInputHoldTime;
                }
            }
            
            float gravity = baseGravity;

            if (currentState == PlayerState.Jump) //ends jump or reduces speed if let go
            {
                if(rb.velocity.y <= 0)
                {
                    currentState = PlayerState.Idle;
                }
                else if(!holdJump && rb.velocity.y > 0)
                {
                    currentState = PlayerState.Idle;
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * endJumpMult);
                }
            }



            /*
            float xFrict = 1;
            if (!m_Grounded)
            {
                xFrict = airMult;
            }
            if (currentState == "wallJump")
            {
                targetXVelo = wallJumpDir * wallJumpSpeedX;
                xNewVelo = targetXVelo;
                wallJumpTimer -= Time.fixedDeltaTime;
            }
            else
            {
                xNewVelo += GetSign(targetXVelo - rb.velocity.x) * horiAccel * Time.fixedDeltaTime * xFrict;
                if((xNewVelo > targetXVelo && rb.velocity.x <= targetXVelo) || (xNewVelo < targetXVelo && rb.velocity.x >= targetXVelo))
                {
                    xNewVelo = targetXVelo;
                }
            }
            rb.velocity = new Vector2(xNewVelo, yNewVelo);
            */

            //Lerping player's vertical movement
            /*
            float yFrict = 1;
            float maxFall = maxFallSpeed;
            if (isWallSliding)
            {
                yFrict = wallSlideMult;
                maxFall = wallSlideFallSpeed;
            }
            if(rb.velocity.y < -1 * maxFall) //slows player if falling faster than max fall speed
            {
                float targetVelocity = -1 * maxFall;
                float newYVelo = rb.velocity.y + (targetVelocity - rb.velocity.y) * Time.fixedDeltaTime * fallReduce * yFrict;
                if(rb.velocity.y + fallDiffSnap >= targetVelocity)
                {
                    newYVelo = targetVelocity;
                }
                rb.velocity = new Vector2(rb.velocity.x, newYVelo);
                
            }
            else if(rb.velocity.y > upMaxSpeed) //slows player if moving faster than max upwards speed
            {
                float targetVelocity = upMaxSpeed;
                float newYVelo = rb.velocity.y + (targetVelocity - rb.velocity.y) * Time.fixedDeltaTime * upReduce * yFrict - gravity * Time.fixedDeltaTime;
                rb.velocity = new Vector2(rb.velocity.x, newYVelo);
            }
            else //applies normal gravity
            {
                float currentVelo = rb.velocity.y;
                currentVelo -= gravity * Time.fixedDeltaTime;
                if (currentVelo < -1 * maxFall)
                {
                    currentVelo = -1 * maxFall;
                }
                rb.velocity = new Vector2(rb.velocity.x, currentVelo);
            }
            */
            float maxFall = maxFallSpeed;
            if (isWallSliding)
            {
                maxFall = wallSlideFallSpeed;
            }
            //float targetXVelo = moveSpeed * hori;
            float targetYVelo = -1 * maxFall;
            //float xNewVelo = rb.velocity.x;
            float yNewVelo = rb.velocity.y;
            if (rb.velocity.y < -1 * maxFall)
            {
                yNewVelo += downAccel * Time.fixedDeltaTime;
            }
            else if (rb.velocity.y > upMaxSpeed)
            {
                yNewVelo -= gravity * Time.fixedDeltaTime;
                if(yNewVelo > upMaxSpeed)
                {
                    yNewVelo += (upMaxSpeed - yNewVelo) * Time.fixedDeltaTime * upAccel;
                }
                if (yNewVelo < targetYVelo)
                {
                    yNewVelo = targetYVelo;
                }
            }
            else
            {
                yNewVelo -= gravity * Time.fixedDeltaTime;
                if (yNewVelo < targetYVelo)
                {
                    yNewVelo = targetYVelo;
                }
            }
            rb.velocity = new Vector2(rb.velocity.x, yNewVelo);

            //Lerping player's horizontal movement
            float frict = 1;
            if (!m_Grounded)
            {
                frict = airMult;
            }
            float targetXVelo = horizontalMove * moveSpeed;
            float newXVelo = 0;
            if (Mathf.Abs(rb.velocity.x) > moveSpeed && GetSign(rb.velocity.x) == GetSign(horizontalMove) && horizontalMove != 0) //deccelerates player by runreduce if player is moving in same direction as their velocity // 
            {

                newXVelo = rb.velocity.x + (targetXVelo - rb.velocity.x) * Time.fixedDeltaTime * runReduce * frict;
                /*
                if (targetVelocity < 0 && rb.velocity.x < 0 && rb.velocity.x + runDiffSnap >= targetVelocity)
                {
                    newXVelo = targetVelocity;
                }
                else if(targetVelocity > 0 && rb.velocity.x > 0 && rb.velocity.x - runDiffSnap <= targetVelocity)
                {
                    newXVelo = targetVelocity;
                }
                */
            }
            else //Normal running acceleration
            {
                newXVelo = rb.velocity.x + (targetXVelo - rb.velocity.x) * Time.fixedDeltaTime * runAccel * frict;
                
            }
            rb.velocity = new Vector2(newXVelo, rb.velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (horizontalMove > 0 && !m_FacingRight && !GetDashState())
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (horizontalMove < 0 && m_FacingRight && !GetDashState())
            {
                // ... flip the player.
                Flip();
            }
        }
        
        if(currentState == PlayerState.Hang && jump) //Ends hang via jump
        {
            EndHang();
            currentState = PlayerState.Jump;
            rb.velocity = new Vector2(hangJumpSpeedX * hori, hangJumpSpeedY);
        }
        
        bool didToss = holdScript.RecievePlayerState(hori, vert, GetDashState(), GetDashDir(), toss, m_FacingRight, stunned); //checks if player threw something

        if (didToss)
        {
            LaunchPlayer(hori, vert);
        }
        //tookDamage = false;
        //Debug.Log("H: " + rb.velocity.x + " V: " + rb.velocity.y);
        //Debug.Log(currentState);
    }

    void Update()
    {
        animator.SetFloat("Horizontal", rb.velocity.x);
        animator.SetFloat("Vertical", rb.velocity.y);
        animator.SetBool("CanDash", canDash);
    }

    public bool GetDashState()
    {
        return currentState == PlayerState.Dash;
    }

    public int GetDashDir()
    {
        return dashDir;
    }

    public bool GetStunnedState()
    {
        return stunned;
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private int GetFacingInt()
    {
        int value = 1;
        if (!m_FacingRight)
        {
            value = -1;
        }
        return value;
    }

    private int GetSign(float value)
    {
        if(value < 0)
        {
            return -1;
        }
        else if(value > 0)
        {
            return 1;
        }
        return 0;
    }

    private void StartDash()
    {
        if (currentState == PlayerState.Hang)
        {
            EndHang();
        }
        dashTimer = startDashTime;
        currentState = PlayerState.Dash;
        canDash = false;
        dashDir = GetFacingInt();
    }

    public void EndDash()
    {
        if(currentState == PlayerState.Dash)
        {
            currentState = PlayerState.Idle;
            rb.velocity = new Vector2(endDashSpeed * dashDir, rb.velocity.y);
        }
        //isDashSlowing = true;
    }

    private void StartWallJump()
    {
        Flip();
        isTouchingWall = false;
        isWallSliding = false;
        lockXDir = GetFacingInt();
        lockXDirTimer = wallJumpApplyTime;
        rb.velocity = new Vector2(wallJumpSpeedX * lockXDir, wallJumpSpeedY);
        currentState = PlayerState.Jump;
    }

    private void EndWallJump()
    {
        currentState = PlayerState.Idle;
    }

    public void LaunchPlayer(float xDir, float yDir)
    {
        float xLaunch = xDir;
        float yLaunch = yDir;
        if(currentState == PlayerState.Hang)
        {
            EndHang();
        }
        currentState = PlayerState.Launch;
        float launchSpeed = holdScript.GetLaunchSpeed();
        if (xDir != 0 && yDir != 0)
        {
            xLaunch *= 1 / Mathf.Sqrt(2);
            yLaunch *= 1 / Mathf.Sqrt(2);
        }
        else if (xDir == 0 && yDir == 0)
        {
            xDir = 1;
            xLaunch = 1;
            if (!m_FacingRight)
            {
                xLaunch = -1;
            }
        }
        /*
        if(xDir == 0 && yDir == 1)
        {
            yLaunch *= upLaunchMultiplier;
        }
        else if (xDir != 0 && yDir == 0)
        {
            xLaunch *= horiLaunchMultiplier;
        }
        */

        //Vector2 launchVelo = new Vector3(xLaunch * launchSpeed, yLaunch * launchSpeed);
        //rb.velocity = launchVelo;
        launchVector = new Vector2(xLaunch * launchSpeed, yLaunch * launchSpeed);
        Debug.Log("X: " + xLaunch + " Y: " + yLaunch);
        launchTimer = launchDashTime;
        if (holdScript.GetRefreshDash())
        {
            canDash = true;
        }
    }

    public void Bounce(float bounceSpeed, bool refresh)
    {
        if (refresh)
        {
            canDash = true;
        }
        rb.velocity = new Vector2(rb.velocity.x, bounceSpeed);
        currentState = PlayerState.Jump;
    }

    public void RecieveDamage()
    {
        stunned = true;
        Debug.Log("Stunned");
        stunTimer = maxStunTime;
    }

    public void StartHang(Vector3 tilePos, RingHangTileScript ring)
    {
        Debug.Log("StartHang");
        ringScript = ring;
        if (GetDashState())
        {
            EndDash();
        }
        currentState = PlayerState.Hang;
        rb.velocity = Vector3.zero;
        transform.position = tilePos;
        canDash = true;
    }

    public void EndHang()
    {
        Debug.Log("EndHang");
        if(ringScript != null)
        {
            ringScript.UnhangPlayer(transform.position);
            if (currentState == PlayerState.Hang)
            {
                currentState = PlayerState.Idle;
            }
        }
        else
        {
            Debug.Log("Player Didn't Have ringScript");
        }
    }

    public void ResetPlayer(Transform resetPos)
    {
        transform.position = resetPos.position;
        stunned = false;
        currentState = PlayerState.Idle;
        rb.velocity = Vector3.zero;
        holdScript.ResetHolding();
    }
}
