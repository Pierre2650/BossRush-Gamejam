using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World_MAP_MiniOrb_Controller : MonoBehaviour
{
    private Rigidbody2D myRb;



    public GameObject player;
    private Vector2 mouvDirection;
    private float speed = 6f; // old 5



    [Header("ToDestroy")]
    private float toDestroyTimer = 0f;
    private  float toDestroyDuration = 4f;

    [Header("damage")]
    public float damage;


    private bool goStraight;

    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame


    private void Update()
    {
        toDestroyTimer += Time.deltaTime;
        if (toDestroyTimer > toDestroyDuration) { 

            Destroy(this.gameObject);

        }
    }


    private void FixedUpdate()
    {
        pursue();
    }


    private void pursue()
    {

        //a first impulse, track player until certain distance, then go that direction , after 5 sec destroy.

       

        if (Vector2.Distance(player.transform.position, transform.position) > 1.5f && !goStraight)
        {
            pathFinding(player.transform.position);

            myRb.linearVelocity = mouvDirection * speed;


        }
        else
        {
            myRb.linearVelocity = mouvDirection * speed;
            goStraight = true;
        }


    }


    





    ///change thios also
    private void pathFinding(Vector2 targetPos)
    {
        /*calculate the nearest direction to the Boss,
         * 1. take current Player position
         * 2. add a cos sin  vector from the possible direction to the player position
         * 3. compare the distance from this new vector to the Boss , to the distance from the current "nearest" cos sin  vector to the Boss
         */



        Vector2 posToTest, currentPos;
   

        //Optimized Version 
        //  divide pi cirlcle on 4 
        //  find where section we are
        //  only find the neares position in this section

        float[] startEnd = findPISection(targetPos);


        for (float i = startEnd[0]; i < startEnd[1]; i = i + 0.01f)
        {
            posToTest = new Vector2(transform.position.x + Mathf.Cos(i), transform.position.y + Mathf.Sin(i));
            currentPos = new Vector2(transform.position.x + mouvDirection.x, transform.position.y + mouvDirection.y);


            if (Vector2.Distance(posToTest, targetPos) < Vector2.Distance(currentPos, targetPos))
            {
                mouvDirection = new Vector2(Mathf.Cos(i), Mathf.Sin(i));
            }
        }





    }

    private float[] findPISection(Vector2 targetPos)
    {
        float[] startEnd = new float[2];


        if (targetPos.x > transform.position.x && targetPos.y > transform.position.y)
        {
            // Debug.Log("cross hair on X Positive, Y Positive");

            startEnd[0] = 0;
            startEnd[1] = (Mathf.PI / 2);

        }
        else if (targetPos.x < transform.position.x && targetPos.y > transform.position.y)
        {
            // Debug.Log("cross hair on X Negative, Y Positive");

            startEnd[0] = (Mathf.PI / 2);
            startEnd[1] = (Mathf.PI);
        }
        else if (targetPos.x < transform.position.x && targetPos.y < transform.position.y)
        {
            // Debug.Log("cross hair on X Negative, Y Negative");
            startEnd[0] = (Mathf.PI);
            startEnd[1] = ((3 * Mathf.PI) / 2);
        }
        else if (targetPos.x > transform.position.x && targetPos.y < transform.position.y)
        {
            //Debug.Log("cross hair on X Positive, Y Negative");

            startEnd[0] = ((3 * Mathf.PI) / 2);
            startEnd[1] = (Mathf.PI * 2);

        }
        else
        {
            // Debug.Log("Not in PI cercle?");

            startEnd[0] = 0;
            startEnd[1] = (Mathf.PI * 2);

        }

        return startEnd;

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Health playerHealth = collision.GetComponent<Health>();
        PlayerController playerController = collision.GetComponent<PlayerController>();

        if (collision.gameObject.tag == "Player"){
            playerHealth.takeDamage(damage);
            playerController.isHit();
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "PlayerAtk")
        {
         
            Destroy(this.gameObject);
        }
    }



}
