using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class World_ATK_Circle_Single_Orb_Controller : MonoBehaviour
{
    [Header("Move Away")]
    private float moveElapsedT = 0f;
    public float moveDur = 2f;
    public float moveAcceleration = 2;
    public AnimationCurve curve;

    public IEnumerator moveAwayAOrb()
    {


        float percetageDur;

        Vector2 start = transform.localPosition;
        Vector2 end = transform.localPosition * 2;


        while (moveElapsedT < moveDur)
        {
            percetageDur = moveElapsedT / moveDur;

            transform.localPosition = Vector2.Lerp(start, end, curve.Evaluate(percetageDur));


            moveElapsedT += Time.deltaTime;

            yield return null;

        }


        moveElapsedT = 0f;




    }

    public IEnumerator approachAOrb()
    {


        float percetageDur;

        Vector2 start = transform.localPosition;
        Vector2 end = transform.localPosition / 2;


        while (moveElapsedT < moveDur)
        {
            percetageDur = moveElapsedT / moveDur;

            transform.localPosition = Vector2.Lerp(start, end, curve.Evaluate(percetageDur));


            moveElapsedT += Time.deltaTime;

            yield return null;

        }


        moveElapsedT = 0f;




    }
}
