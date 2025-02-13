using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class World_ATK : MonoBehaviour
{

    private Vector2 startPos;

    [Header("Berzier Mouvement")]
    private float speed = 1.0f;
    private float t = 0f;
    private Vector3 P0;
    private Vector3 P1;
    private Vector3 P2;
    private float maxD = 7f;


    [Header("Jump")]
    private bool goJump = false;
    private int nJumps = 1;


    [Header("The Player")]
    public GameObject player;

    [Header("Boss Controller")]
    private Enemy_Controller mainController;


    [Header("ATK")]
    public bool isAttacking = false;
    public GameObject circleOrbs;
    private World_ATK_Circle_Orbs_Controller circleOrbsController;
    

    private float atkIntervalElased = 4f;
    private float atkInterval = 3f;

    private bool switchMouv = false;
    private int nbATK = 2;

    //Attack Phase
    private int phase = 1;

    [Header("MegaOrb")]
    //ATK phase 3
    private GameObject MegaOrbPrefab;



    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        mainController = GetComponent<Enemy_Controller>();
        player = mainController.thePlayer;

        MegaOrbPrefab = (GameObject)Resources.Load("World_ATK_MegaOrb", typeof(GameObject));

        spawnPrefab();

        StartCoroutine(waitToJump());
    }


    private void spawnPrefab()
    {
        circleOrbsController = circleOrbs.GetComponent<World_ATK_Circle_Orbs_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) > maxD && phase == 1)
        {
            P2 = player.transform.position;
        }


        if (goJump)
        {
            jump();

        }

        if (isAttacking)
        {
            
             atkIntervalElased += Time.deltaTime;
            

            if (atkIntervalElased > atkInterval)
            {


                if (nbATK == 0)
                {
                    isAttacking = false;
                    StartCoroutine(attackPhase2());
                    //finish attack
                }
                else {

                    attackPhase1();

                    atkIntervalElased = 0f;
                    nbATK--;

                }


            }

        }



    }

    private void attackPhase1()
    {

        if (switchMouv)
        {
            circleOrbsController.approachAllOrbs(1);
            switchMouv = !switchMouv;


        }

        else
        {
            circleOrbsController.moveAwayAllOrbs(1);
            switchMouv = !switchMouv;
        }

    }

    private IEnumerator attackPhase2()
    {
        int nbMoveTimes = 2;

        circleOrbsController.orbsMoveDistance = 2.5f;
        circleOrbsController.rotationAcceleration = 2.5f;

        yield return new WaitForSeconds(2f);

        circleOrbsController.moveAwayAllOrbs(nbMoveTimes);
        StartCoroutine(reset(nbMoveTimes));

    }

    private IEnumerator reset(int nbMoveTimes)
    {
        //end attack phase 2
        yield return new WaitForSeconds(5f);
        circleOrbsController.approachAllOrbs(nbMoveTimes);

        yield return new WaitForSeconds(3f);
        circleOrbsController.orbsMoveDistance = 2f;
        circleOrbsController.rotationAcceleration = 1f;

        yield return new WaitForSeconds(1f);

        //go to spawn start phase 3
        phase = 3;

        StartCoroutine(waitToJump());


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

            if (phase == 1)
            {
                nJumps--;
                StartCoroutine(circleOrbsController.showOrbs());
            }

            if (phase == 3) {
                circleOrbsController.allOrbsToCenter();
                StartCoroutine(waitToMegaOrb());
                phase = 0;
            }
           
        }

    }

   
    private IEnumerator waitToJump()
    {
        yield return new WaitForSeconds(0.5f);

        P0 = transform.position;
    
        if (phase == 3)
        {
            P2 = startPos;

        }

        P1 = new Vector3(P2.x, 12f);

        goJump = true;
    }

    private IEnumerator waitToMegaOrb()
    {
        yield return new WaitForSeconds(1.1f);

        Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y + 1f);

        GameObject temp = Instantiate(MegaOrbPrefab, spawnPos, transform.rotation,transform.parent);
        World_ATK_MegaOrb tempController = temp.GetComponent<World_ATK_MegaOrb>();
        tempController.player = player;
        tempController.headCOntroller = this;

    }


    public void restartStateMachine()
    {

    }
}
