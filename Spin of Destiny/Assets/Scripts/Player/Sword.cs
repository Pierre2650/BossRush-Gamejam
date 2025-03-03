using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Sword : MonoBehaviour
{

    [Header("Animation")]
    private float swingElapsedT = 0f;
    private float swingDuration = 0.1f;
    private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    private Coroutine attackMouv = null;


    [Header("Controller")]
    private Special_Weapon_Controller controller;

    [Header("player")]
    private GameObject player;
    private Player_controller playerController;

    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<Player_controller>();
        controller = GetComponent<Special_Weapon_Controller>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            attackMouv = StartCoroutine(attack());
        }

        if (playerController.myRb.linearVelocity != Vector2.zero && attackMouv == null)
        {
            setDirection();
        }


    }

    private void setDirection()
    {
        if (playerController.xAxis > 0)
        {
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        }

        if (playerController.xAxis < 0)
        {
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (playerController.yAxis > 0 && transform.eulerAngles.x == 0 )
        {
          
           // transform.eulerAngles = new Vector3(180f, transform.eulerAngles.y, transform.eulerAngles.z);

        }

        if (playerController.yAxis < 0 && transform.eulerAngles.x != 0 )
        {
            //transform.eulerAngles = new Vector3(180f, transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }

    private IEnumerator attack()
    {

        float percentageDur = 0;
 
        Vector3 start = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        Vector3 end = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 250f);

       

        while (swingElapsedT < swingDuration)
        {

            percentageDur = swingElapsedT / swingDuration;

            transform.eulerAngles = Vector3.Lerp(start, end, curve.Evaluate(percentageDur));

            swingElapsedT += Time.deltaTime;
            yield return null;

        }
        
        swingElapsedT = 0;
        attackMouv = null;

        controller.resetWeapon();
        Destroy(this);

    }

}
