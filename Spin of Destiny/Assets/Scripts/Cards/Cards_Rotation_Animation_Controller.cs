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
    public int nextCardPosition = 0;


    [Header("Rotation")]
    public float rotationDur;
    private float rotationElapsedT = 0;
    public float rotationAcceleration;


    public AnimationCurve curve;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        generateCardsPosition();

    }


    private void OnEnable()
    {
        StartCoroutine(rotate());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
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
            if(rotationElapsedT > rotationDur)
            {
                rotationElapsedT = 0;
            }

            percetageDur = rotationElapsedT / rotationDur;

            transform.eulerAngles = Vector3.Lerp(start, end, curve.Evaluate(percetageDur));


            rotationElapsedT += Time.deltaTime;

            yield return null;

        }



    }

   
}
