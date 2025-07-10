using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Obsolete("not used anymore")]
public class Player_controller : MonoBehaviour
{
    //SelfComponents
    public Rigidbody2D myRb;
    private BoxCollider2D myBxC;
    private Animator myAni;
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


        insideObstacleDebug();

        setlastMouvDir();


        if (!mouvConstrained)
        {


            if (!debugZone)
            {
                mouvement();
            }
            else
            {

                moveFromforbiddenZone();

            }
        }


       




    }



    private void setlastMouvDir()
    {

        if (myRb.linearVelocity != Vector2.zero)
        {

            lastMouvDir = myRb.linearVelocity;

        }


    }

    private void insideObstacleDebug()
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

            myRb.linearVelocity = Vector2.zero;

        }
        

    }



    private void animationController()
    {
        myAni.SetFloat("horizontal", xAxis);
        myAni.SetFloat("vertical", yAxis);
        myAni.SetFloat("speed", myRb.linearVelocity.sqrMagnitude);

    }

    private void mouvementInput()
    {
        
        
        if (Input.GetKey(KeyCode.D))
        {
            xAxis = 1;
            transform.right = Vector2.right;
        }

        if (Input.GetKey(KeyCode.A))
        {
            xAxis = -1;
            transform.right = Vector2.left;
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
            myRb.linearVelocity = Vector2.zero;
            
        }
        else
        {
            Vector2 temp = new Vector2(xAxis, yAxis);

            temp.Normalize();

            myRb.linearVelocity = temp * speed;

            
        }
    }

    public void restrainMouvement()
    {
        myRb.linearVelocity = Vector2.zero;
        mouvConstrained = true;

        myBxC.excludeLayers = LayerMask.GetMask("Obstacles");

        //Bx collider filter obstacles not deactivate
    }

    public void freeMouvement()
    {
        myRb.linearVelocity = Vector2.zero;
        mouvConstrained = false;
        myBxC.excludeLayers = LayerMask.GetMask("Nothing");
    }


    private void  moveFromforbiddenZone()
    {
        Debug.Log("Moving form forbidden zone");
        myRb.linearVelocity = lastMouvDir;

    }


    void OnDrawGizmos()
    {
        // Box parameters
        // Box parameters
        Vector2 position = new Vector2(transform.position.x, transform.position.y + 0.2f);  // Center of the box
        float size = 0.1f; // Size of the box (width x height)

        // Draw the box using Gizmos
        Gizmos.color = Color.green;  // Color of the Gizmo
        Gizmos.DrawWireSphere(position, size);
    }





}
