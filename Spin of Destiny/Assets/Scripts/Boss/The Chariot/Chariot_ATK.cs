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

    private Personal_Direction_Finder dirFinder;
    private Vector2 chargeDirection;
    private bool isCharging = false;
    private int nCharges = 3;


    [Header("ChargeAttack")]
    private bool startStop = false;
    private float stopElapsedT;
    private float chargeStopDuration = 0.1f;
    
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

        dirFinder = new Personal_Direction_Finder(transform.position, 0.01f);

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

            dirFinder.selfRef = transform.position;
            dirFinder.target = targetPos;

            chargeDirection = dirFinder.findDirToTarget();
        }


       

        if (isCharging) {


            //check is arrived at last position
            if (Vector2.Distance(transform.position, aimLine.lastPosition) <= 0.55 && !startStop)
            {
                startStop = true;

            }

            if (startStop) {
                stopCharge();
            }

            myRb.linearVelocity = chargeDirection * chargeSpeed;
        }
        else
        {
            myRb.linearVelocity = Vector2.zero;

        }

        if (isWaiting)
        {
            wait();

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

    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x + chargeDirection.x, transform.position.y + chargeDirection.y), 0.25f);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(safeZone, 0.25f);

    }


}
