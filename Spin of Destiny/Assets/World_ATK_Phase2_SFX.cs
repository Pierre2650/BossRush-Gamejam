using System.Collections;
using UnityEngine;

public class World_ATK_Phase2_SFX : MonoBehaviour
{

    [Header("Rotation")]
    private float rotationElapsedT = 0f;
    private float rotationDur = 1f;
    public AnimationCurve curve;

    [Header("Expansion")]
    private float expandElapsedT = 0f;
    private float expandDur = 2f;


    private void Start()
    {
        StartCoroutine(changeSize(new Vector2 (0,0), new Vector2(4,4), false));

    }

    private IEnumerator rotate()
    {
        float percetageDur;

        Vector3 start = new Vector3(0, 0, 0);
        Vector3 end = new Vector3(0, 0, 360);


        while (true)
        {
            if(rotationElapsedT > rotationDur)
            {

                rotationElapsedT = 0f;

            }

            percetageDur = rotationElapsedT / rotationDur;

            transform.eulerAngles = Vector3.Lerp(start, end, curve.Evaluate(percetageDur));


            rotationElapsedT += Time.deltaTime;

            yield return null;

        }


        //rotationElapsedT = 0f;

        //StartCoroutine(rotate());

    }


    public IEnumerator changeSize(Vector2 start, Vector2 end , bool destroy)
    {
        //yield return new WaitForSeconds(0.5f);

        float percetageDur;


        while (expandElapsedT < expandDur)
        {
            

            percetageDur = expandElapsedT / expandDur;

            transform.localScale = Vector2.Lerp(start, end, curve.Evaluate(percetageDur));


            expandElapsedT += Time.deltaTime;

            yield return null;

        }


        expandElapsedT = 0f;

        if (destroy)
        {
            yield return new WaitForSeconds(0.5f);

            Destroy(this.gameObject);
        }

    }
}
