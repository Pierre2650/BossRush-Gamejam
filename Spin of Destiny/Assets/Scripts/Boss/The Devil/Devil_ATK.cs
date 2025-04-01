using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil_ATK : MonoBehaviour
{
    private Vector2 startPos;

    [Header("Berzier Mouvement")]
    private float speed = 1.0f;
    private float t = 0f;
    private Vector3 P0;
    private Vector3 P1;
    private Vector3 P2; // jump end position
    public float maxD = 3.2f;


    [Header("Jump")]
    private bool goJump = false;
    private int nJumps = 3 ;



    [Header("The Player")]
    public GameObject player;


    [Header("Scythe Cosmetic")]
    private GameObject fakeScythePrefab;
    private GameObject fakeScythe;

    [Header("AttackZone")]
    private GameObject meshPrefab;
    private GameObject mesh;

    private Vector2 playerDir = Vector2.zero;
    private MeshDevilAtkZone meshController;
    private bool isAttacking = true;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        fakeScythePrefab = (GameObject)Resources.Load("DevilScythe_Cosmetic", typeof(GameObject));
        meshPrefab = (GameObject)Resources.Load("AttackZone", typeof(GameObject));

        spawnPrefabs();

       
        startPos = transform.position;

        stateMachine();


    }

    private void spawnPrefabs()
    {
       fakeScythe = Instantiate(fakeScythePrefab, transform);
       mesh = Instantiate(meshPrefab, transform);

       meshController = mesh.GetComponent<MeshDevilAtkZone>();
       meshController.controller = this;

    }

    // Update is called once per frame
    void Update()
    {
        setFakeScytheDir();

        if (Vector3.Distance(this.transform.position, player.transform.position) > maxD && isAttacking)
        {
            P2 = player.transform.position + new Vector3(Random.Range(0.1f, 0.4f), Random.Range(0.1f,0.4f));
        }
       

        if (goJump)
        {
            jump();
            
        }



    }


    private void setFakeScytheDir()
    {
        float angleY = 0f;

        if (transform.position.x < player.transform.position.x)
        {

            angleY = 0f;
        }

        if (transform.position.x > player.transform.position.x)
        {

          
            angleY = 180f;
        }


        transform.rotation = Quaternion.Euler(0, angleY, 0);

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
            goJump = false;
            t = 0f;

            if (isAttacking) { 
                nJumps--;
                showAttack();
            }
            else
            {
                StartCoroutine(waitOnSpawn());

            }
        }

    }

    private void showAttack()
    {
        fakeScythe.SetActive(false);

        pathFind(player.transform.position);
        meshController.Dir = playerDir;
        meshController.aimAttackZoneAtPlayer();
        meshController.myMshR.enabled = true;

        meshController.showScythe();

    }


    public void stateMachine()
    {
        fakeScythe.SetActive(true);

        if(nJumps  <= 0)
        {
            isAttacking = false;
            
        }

        StartCoroutine(waitToJump());
    }
    private IEnumerator waitToJump()
    {
        yield return new WaitForSeconds(0.5f);

        P0 = transform.position;

        P1 = new Vector3(P2.x, 11f);

        if (!isAttacking)
        {
            P2 = startPos;
            P1 = new Vector3(P0.x, 11f);
        }

        

        goJump = true;
    }

    private IEnumerator waitOnSpawn()
    {
        yield return new WaitForSeconds(2f);
        isAttacking = true;
        nJumps = 3;
        stateMachine();
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
