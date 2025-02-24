using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Devil_Scythe_Controller : MonoBehaviour
{
    [Header("Attack Zone")]
    private Vector2 Dir = Vector2.zero;


    [Header("Animation")]
    private float swingElapsedT = 0f;
    private float swingDuration = 0.1f;
    private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    private Coroutine attackMouv = null;


    [Header("Parent")]
    private MeshDevilAtkZone meshController;

    // Start is called before the first frame update

    private void Awake()
    {
        meshController = transform.parent.GetComponent<MeshDevilAtkZone>();
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {

        StartCoroutine(waitToAtk());
    }




    private IEnumerator waitToAtk()
    {
        yield return new WaitForSeconds(0.3f);

        meshController.myMshR.enabled = false;
        StartCoroutine(attack());
        
    }

    private IEnumerator attack()
    {

        float percentageDur = 0;

        Vector3 start = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        Vector3 end = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 182);



        while (swingElapsedT < swingDuration)
        {

            percentageDur = swingElapsedT / swingDuration;

            transform.eulerAngles = Vector3.Lerp(start, end, curve.Evaluate(percentageDur));

            swingElapsedT += Time.deltaTime;
            yield return null;

        }

        swingElapsedT = 0;
        attackMouv = null;

        StartCoroutine(endAtk());

    }

    private IEnumerator endAtk()
    {
        
        yield return new WaitForSeconds(0.5f);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 44);
        meshController.attackEnded();
    }

}
