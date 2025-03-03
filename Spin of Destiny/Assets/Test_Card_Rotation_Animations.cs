using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Test_Card_Rotation_Animations : MonoBehaviour
{
    public List<Vector2> circlePoints = new List<Vector2>();
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            //generateCardsPosition();
            StartCoroutine(rotate());
        }
        
    }


    private void generateCardsPosition()
    {
        float x;
        float y;
        for (int i = 0; i < n; i++)
        {
            x = radius * Mathf.Sin((Mathf.PI * i * 2) / n);
            y = radius * Mathf.Cos((Mathf.PI * i * 2) / n);

            circlePoints.Add(new Vector2(transform.position.x + x, transform.position.y + y ));
        }


    }


    private void addCardToRotation()
    {
        //Set the position por the cards

        nextCardPosition++;
    }

    private IEnumerator rotate()
    {
        float percetageDur;

        Vector3 start = new Vector3(0, 0, 0);
        Vector3 end = new Vector3(0, 0, -360);


        while (rotationElapsedT < rotationDur / rotationAcceleration)
        {
            percetageDur = rotationElapsedT / (rotationDur / rotationAcceleration);

            transform.eulerAngles = Vector3.Lerp(start, end, curve.Evaluate(percetageDur));


            rotationElapsedT += Time.deltaTime;

            yield return null;

        }


        rotationElapsedT = 0f;

        StartCoroutine(rotate());

    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 0.2f);


        if (circlePoints.Count > 0)
        {

            for(int i = 0; i < circlePoints.Count; i++)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(circlePoints[i], 0.2f);
            }
        }
        



    }
}
