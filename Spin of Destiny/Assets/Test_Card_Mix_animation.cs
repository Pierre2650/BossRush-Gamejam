using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class Test_Card_Mix_animation : MonoBehaviour
{

    [Header("Berzier Mouvement")]
    private float speed = 1.0f;
    private float t = 0f;
    private Vector3 P0;
    private Vector3 P1;
    private Vector3 P2;

    private bool isSpreading = false;


    [Header("SIze Change")]
    private float szChangeDur = 0.4f;
    private float szChangeElapsedT = 0;

    public AnimationCurve curve;


    [Header("Test")]
    public Vector2 posToGo;
    public GameObject newParent;
    private float changePosElapsedT = 0;
    private float changePosDur = 0.5f;

    // Update is called once per frame
    void Update()
    {


        if (isSpreading)
        {
            spread();

        }

    }


    private void setControlPoints()
    {
        P0 = transform.position;
        P2 = new Vector3(P0.x + 15, P0.y, 0);
        P1 = new Vector3(P0.x + 5.5f , P0.y -6, 0);
        
    }


    public void startSpread(float delay)
    {
        setControlPoints();

        isSpreading = true;
        StartCoroutine(waitToStop(delay));

    }
    private void spread()
    {
        //quadratic Berzier exemple
        if (t < 1f)
        {
            transform.position = P1 + Mathf.Pow((1 - t), 2) * (P0 - P1) + Mathf.Pow(t, 2) * (P2 - P1);

            t = t + speed * Time.deltaTime;

        }
        else
        {
            
            t = 0f;
            isSpreading = false;

        }

    }

    public void shrink()
    {
        Vector2 size = new Vector2(0.6f, 0.6f);
        StartCoroutine(changeSize(size));

    }

    public void increase()
    {
        Vector2 size = new Vector2(1f, 1f);
        StartCoroutine(changeSize(size));

    }
    public IEnumerator changeSize(Vector2 end)
    {
        float percetageDur;

        Vector2 start = transform.localScale;


        while (szChangeElapsedT < szChangeDur)
        {
            percetageDur = szChangeElapsedT / szChangeDur;

            transform.localScale = Vector3.Lerp(start, end, curve.Evaluate(percetageDur));


            szChangeElapsedT += Time.deltaTime;

            yield return null;

        }


        szChangeElapsedT = 0f;


    }



    private IEnumerator waitToStop(float i)
    {

        yield return new WaitForSeconds(0.05f + i);
        isSpreading = false;

    }


    public IEnumerator moveToPos(Vector2 pos, bool selection)
    {


        float percetageDur;

        Vector2 start = transform.position;


        while (changePosElapsedT < changePosDur)
        {
            percetageDur = changePosElapsedT / changePosDur;

            transform.position = Vector3.Lerp(start, pos, curve.Evaluate(percetageDur));


            changePosElapsedT += Time.deltaTime;

            yield return null;

        }


        changePosElapsedT = 0f;



        transform.parent = newParent.transform;

        if (selection) { 
            transform.eulerAngles = Vector3.zero; 
        }
        


    }

   
}
