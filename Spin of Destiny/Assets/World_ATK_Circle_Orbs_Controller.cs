using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class World_ATK_Circle_Orbs_Controller : MonoBehaviour
{

    [Header("Rotation")]
    private float rotationElapsedT = 0f;
    public float rotationDur = 2.5f;
    public float rotationAcceleration = 2;
    public AnimationCurve curve;

 
    [Header("Orbs")]
    public List<GameObject> orbs;

    private bool show = false;
    private float showObsElapsedT = 0f;
    private float showObsIntervalDur = 0.2f;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(rotate());
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.Z)) {
            //StartCoroutine(rotate());
            //moveAwayAllOrbs();

            StartCoroutine(showOrbs());
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            approachAllOrbs();
        }


        if (Input.GetKeyUp(KeyCode.C))
        {
            StopAllCoroutines();
        }

    }

    private IEnumerator showOrbs()
    {
        int i = orbs.Count-1;
        int temp = 0;

        while ( i >= 0 )
        {
        
            orbs[i].SetActive(true);
            i--;
            temp++;

            if (temp > 20)
            {
                Debug.Log("temp = " + temp);
                break;
            }

            yield return new WaitForSeconds(showObsIntervalDur);

        }

      


    }

    private void moveAwayAllOrbs() {

        World_ATK_Circle_Single_Orb_Controller tempController;

        foreach (GameObject orb in orbs) {
            // orb.transform.localPosition = orb.transform.localPosition * 2;

            tempController = orb.GetComponent<World_ATK_Circle_Single_Orb_Controller>();
            StartCoroutine(tempController.moveAwayAOrb());
        }
    }

    private void approachAllOrbs()
    {

        World_ATK_Circle_Single_Orb_Controller tempController;

        foreach (GameObject orb in orbs)
        {
            // orb.transform.localPosition = orb.transform.localPosition * 2;

            tempController = orb.GetComponent<World_ATK_Circle_Single_Orb_Controller>();
            StartCoroutine(tempController.approachAOrb());
        }
    }





    private IEnumerator rotate()
    {
        float percetageDur;

        Vector3 start = new Vector3(0,0,0);
        Vector3 end = new Vector3(0, 0, -360);


        while (rotationElapsedT < rotationDur)
        {
            percetageDur = rotationElapsedT / rotationDur;

            transform.eulerAngles = Vector3.Lerp(start, end, curve.Evaluate(percetageDur));


            rotationElapsedT += Time.deltaTime;

            yield return null;

        }


        rotationElapsedT = 0f;

        StartCoroutine(rotate());

    }
}
