using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_controller : MonoBehaviour
{
    //SelfComponents
    public Rigidbody2D myRb;
    private BoxCollider2D myBxC;
    private Animator myAni;

    [Header("Mouvement Manager")]
    public float xAxis = 0;
    public float yAxis = 0;
    [SerializeField] private float speed = 0;


    [Header("Debuff")]
    private bool mouvConstrained = false;

    [Header("Forbidden Zone Debug")]
    private Vector2 lastMouvDir = Vector2.zero;
    private bool debugZone = false;


    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myAni = GetComponent<Animator>();
        myBxC = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        xAxis = 0;
        yAxis = 0;



        mouvementInput();
        animationController();

        //Reload scene
        if (Input.GetKeyDown(KeyCode.P))
        { SceneManager.LoadScene("SampleScene"); }


        
    }

   


    private void FixedUpdate()
    {
        setlastMouvDir();


        if (!mouvConstrained ) { 
            

            if (!debugZone)
            {
                mouvement();
            }
            else {

                moveFromforbiddenZone();

            }
        }


        insideObstacleDebug();




    }

    private void setlastMouvDir()
    {

        if (myRb.velocity != Vector2.zero)
        {

            lastMouvDir = myRb.velocity;

        }


    }

    private void insideObstacleDebug()
    {
     
        // Box parameters
        Vector2 position = new Vector2(transform.position.x, transform.position.y + 0.2f);  // Center of the box
        Vector2 size = new Vector2(0.3f, 0.45f); // Size of the box (width x height)


        Collider2D overlap = Physics2D.OverlapBox(position, size, 0f , LayerMask.GetMask("Obstacles"));

     
        if (overlap && !debugZone ) {

            
            myBxC.isTrigger = true;
            debugZone = true;
            

        }

        if (!overlap && debugZone ) 
        {
            debugZone = false;
            myBxC.isTrigger = false;

            myRb.velocity = Vector2.zero;

        }
        

    }



    private void animationController()
    {
        myAni.SetFloat("horizontal", xAxis);
        myAni.SetFloat("vertical", yAxis);
        myAni.SetFloat("speed", myRb.velocity.sqrMagnitude);




    }

    private void mouvementInput()
    {
        if (Input.GetKey(KeyCode.D))
        {
            xAxis = 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            xAxis = -1;
        }

        if (Input.GetKey(KeyCode.W))
        {
            yAxis = 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            yAxis = -1;
        }
    }

    private void mouvement()
    {
        if (xAxis == 0 && yAxis == 0 )
        {
            myRb.velocity = Vector2.zero;

        }
        else
        {
            Vector2 temp = new Vector2(xAxis, yAxis);

            temp.Normalize();

            myRb.velocity = temp * speed;
        }


    }

    public void restrainMouvement()
    {
        myRb.velocity = Vector2.zero;
        mouvConstrained = true;

        myBxC.excludeLayers = LayerMask.GetMask("Obstacles");
        //Bx collider filter obstacles not deactivate
    }

    public void freeMouvement()
    {
        myRb.velocity = Vector2.zero;
        mouvConstrained = false;
        myBxC.excludeLayers = LayerMask.GetMask("Nothing");
    }


    private void  moveFromforbiddenZone()
    {
        myRb.velocity = lastMouvDir;

    }


    void OnDrawGizmos()
    {
        // Box parameters
        Vector2 position = new Vector2(transform.position.x, transform.position.y + 0.2f);  // Center of the box
        Vector2 size = new Vector2(0.3f, 0.45f); // Size of the box (width x height)

        // Draw the box using Gizmos
        Gizmos.color = Color.green;  // Color of the Gizmo
        Gizmos.DrawWireCube(position, size);
    }




}
