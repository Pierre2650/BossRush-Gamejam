using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil : MonoBehaviour
{

    [Header("Berzier Mouvement")]
    public float speed = 1.0f;
    private float t = 0f;
    private Vector3 P0;
    public Vector3 P1;
    public Vector3 P2;
    private bool test = false;
    private float maxD = 5.2f;


    [Header("The Player")]
    public GameObject player;

    [Header("AttackZone")]
    private float pathFindInterval = 0f;
    public Vector2 playerDir = Vector2.zero;
    public GameObject mesh;
    private MeshTest meshController;

    // Start is called before the first frame update
    void Start()
    {
        P0 = transform.position;
        player = GameObject.Find("Player");
        //P2 = player.transform.position;
        meshController = mesh.GetComponent<MeshTest>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) > maxD)
        {
            P2 = player.transform.position;
        }
        

        if (Input.GetKey(KeyCode.X))
        {
            test = true;
            P1 = new Vector3(P2.x, 11f);
        }

        if (test)
        {
            jump();
            
        }

        pathFindInterval += Time.deltaTime;
        if (pathFindInterval > 0.2f)
        {
            //pathFind(player.transform.position);
            pathFindInterval = 0f;
        }


    }


    private void jump()
    {
        //quadratic Berzier exemple
        if (t < 1f)
        {
            transform.position = P1 + Mathf.Pow((1 - t), 2) * (P0 - P1) + Mathf.Pow(t, 2) * (P2 - P1);

            t = t + speed * Time.deltaTime;

        }
        else
        {
            test = false;
            t = 0f;
            showAttack();
        }

    }

    private void showAttack()
    {
        pathFind(player.transform.position);

        meshController.Dir = playerDir;

        meshController.followPlayer();
        meshController.myMshR.enabled = true;
        meshController.callScythe();

    }




    private void pathFind(Vector2 targetPos)
    {
        /*calculate the nearest direction to the Boss,
         * 1. take current Player position
         * 2. add a cos sin  vector from the possible direction to the player position
         * 3. compare the distance from this new vector to the Boss , to the distance from the current "nearest" cos sin  vector to the Boss
         */



        Vector2 posToTest, currentNpos;

        //Optimized Version 
        //  divide pi cirlcle on 4 
        //  find where section we are
        //  only find the neares position in this section

        float[] startEnd = findPISection(targetPos);


        for (float i = startEnd[0]; i < startEnd[1]; i = i + 0.05f)
        {
            posToTest = new Vector2(transform.position.x + Mathf.Cos(i), transform.position.y + Mathf.Sin(i));
            currentNpos = new Vector2(transform.position.x + playerDir.x, transform.position.y + playerDir.y);


            if (Vector2.Distance(posToTest, targetPos) < Vector2.Distance(currentNpos, targetPos))
            {
                playerDir = new Vector2(Mathf.Cos(i), Mathf.Sin(i));
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



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(P1, 0.2f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(P2, 0.2f);

        
         Vector2 temp = new Vector2(transform.position.x + playerDir.x*2, transform.position.y + playerDir.y*2);

         Gizmos.color = Color.black;
         Gizmos.DrawWireSphere(temp, 0.25f);

        
    }
}
