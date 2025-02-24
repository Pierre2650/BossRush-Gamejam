using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class World_ATK_MegaOrb : MonoBehaviour
{
    [Header("To Init")]
    private LineRenderer myLR;
    private Rigidbody2D myRB;

    [Header("The PLayer")]
    public GameObject player;


    [Header("Start")]
    private float startAnimDur = 1f;
    private float startAnimElapsedT = 0;
    public AnimationCurve curve;


    [Header("Aim")]
    private bool isAiming = false;
    private float aimElapsedT = 0;
    private Personal_Direction_Finder dirFinder ;
    private Vector2 dir;

    [Header("Throw")]
    private bool throwOrb = false;
    private float speed = 6f;

    private float throwElapsedT = 0f;
    private float throwDur = 0.7f;


    [Header("Main Controller")]
    public World_ATK headController;

    // Start is called before the first frame update
    void Start()
    {
        myLR = GetComponent<LineRenderer>();
        myRB = GetComponent<Rigidbody2D>();


        StartCoroutine(start());
    }

    // Update is called once per frame
    void Update()
    {
        


        if (isAiming)
        {
            aim();
            setPoints();
            
        }

        if (throwOrb) {
            checkMapLim();
        }

    }


    private void FixedUpdate()
    {
        //throw
        
          if(throwOrb){
            myRB.velocity = dir * speed;
          
         }else{
           myRB.velocity = Vector2.zero;
         }
         
    }


    private void checkMapLim()
    {
        bool stop = false;
        if (transform.position.x > 15.5f || transform.position.x < -15.5f)
        {
            stop = true;

        }

        if (transform.position.y < -8.5f && !stop)
        {
            stop = true;

        }

        if (stop) {
            StopAllCoroutines();
            throwOrb = false;
            transform.localScale = Vector3.one * 6;
            StartCoroutine(toDestroy());
        }
    }

    private void aim()
    {
        aimElapsedT += Time.deltaTime;
        if (aimElapsedT > 0.2f) {
            dirFinder.target = player.transform.position;
            dir = dirFinder.findDirToTarget();
            dir *= 5;
            aimElapsedT = 0;
        }
    }


    private IEnumerator start()
    {
        yield return new WaitForSeconds(1.5f);


        float percetageDur;

        Vector2 start = transform.localScale;
        Vector2 end = Vector2.one*2 ;


        while (startAnimElapsedT < startAnimDur )
        {
            percetageDur = startAnimElapsedT / startAnimDur;

            transform.localScale = Vector2.Lerp(start, end, curve.Evaluate(percetageDur));


            startAnimElapsedT += Time.deltaTime;

            yield return null;

        }

        transform.localScale = end;

        startAnimElapsedT = 0f;

        StartCoroutine(moveUp());


    }

    private IEnumerator moveUp()
    {
        float percetageDur, duration, elapsedT;
        duration = 0.5f;
        elapsedT = 0f;

        Vector2 start = transform.localPosition;
        Vector2 end = new Vector2(start.x, start.y + 2.5f);


        while (elapsedT < duration)
        {
            percetageDur = elapsedT / duration;

            transform.localPosition = Vector2.Lerp(start, end, curve.Evaluate(percetageDur));


            elapsedT += Time.deltaTime;

            yield return null;

        }

        dirFinder = new Personal_Direction_Finder(transform.position, 0.05f);
        isAiming = true;
        StartCoroutine(waitToThrow());

    }

    private IEnumerator waitToThrow()
    {
        yield return new WaitForSeconds(1.5f);

        myLR.enabled = false;

        float duration = 1.5f;

        yield return new WaitForSeconds(duration/6);

        myLR.enabled = true;
        yield return new WaitForSeconds(duration / 6);

        myLR.enabled = false;

        yield return new WaitForSeconds(duration / 6);

        myLR.enabled = true;
        yield return new WaitForSeconds(duration / 6);

        myLR.enabled = false;

        yield return new WaitForSeconds(duration / 6);

        myLR.enabled = true;
        yield return new WaitForSeconds(duration / 6);


        myLR.enabled = false;

        isAiming = false;
        throwOrb = true;
        StartCoroutine(expand());
    }


    private IEnumerator expand()
    {

        float percetageDur;

        Vector2 start = transform.localScale;
        Vector2 end = Vector2.one * 6;


        while (throwElapsedT < throwDur)
        {
            percetageDur = throwElapsedT / throwDur ;

            transform.localScale = Vector2.Lerp(start, end, curve.Evaluate(percetageDur));


            throwElapsedT += Time.deltaTime;

            yield return null;

        }

        transform.localScale = end;

        throwElapsedT = 0f;
        throwOrb = false;

        StartCoroutine(toDestroy());

    }

    private IEnumerator toDestroy()
    {
        yield return new WaitForSeconds(2f);
        headController.restartStateMachine();
        Destroy(this.gameObject);
    }

    private void setPoints()
    {
       Vector2 temp =  new Vector2(transform.position.x + dir.x, transform.position.y + dir.y);

        myLR.SetPosition(0, transform.position);
        myLR.SetPosition(1, temp);

    }


    private void OnDrawGizmos()
    {
        Vector2 temp = new Vector2(transform.position.x + dir.x, transform.position.y + dir.y);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(temp, 0.25f);

    }

}
