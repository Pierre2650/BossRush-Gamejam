using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Chariot : Tarot
{
    private Rigidbody2D myRb;
    public Vector2 targetPos;
    public GameObject prefabRedLine;

    private ChariotAim aimLine;

    private bool charge = false;

    private float chargeElapsetT = 0;
    private float chargeD = 0.3f;
    private AnimationCurve curve = new AnimationCurve();

    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        prefabRedLine = GameObject.Find("BossLineAim");
        aimLine = prefabRedLine.GetComponent<ChariotAim>();
        aimLine.enabled = true;

        // Add keys for a straight line from (0, 0) to (1, 1)
        curve.AddKey(new Keyframe(0f, 0f, 1f, 1f)); // Start point with tangent = 1 (straight slope)
        curve.AddKey(new Keyframe(1f, 1f, 1f, 1f)); // End point with tangent = 1

        startAim();

    }



    // Update is called once per frame
    void Update()
    {
        if (aimLine.stop && !charge)
        {
            targetPos = aimLine.lastPosition;
            StartCoroutine(goCharge());


        }
       
    }

    private void startAim()
    {
        //instantiate
    }

    private IEnumerator goCharge()
    {
        charge = true;

        float percentageDur = 0;

        Vector2 start = transform.position;
        Vector2 end = targetPos;


        while (chargeElapsetT < chargeD)
        {

            percentageDur = chargeElapsetT / chargeD;

            transform.position = Vector2.Lerp(start, end, curve.Evaluate(percentageDur));

            chargeElapsetT += Time.deltaTime;
            yield return null;

        }

        chargeElapsetT = 0;

        aimLine.stop = false;
        charge = false;
        


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(targetPos, 0.25f);

    }


}
