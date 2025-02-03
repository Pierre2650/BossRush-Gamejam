using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Chariot_ATK: Tarot_Controllers
{
    [Header("To Init")]
    private Rigidbody2D myRb;
    private Vector2 targetPos;
    private Vector2 safeZone;
    private Enemy_Controller mainController;

    [Header("Aim Prefab")]
    private GameObject prefabRedLine;
    private ChariotAim aimLine;


    [Header("ChargeAttack")]
    private float chargeSpeed = 40f;
    private Vector2 chargeDirection;
    private bool startStop = false;
    private float stopElapsedT;
    private float chargeStopDuration = 0.1f;
    private bool isCharging = false;

    private int  nCharges = 3;

    [Header("Vulnerability")]
    private bool isVulnerable;
    private bool isWaiting = false;
    private float vulnerableElapsedT;
    private float vulnerableDuration = 1.5f;


    [Header("Invulnerability")]
    private float backToSafetyElapsed = 0;
    private float backToSafetyDuration = 0.5f;
    private AnimationCurve backToSafetyCurve = AnimationCurve.Linear(0f,0f,1f,1f);

 

    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        mainController = GetComponent<Enemy_Controller>();
        generateAim();

        safeZone = transform.localPosition;


    }

    private void generateAim()
    {
        prefabRedLine = (GameObject)Resources.Load("BossAim", typeof(GameObject));
        GameObject temp = Instantiate(prefabRedLine, this.transform);

        aimLine = temp.GetComponent<ChariotAim>();
        aimLine.player = mainController.thePlayer;
        aimLine.boss = this.gameObject;
        aimLine.enabled = true;

    }



    // Update is called once per frame
    void Update()
    {
        if (aimLine.stopAim && !isCharging && nCharges > 0)
        {
            targetPos = aimLine.lastPosition;

            nCharges--;

            isCharging = true;
            findChargeDirection();
        }


        if (isCharging) {


            //check is arrived at last position
            if (Vector2.Distance(transform.position, aimLine.lastPosition) < 0.5 && !startStop)
            {
                startStop = true;

            }

            if (startStop) {
                stopCharge();
            }
        }

        if (isWaiting) {
            wait();
        
        }


       
    }


    private void FixedUpdate()
    {
        if (isCharging)
        {
            myRb.velocity = chargeDirection * chargeSpeed;
        }
        else
        {
            myRb.velocity = Vector2.zero;
        }
    }

    private void startAim()
    {
        //instantiate
    }


    private void stopCharge()
    {
        stopElapsedT += Time.deltaTime;

        if (stopElapsedT > chargeStopDuration)
        {

            if (nCharges == 0)
            {
                isWaiting = true;
                aimLine.setVisibleLine(false);

            }
            else
            {
                aimLine.stopAim = false;
            }

            isCharging = false;

            stopElapsedT = 0f;
            startStop = false;


        }

    }


    private void wait()
    {
        vulnerableElapsedT += Time.deltaTime;

        if (vulnerableElapsedT > vulnerableDuration)
        {

            isWaiting = false;
            nCharges--;
            StartCoroutine(backToSafety());
            vulnerableElapsedT = 0;
        }
    }

    private IEnumerator backToSafety()
    {
        
        Vector2 start = transform.localPosition;
        Vector2 end = safeZone;

        float percentageDur = 0f;

        while (backToSafetyElapsed < backToSafetyDuration)
        {

            percentageDur = backToSafetyElapsed / backToSafetyDuration;

            transform.localPosition = Vector2.Lerp(start, end, backToSafetyCurve.Evaluate(percentageDur));

            backToSafetyElapsed += Time.deltaTime;
            yield return null;

        }
        backToSafetyElapsed = 0;
        aimLine.stopAim = false;
        
        nCharges = 3;

    }

    private void findChargeDirection()
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

        float[] startEnd = findPISection();


        for (float i = startEnd[0]; i < startEnd[1]; i = i + 0.01f)
        {
            posToTest = new Vector2(transform.position.x + Mathf.Cos(i), transform.position.y + Mathf.Sin(i));
            currentNpos = new Vector2(transform.position.x + chargeDirection.x, transform.position.y + chargeDirection.y);


            if (Vector2.Distance(posToTest, targetPos) < Vector2.Distance(currentNpos, targetPos))
            {
                chargeDirection = new Vector2(Mathf.Cos(i), Mathf.Sin(i));
            }
        }





    }

    private float[] findPISection()
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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x + chargeDirection.x, transform.position.y + chargeDirection.y), 0.25f);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(safeZone, 0.25f);

    }


}
