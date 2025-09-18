using NUnit;
using System.Collections;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class World_ATK : MonoBehaviour
{
    private CameraShake cameraSH;
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
    private GameObject circleOrbsPrefab;
    private World_ATK_Circle_Orbs_Controller circleOrbsController;
    
    private float atkIntervalElased = 4f;
    private float atkInterval = 3f;

    private bool switchMouv = false;
    //Multiple nb of atks by 2
    private int nbATK = 6;

        [Header("Push")]
        private CircleCollider2D myCC;

        [Header("Phase 2 ")]
        private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        private float pushDamage = 5;
        private GameObject phase2Prefab;

    //Attack Phase
    private int phase = 1;

    [Header("MegaOrb")]
    //ATK phase 3
    private GameObject megaOrbPrefab;
    private GameObject phase2SFX;





    // Start is called before the first frame update
    void Start()
    {
        myCC = GetComponent<CircleCollider2D>();
        cameraSH = GetComponent<CameraShake>();

        startPos = transform.position;
        mainController = GetComponent<Enemy_Controller>();
        player = mainController.thePlayer;

        circleOrbsPrefab = (GameObject)Resources.Load("Word_ATK_CircleOrbs", typeof(GameObject));
        megaOrbPrefab = (GameObject)Resources.Load("World_ATK_MegaOrb", typeof(GameObject));
        phase2Prefab = (GameObject)Resources.Load("World_ATK_Ph2_FX", typeof(GameObject));

        spawnPrefab();
        StartCoroutine(waitToJump());
    }


    private void spawnPrefab()
    {
        Vector2 prefabStartPos = new Vector2 (transform.position.x, transform.position.y + 0.9f);

        GameObject temp = Instantiate(circleOrbsPrefab, prefabStartPos, transform.rotation, transform);
        circleOrbsController = temp.GetComponent<World_ATK_Circle_Orbs_Controller>();
        circleOrbsController.worldConroller = this;
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

            StartCoroutine(waitToPush());


        }
        else
        {
            myCC.enabled = false;
            circleOrbsController.moveAwayAllOrbs(1);
            switchMouv = !switchMouv;
        }

    }

    private IEnumerator waitToPush()
    {
        yield return new WaitForSeconds(1f);
        myCC.enabled = true;

    }

    private IEnumerator attackPhase2()
    {
        int nbMoveTimes = 2;

        //Accelerate mouv roation and change mouv distance step
        circleOrbsController.orbsMoveDistance = 2.5f;

        StartCoroutine(circleOrbsController.changeRotationSpeed(2.5f));
        phase2SFX = Instantiate(phase2Prefab, this.transform);

        yield return new WaitForSeconds(2f);

        circleOrbsController.moveAwayAllOrbs(nbMoveTimes);

        //Expand Duration

        StartCoroutine(phase2SFX.GetComponent<World_ATK_Phase2_SFX>().changeSize(phase2SFX.transform.localScale,new Vector2(20,20), false));
        pushDamage *= 2;
        StartCoroutine(ChangeCircleColliderRadius(myCC.radius, 16.7f ));

        //Coroutine to expand collider

        StartCoroutine(reset(nbMoveTimes));

    }

    private IEnumerator ChangeCircleColliderRadius(float start, float end)
    {
        float expandElapsedT = 0f, expandDur = 2f;
        float percetageDur;


        while (expandElapsedT < expandDur)
        {


            percetageDur = expandElapsedT / expandDur;

            myCC.radius = Mathf.Lerp(start, end, curve.Evaluate(percetageDur));


            expandElapsedT += Time.deltaTime;

            yield return null;

        }

    }

    private IEnumerator reset(int nbMoveTimes)
    {
        //end attack phase 2
        yield return new WaitForSeconds(4f);
        circleOrbsController.approachAllOrbs(nbMoveTimes);
        StartCoroutine(phase2SFX.GetComponent<World_ATK_Phase2_SFX>().changeSize(phase2SFX.transform.localScale, new Vector2(4, 4),true));
        pushDamage/= 2;
        StartCoroutine(ChangeCircleColliderRadius(myCC.radius,2.2f));

        yield return new WaitForSeconds(3f);

        //Reset orbs Speed and orb mouv distance
        circleOrbsController.orbsMoveDistance = 2.2f;
        StartCoroutine(circleOrbsController.changeRotationSpeed(0.9f));

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

        GameObject temp = Instantiate(megaOrbPrefab, spawnPos, transform.rotation,transform.parent);
        World_ATK_MegaOrb tempController = temp.GetComponent<World_ATK_MegaOrb>();
        tempController.cameraSH = cameraSH;
        tempController.player = player;
        tempController.headController = this;

    }


    public void restartStateMachine()
    {
       phase = 1;
       nbATK = 6;
       StartCoroutine( waitToJump());

    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);

       if (collision.tag == "Player"  && myCC.IsTouching(collision.GetComponent<Collider2D>()))
       {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            Health playerHealth = collision.GetComponent<Health>();

            if (!playerHealth.isInvincible)
            {

                playerHealth.takeDamage(pushDamage);
                playerController.isHit();

            }

            Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
           playerController.startKnockBack(knockbackDir.normalized, 0.5f, 25f, 0.2f, 0.2f);
            
        }
    }
}
