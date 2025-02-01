using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow_Attack_Controller : MonoBehaviour
{

    [Header("Spawn Anim")]
    private float animElapsed = 0f;
    private float animDur = 0.2f;
    private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [Header("Duration")]
    private float toDestroyElapsed = 0f;
    private float toDestroyDur = 3f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnAnim());

    }


    private void Update()
    {
        toDestroyElapsed += Time.deltaTime;
        if (toDestroyElapsed > toDestroyDur)
        {
            Destroy(gameObject);

        }
    }


    private IEnumerator spawnAnim()
    {

        Vector2 start = new Vector2( transform.localPosition.x, transform.localPosition.y + 1f);
        Vector2 end = transform.localPosition;

        float percentageDur = 0f;

        while (animElapsed < animDur)
        {

            percentageDur = animElapsed / animDur;

            transform.localPosition = Vector2.Lerp(start, end, curve.Evaluate(percentageDur));

            animElapsed += Time.deltaTime;
            yield return null;

        }
        animElapsed = 0;
      

    }
}
