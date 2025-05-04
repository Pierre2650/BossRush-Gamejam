using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil_ATK_Scythe_Controller : MonoBehaviour
{
    private SpriteRenderer mySprR;
    private Sprite baseSprite;

    [Header("Attack Zone")]
    private Vector2 Dir = Vector2.zero;


    [Header("Animation")]
    private float swingElapsedT = 0f;
    private float swingDuration = 0.15f;
    private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    private Coroutine attackMouv = null;


    [Header("Impact Frame")]
    private CameraShake cameraShake;



    [Header("Sprites")]
    public Sprite scythEffect ;



    [Header("Parent")]
    private Devil_ATK_Mesh_AtkZone meshController;



    // Start is called before the first frame update

    private void Awake()
    {
        mySprR = GetComponent<SpriteRenderer>();
        baseSprite = mySprR.sprite;
        scythEffect = Resources.Load<Sprite>("Devil_scythe_FX"); 
        meshController = transform.parent.GetComponent<Devil_ATK_Mesh_AtkZone>();
        
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {

        StartCoroutine(waitToAtk());
    }

    public void setCamera(CameraShake x)
    {
        cameraShake = x;
    }



    private IEnumerator waitToAtk()
    {
        yield return new WaitForSeconds(0.3f);

        meshController.myMshR.enabled = false;
        StartCoroutine(attack());

        meshController.myPlC.enabled = true;
        cameraShake.randomCameraShake();


    }

    private IEnumerator attack()
    {
       

        float percentageDur = 0;

        Vector3 start = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        Vector3 end = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 182);

        mySprR.sprite = scythEffect;
       
        while (swingElapsedT < swingDuration)
        {

            percentageDur = swingElapsedT / swingDuration;

            transform.eulerAngles = Vector3.Lerp(start, end, curve.Evaluate(percentageDur));



            swingElapsedT += Time.deltaTime;

            if(swingElapsedT > 0.1f)
            {
                mySprR.sprite = baseSprite;
            }

            yield return null;

        }

        swingElapsedT = 0;
        attackMouv = null;

        meshController.myPlC.enabled = false;
        StartCoroutine(endAtk());

    }

    

    private IEnumerator endAtk()
    {

        
        yield return new WaitForSeconds(0.5f);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 44);
        meshController.attackEnded();
    }

}
