using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour
{
    //SelfComponents
    private Rigidbody2D myRb;
    private Animator myAni;

    [Header("Mouvement Manager")]
    private float xAxis = 0;
    private float yAxis = 0;
    [SerializeField] private float speed = 0;


    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myAni = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        xAxis = yAxis = 0;


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
        if (xAxis == 0 && yAxis == 0)
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





}
