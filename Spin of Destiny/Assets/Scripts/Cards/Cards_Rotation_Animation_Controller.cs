using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Cards_Rotation_Animation_Controller : MonoBehaviour
{

    public Vector2 cardAdditionPos;
    public List<GameObject> cards = new List<GameObject>();
    public  float radius = 1;
    public int n = 22;

    [Header("Rotation")]
    public float rotationDur;
    private float rotationElapsedT = 0;
    public float rotationAcceleration = 1;

    private Coroutine infiniteRotation;


    public AnimationCurve curve;

    private PickableObj_Mouvement myPickObjMouv;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        generateCardsPosition();
        myPickObjMouv = GetComponent<PickableObj_Mouvement>();

    }


    private void OnEnable()
    {
        infiniteRotation = StartCoroutine(rotate());
    }

    private void OnDisable()
    {
        rotationAcceleration = 1f;
        myPickObjMouv.enabled = false;
        StopAllCoroutines();
    }

    
    public void stopRoation()
    {
        // rotationAcceleration = 0.5f;
        StartCoroutine(deAcceleration());
    }

    private IEnumerator deAcceleration()
    {
        float percetageDur , elapsed = 0;

        float start = 1f;
        float end = 0.5f;


        while (elapsed < 1f)
        {

            percetageDur = elapsed / 1f ;

            rotationAcceleration = Mathf.Lerp(start, end, curve.Evaluate(percetageDur));


            elapsed += Time.deltaTime;

            yield return null;

        }

        StopCoroutine(infiniteRotation);
    }

    private void cardsRotationCorrection()
    {
        foreach( GameObject c in cards)
        {
            c.transform.eulerAngles = Vector3.zero;
            //c.transform.eulerAngles = -transform.eulerAngles;

        }
    }

    private void generateCardsPosition()
    {
        float x;
        float y;

        x = radius * Mathf.Sin((Mathf.PI * 21 * 2) / n);
        y = radius * Mathf.Cos((Mathf.PI * 21 * 2) / n);

        cardAdditionPos = new Vector2(transform.position.x + x, transform.position.y + y);

    }

   

    private IEnumerator rotate()
    {
        float percetageDur;

        Vector3 start = new Vector3(0, 0, 0);
        Vector3 end = new Vector3(0, 0, -360);


        while (true)
        {
            if(rotationElapsedT > (rotationDur/ rotationAcceleration))
            {
                rotationElapsedT = 0;
            }

            percetageDur = rotationElapsedT / (rotationDur/ rotationAcceleration);

            transform.eulerAngles = Vector3.Lerp(start, end, curve.Evaluate(percetageDur));


            rotationElapsedT += Time.deltaTime;

            yield return null;

        }



    }

   
}
