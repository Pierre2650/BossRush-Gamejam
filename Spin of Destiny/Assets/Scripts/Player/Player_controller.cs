using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour
{
    //SelfComponents
    public Rigidbody2D myRb;
    private Animator myAni;
    public float xAxis = 0;
    public float yAxis = 0;
    private char lastDir = 's';
    [SerializeField] private float speed = 0;




    void Awake(){

    }

    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myAni = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        xAxis = 0;
        yAxis = 0;


        mouvementInput();
        animationController();


    }




    private void FixedUpdate()
    {
        mouvement();
    
    
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
        if (xAxis == 0 && yAxis == 0)
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






}
