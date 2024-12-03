using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovementScript : MonoBehaviour
{


    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    private TrailRenderer trailRenderer;
    private CameraShake cameraShake;

    [Header("Shooting")]
    [SerializeField] GameObject Arrow;
    public float shootCooldown = 0.5f;

    private float lastShotTime;
    [SerializeField] Transform ArrowSpawn;
    int direction;

    [Header("Collision")]
    float gravityScaleAtStart;
    bool isGrounded;
    Vector2 vecGravity;
    public LayerMask groundLayer;
    public LayerMask Wall;
    private float gravityStore;
    float originalGravity;


    [Header("Running")]
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float acceleration = 18f; // how fast the player speeds up
    [SerializeField] float deceleration = 18f; // how fast the player slows down

    [Header("Jumping")]
    [SerializeField] float jumpSpeed = 40f;
    bool isJumping;

    private bool doubleJump;
    private float jumpCutMultiplier = 7.5f;
    private float jumpTime = 2f; // set the maximum jump time to 2 seconds
    private float jumpTimeCounter = 0f; // initialize the jump time counter to zero
    private int maxJumpTime = 60; // allow the player


    [Header("Climbing")]
    [SerializeField] float climbSpeed = 3f;



    [Header("Death")]
    [SerializeField] Vector2 DeathKick = new Vector2(10f, 10f);
    bool isAlive = true;

    [Header("Audio")]
    [SerializeField] private AudioClip ShootingSFX;
    [SerializeField] private AudioClip JumpingSFX;

    [Header("Dash")]
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashDistance = 10f;
    [SerializeField] private float dashingVelocity = 25f;
    [SerializeField] private float dashingTime = 101f;
    private Vector2 dashingDirection;
    private bool isDashing;
    private bool canDash = true;
    private float dashTimer;
    private Vector2 dashStartPosition;






    void Start()
    {
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
        gravityStore = myRigidbody.gravityScale;
        cameraShake = Camera.main.GetComponent<CameraShake>();
        trailRenderer = GetComponent<TrailRenderer>();
      


    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            // calculate the velocity based on the desired direction and speed
            Vector2 velocity = dashingDirection.normalized * dashingVelocity;

            // set the player's velocity to the new velocity
            myRigidbody.velocity = velocity;

            // end the dash if the player has moved the desired distance
            if (Vector2.Distance(transform.position, dashStartPosition) >= dashDistance)
            {
                isDashing = false;
                myRigidbody.velocity = Vector2.zero;
            }
        }
    }

    void Update()
    {
        if (!isAlive)
        {
            return;
        }
        if (Input.GetButtonDown("Dash") && canDash && !isGrounded && dashTimer <= 0f)
        {
           cameraShake.Shake();
            isDashing = true;
            canDash = false;

            // record the starting position of the dash
            dashStartPosition = transform.position;

            // calculate the direction of the dash
            dashingDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (dashingDirection == Vector2.zero)
            {
                dashingDirection = new Vector2(transform.localScale.x, 0);
            }

            // set the duration of the dash
            dashTimer = dashDuration;
        }

        // decrement the dash timer
        if (dashTimer > 0f)
        {
            dashTimer -= Time.deltaTime;
        }

        // end the dash if the timer has expired
        if (dashTimer <= 0f && isDashing)
        {
            isDashing = false;
            myRigidbody.velocity = Vector2.zero;
            canDash = true; // reset canDash
        }

        myAnimator.SetBool("isDashing", isDashing);
    



    HandleShooting();
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
        myAnimator.SetBool("isJumping", isJumping && !isGrounded);
        myAnimator.SetFloat("yVelocity", myRigidbody.velocity.y);
        

        if (Input.GetButtonUp("Fire1"))
        {
            myAnimator.SetBool("isShooting", false);
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }

        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground" , "Bridge")))
        {
            isGrounded = true;
            canDash = true;



        }
        else
        {
            isGrounded = false;

        }

       

    }






  
        void HandleShooting()
    {
        if (!isAlive && !StartCutScene.isCutSceneOn)
        {
            return;
        }
        if (Input.GetButtonDown("Fire1") && isGrounded) {
            SoundManager.instance.PlaySound(ShootingSFX);
           GameObject newArrow = Instantiate(Arrow, ArrowSpawn.position, transform.rotation);
            newArrow.transform.localScale = new Vector2(newArrow.transform.localScale.x * direction, newArrow.transform.localScale.y);
            myAnimator.SetBool("isShooting",true);
           
        }    
        
    }




    void Run()
    {
        float targetSpeed = moveInput.x * runSpeed; // calculate the target speed based on input
        float currentSpeed = myRigidbody.velocity.x;

        // accelerate towards target speed
        if (targetSpeed > currentSpeed)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * acceleration);
        }
        // decelerate towards target speed
        else if (targetSpeed < currentSpeed)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * deceleration);
        }

        myRigidbody.velocity = new Vector2(currentSpeed, myRigidbody.velocity.y);

        bool playerHasHorizantalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon && Mathf.Abs(moveInput.x) > Mathf.Epsilon;
        myAnimator.SetBool("IsRunning", playerHasHorizantalSpeed && isGrounded);

        Debug.Log(playerHasHorizantalSpeed);
       
    }




    // void Run()
    // {

    //    Vector2 PlayerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
    //   myRigidbody.velocity = PlayerVelocity;
    //
    //   bool playerHasHorizantalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
    //   myAnimator.SetBool("IsRunning", playerHasHorizantalSpeed && isGrounded);


    //  }

    void OnMove(InputValue value)
    {
        if (!isAlive && !StartCutScene.isCutSceneOn)
        {
            return; 
        }
        
         moveInput = value.Get<Vector2>();
        if (myRigidbody.velocity.y < Mathf.Epsilon)
        {
            
            isJumping = false;

        }

    }









    void OnJump()
    {
        if (!isAlive && !StartCutScene.isCutSceneOn)
        {
            return;
        }

        if (isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                SoundManager.instance.PlaySound(JumpingSFX);

                // Add the horizontal velocity to the jump velocity
                Vector2 jumpVelocity = new Vector2(myRigidbody.velocity.x, jumpSpeed);

                myRigidbody.velocity = jumpVelocity;

                isGrounded = false;
                isJumping = true;
                jumpTimeCounter = 0;
            }
        }
        else if (isJumping)
        {
            // Add the horizontal velocity to the jump velocity
            Vector2 jumpVelocity = new Vector2(myRigidbody.velocity.x, jumpSpeed);

            myRigidbody.velocity = jumpVelocity;

            // Limit the jump time
            if (Input.GetButton("Jump") && jumpTimeCounter < maxJumpTime)
            {
                jumpTimeCounter += Time.deltaTime;
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpSpeed);
            }
            else
            {
                isJumping = false;
            }
        }
    }









    void FlipSprite()
    {
        
        bool playerHasHorizantalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizantalSpeed)
        {
           

            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);

        }
    }








    void ClimbLadder()
    {
        
        bool istouchingLadder = (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")));
       if(istouchingLadder ==true)
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("IsClimbing", false);
            return;
        }
     


       
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        if (playerHasVerticalSpeed == true)
        {
            myAnimator.SetBool("IsClimbing", true);
        }
        else
        {
            myAnimator.SetBool("IsClimbing", false);

        }
        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
      
        
        if (!isJumping)
        {
            myRigidbody.velocity = climbVelocity;
            myRigidbody.gravityScale = 0f;
        }
        else
        {

            myAnimator.SetBool("IsClimbing", false);
            return;
        }
    }

    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = DeathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
   
}
