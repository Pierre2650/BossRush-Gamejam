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



    public IEnumerator changeSize(Vector2 start, Vector2 end , bool destroy)
    {

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
