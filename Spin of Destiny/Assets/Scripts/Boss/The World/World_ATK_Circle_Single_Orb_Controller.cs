using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class World_ATK_Circle_Single_Orb_Controller : MonoBehaviour
{
    [Header("Move")]
    private float moveDistance = 2;
    private float moveElapsedT = 0f;
    private float moveDur = 1f;
    private float moveAcceleration = 1;
    public AnimationCurve curve;

    [Header("Father Controller")]
    public World_ATK_Circle_Orbs_Controller dadController;

    private Vector2 startPos;

    public float damage;



    private void Start()
    {
        startPos = transform.localPosition;
    }
    private void Update()
    {
        moveDistance = dadController.orbsMoveDistance;
        moveAcceleration = dadController.orbsMoveSpeed;


    }


    public IEnumerator moveAwayAOrb(int nbTimes)
    {

        float percetageDur;

        Vector2 start = transform.localPosition;
        Vector2 end = transform.localPosition * moveDistance;


        while (moveElapsedT < moveDur/moveAcceleration)
        {
            percetageDur = moveElapsedT / (moveDur / moveAcceleration);

            transform.localPosition = Vector2.Lerp(start, end, curve.Evaluate(percetageDur));


            moveElapsedT += Time.deltaTime;

            yield return null;

        }


        moveElapsedT = 0f;
        nbTimes--;

        if (nbTimes > 0) { 
            StartCoroutine(moveAwayAOrb(nbTimes));
        }




    }

    public IEnumerator approachAOrb(int nbTimes)
    {


        float percetageDur;

        Vector2 start = transform.localPosition;
        Vector2 end = transform.localPosition / moveDistance;


        while (moveElapsedT < moveDur/ moveAcceleration)
        {
            percetageDur = moveElapsedT / (moveDur / moveAcceleration);

            transform.localPosition = Vector2.Lerp(start, end, curve.Evaluate(percetageDur));


            moveElapsedT += Time.deltaTime;

            yield return null;

        }


        moveElapsedT = 0f;


        nbTimes--;

        if (nbTimes > 0)
        {
            StartCoroutine(approachAOrb(nbTimes));
        }



    }

    public IEnumerator goToCenter()
    {


        float percetageDur;

        Vector2 start = transform.localPosition;
        Vector2 end = Vector2.zero;


        while (moveElapsedT < moveDur / moveAcceleration)
        {
            percetageDur = moveElapsedT / (moveDur / moveAcceleration);

            transform.localPosition = Vector2.Lerp(start, end, curve.Evaluate(percetageDur));


            moveElapsedT += Time.deltaTime;

            yield return null;

        }

        moveElapsedT = 0f;






    }

    private void OnDisable()
    {
        transform.localPosition = startPos;

    }

    void OnTriggerEnter2D(Collider2D other) {
     
        if(other.tag=="Player"){
            Health playerHealth = other.GetComponent<Health>();
            PlayerController playerController = other.GetComponent<PlayerController>();

            playerHealth.takeDamage(damage);
            playerController.isHit();
        }
    }
}
