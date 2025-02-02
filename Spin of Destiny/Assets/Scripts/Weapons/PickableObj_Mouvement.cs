using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickableObj_Mouvement : MonoBehaviour
{
    //To know if to switch mouvement
    private bool switchPos = false;

    [SerializeField] private float duration;
    public float elapsedT;

    //Curve for type of mouvement
    [SerializeField] private AnimationCurve curve;


    // Start is called before the first frame update

    private void OnEnable() {
        StartCoroutine(floatingMouv());
    }



    IEnumerator floatingMouv()
    {
        Vector2 startPos = transform.localPosition; 
        Vector2 endPos;
        if (!switchPos)
        {
            endPos = new Vector2(startPos.x, startPos.y + 0.3f);
        }
        else {
        
            endPos = new Vector2(startPos.x, startPos.y - 0.3f);
        }
        

        float percentageDur = 0;


        while(elapsedT < duration) 
        {
            elapsedT += Time.deltaTime;

            percentageDur = elapsedT / duration;

            transform.localPosition = Vector2.Lerp(startPos, endPos, curve.Evaluate(percentageDur));

            elapsedT += Time.deltaTime;
            yield return null;

        }

        switchPos = !switchPos;
        elapsedT = 0;


        StartCoroutine(floatingMouv());

    }

    private void OnDisable()
    {
        elapsedT = 0;
        StopAllCoroutines();
    }


}
