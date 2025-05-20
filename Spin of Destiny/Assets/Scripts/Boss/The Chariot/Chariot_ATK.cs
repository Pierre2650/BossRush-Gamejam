using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Chariot_ATK: MonoBehaviour
{
    [Header("To Init")]
    private Rigidbody2D myRb;
    private BoxCollider2D myBx;
    private Animator myAni;

    //direction
    private Vector2 targetPos;
    private Vector2 safeZone;
    private Enemy_Controller mainController;

    [Header("Aim Prefab")]
    private GameObject prefabRedLine;
    private ChariotAim aimLine;


    [Header("ChargeAttack")]
    public float chargeRange;
    public float chargeSpeed = 40f;
    public float chargeKnockBack;
    public float knockbackMaxAngle;
    public int damage;
    private Personal_Direction_Finder dirFinder;
    private Vector2 chargeDirection;
    private bool isCharging = false;
    public int nCharges = 3;
    private bool startStop = false; //start to stop the charge
    private float stopElapsedT;
    public float chargeStopDuration = 0f;
    private Vector2 positionBeforeCharge;
    
    [Header("Vulnerability")]
    private bool isVulnerable;
    private bool isWaiting = false;
    private float vulnerableElapsedT;
    public float vulnerableDuration = 1.5f; //temps d'attente avant de repartir au safe spot

    [Header("Invulnerability")]
    private float backToSafetyElapsed = 0;
    public float backToSafetyDuration = 0.5f;
    private AnimationCurve backToSafetyCurve = AnimationCurve.Linear(0f,0f,1f,1f);




    // Start is called before the first frame update

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myBx = GetComponent<BoxCollider2D>();
        myAni = GetComponent<Animator>();

        mainController = GetComponent<Enemy_Controller>();
        generateAim();

        dirFinder = new Personal_Direction_Finder(transform.position, 0.01f);

        safeZone = transform.localPosition;


    }

    private void generateAim()
    {
        prefabRedLine = (GameObject)Resources.Load("Chariot_ATK_Aim", typeof(GameObject));
        GameObject temp = Instantiate(prefabRedLine, this.transform);

        aimLine = temp.GetComponent<ChariotAim>();
        aimLine.player = mainController.thePlayer;
        aimLine.boss = this.gameObject;
        aimLine.enabled = true;

    }



    // Update is called once per frame
    void Update()
    {
        
        if (!aimLine.isAiming && !isCharging && nCharges > 0)
        {
            startCharges();
        }

       

        if (isCharging) {

            myRb.linearVelocity = chargeDirection * chargeSpeed;

            //check is arrived at last position
            if (Vector2.Distance(transform.position, targetPos) <= 1 && !startStop )// max distance reached
            {
                startStop = true;
            }
            

            if (startStop) {// is stoping 
                stopCharge();
            }

        }
        else
        {
            setSpriteDir();
        }

        if (isWaiting)
        {
            waitToSafety();

        }


       
    }

    private void setSpriteDir()
    {
        float angleY = 0f;

        if (transform.position.x < mainController.thePlayer.transform.position.x)
        {

            angleY = 0f;
        }

        if (transform.position.x > mainController.thePlayer.transform.position.x)
        {


            angleY = 180f;
        }


        transform.rotation = Quaternion.Euler(0, angleY, 0);

    }

    private void startCharges()
    {
        targetPos = aimLine.lastPosition;

        


        dirFinder.selfRef = transform.position;
        dirFinder.target = targetPos;

        //chargeDirection = dirFinder.findDirToTarget();
        
        chargeDirection = (mainController.thePlayer.transform.position - transform.position).normalized;
       

        positionBeforeCharge = transform.position;

        isCharging = true;
        myAni.SetBool("Charging", isCharging);
        nCharges--;
    }


    private void stopCharge()
    {
        stopElapsedT += Time.deltaTime;

        if (stopElapsedT >= chargeStopDuration)
        {
            myRb.linearVelocity = Vector2.zero;

            if (nCharges == 0)
            {
                isWaiting = true;
                aimLine.setVisibleLine(false);

            }
            else
            {
                aimLine.isAiming = true;
                aimLine.setVisibleLine(true);
            }

            isCharging = false;
            myAni.SetBool("Charging", isCharging);
            startStop = false;

            stopElapsedT = 0f;

        }

    }


    private void waitToSafety()
    {
        vulnerableElapsedT += Time.deltaTime;

        if (vulnerableElapsedT > vulnerableDuration)
        {

            isWaiting = false;
            StartCoroutine(backToSafety());
            vulnerableElapsedT = 0;
        }
    }

    private IEnumerator backToSafety()
    {
        myBx.enabled = false;
        
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
        aimLine.isAiming = true;
        
        nCharges = 3;

        myBx.enabled = true;
    }

    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x + chargeDirection.x, transform.position.y + chargeDirection.y), 0.25f);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(safeZone, 0.25f);

    }


    private void pushPlayer(Vector3 plPosition, PlayerController plController)
    {
        Vector2 knockbackDir = (plPosition - transform.position).normalized;

        float angle = Vector3.SignedAngle(chargeDirection, knockbackDir, Vector3.forward);

        if (Mathf.Abs(angle) > knockbackMaxAngle)
        {
            angle = angle > 0 ? knockbackMaxAngle : -knockbackMaxAngle;
            print("Angle: " + angle);
            knockbackDir = Quaternion.Euler(0, 0, angle) * chargeDirection;
        }
        StartCoroutine(plController.knockback(knockbackDir.normalized, 0.5f, chargeKnockBack, 0.2f, 0.2f));

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="Player" && isCharging){


            Health playerHealth = other.GetComponent<Health>();
            PlayerController playerController = other.GetComponent<PlayerController>();

            if(!playerHealth.isInvincible){

                playerHealth.takeDamage(damage);
                playerController.isHit();

                pushPlayer(playerHealth.transform.position, playerController);

            }
            

        }
    }


}
