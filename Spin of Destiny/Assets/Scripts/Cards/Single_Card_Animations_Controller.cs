using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.GPUSort;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Single_Card_Animations_Controller : MonoBehaviour
{

    [Header("Berzier Mouvement")]
    private float speed = 1.0f;
    private float t = 0f;
    private Vector3 P0;
    private Vector3 P1;
    private Vector3 P2;

    private bool isSpreading = false;


    [Header("Size Change")]
    private float szChangeDur = 0.4f;
    private float szChangeElapsedT = 0;

    public AnimationCurve curve;


    [Header("Position Change")]
    public Vector2 posToGo;
    public GameObject newParent;
    private float changePosElapsedT = 0;
    private float changePosDur = 0.5f;


    [Header("Rotation Correction")]
    private float correctionElapsedT = 0;


    [Header("Deck")]
    public Cards_Deck_Controller deckController;

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
        StartCoroutine(waitToStopSpread(delay));

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
    private IEnumerator changeSize(Vector2 end)
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

        transform.localScale = end;

        szChangeElapsedT = 0f;


    }



    private IEnumerator waitToStopSpread(float i)
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

        transform.position = pos;
        changePosElapsedT = 0f;



        transform.parent = newParent.transform;

        if (selection)
        {
            StartCoroutine(correctRotation());

        }



    }

    private IEnumerator correctRotation()
    {
       

        float percetageDur;
        

        Vector3 start = transform.eulerAngles;

        Vector3 end = new Vector3(0,0,360);

        

        while (correctionElapsedT < 0.2f)
        {
            percetageDur = correctionElapsedT / 0.2f;

          
            transform.eulerAngles = Vector3.Lerp(start, end, curve.Evaluate(percetageDur));
      

            correctionElapsedT += Time.deltaTime;
            yield return null;

        }

        transform.eulerAngles = Vector3.zero;
        
        correctionElapsedT = 0f;

        
        deckController.switchToPlayableCards();

        disableFalseCard();

        

    }

    private void disableFalseCard()
    {
        this.gameObject.SetActive(false);
        
    }



}
