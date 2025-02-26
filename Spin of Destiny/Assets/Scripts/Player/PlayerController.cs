
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public PlayerInputActions playerInputActions;
    [Header("Movement")]
    private Rigidbody2D myRb;
    private Animator myAni;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;
    public float speed = 0;
    float targetSpeed;
    public float accelerationValue;
    public float decelerationValue;
    private float accelRate;

    [Header("Debuff")]
    private bool mouvConstrained = false;

    //private Vector2 lastMouvDir = Vector2.zero;
    //private bool debugZone = false;

    void Awake(){
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myAni = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = playerInputActions.Player.Move.ReadValue<Vector2>();
        targetSpeed = speed * moveInput.magnitude;
        if(moveInput.magnitude==0 || myRb.linearVelocity.magnitude>speed){
            myAni.SetBool("isMoving", false);
            accelRate = decelerationValue; 
        }
        else{
            myAni.SetBool("isMoving", true);
            accelRate = accelerationValue;
            spriteRenderer.flipX = moveInput.x>0? false : moveInput.x<0 ?true :spriteRenderer.flipX;
        }
    }

    void FixedUpdate()
    {     
        applyForceToSpeed(targetSpeed, moveInput, myRb, accelRate);
        
    }

    public IEnumerator knockback(Vector2 dir, float slowFactor, float force, float slowTime, float newAcceleration){
        myRb.linearVelocity *= slowFactor;
        myRb.AddForce(dir*force, ForceMode2D.Impulse);
        float tmp = accelerationValue;
        accelerationValue = newAcceleration;
        yield return new WaitForSeconds(slowTime);
        accelerationValue = tmp;
        print("end");
    }



    public static void applyForceToSpeed(float maxSpeed, Vector2 target, Rigidbody2D rb2d, float accelerationRate){
        Vector2 speedDiff = new Vector2(maxSpeed*target.x,maxSpeed*target.y)-rb2d.linearVelocity;
        rb2d.AddForce(speedDiff * accelerationRate,ForceMode2D.Force);
    }

    /*private void insideObstacleDebug()
    {
     
        // Box parameters
        Vector2 position = new Vector2(transform.position.x, transform.position.y + 0.2f);  // Center of the box
        float size = 0.1f; // Size of the box (width x height)


        Collider2D overlap = Physics2D.OverlapCircle(position, size, LayerMask.GetMask("Obstacles"));
        
     
        if (overlap && !debugZone ) {
            Debug.Log(overlap);

           myBxC.isTrigger = true;
           debugZone = true;
            

        }else if (!overlap && debugZone ) 
        {
            debugZone = false;
            myBxC.isTrigger = false;

            myRb.velocity = Vector2.zero;

        }
    }*/
}
