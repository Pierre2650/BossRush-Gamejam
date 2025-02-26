using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class World_ATK_Circle_Orbs_Controller : MonoBehaviour
{

    [Header("Rotation")]
    private float rotationElapsedT = 0f;
    private float rotationDur = 2.5f;
    public AnimationCurve curve;

    [Header("Rotation Speed")]
    public float rotationAcceleration = 1;
    private float changeSpeedElapsedT = 0f;
    public float changeSpeedDur = 0.25f;


    [Header("Orbs")]
    public List<GameObject> orbs;

    private bool show = false;
    private float showObsElapsedT = 0f;
    private float showObsIntervalDur = 0.1f;
       
    public float orbsMoveDistance = 2.2f;
    public float orbsMoveSpeed = 1;

    [Header("Head Controller")]
    public World_ATK worldConroller;


    


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(rotate());
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyUp(KeyCode.C))
        {
            StopAllCoroutines();
        }

    }

    public IEnumerator showOrbs()
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

      
        moveAwayAllOrbs(1);
        yield return new WaitForSeconds(2f);
        worldConroller.isAttacking = true;

    }

    public IEnumerator hideOrbs()
    {

        int i = orbs.Count - 1;
        int temp = 0;

        while (i >= 0)
        {

            orbs[i].SetActive(false);
            i--;
            temp++;

            if (temp > 20)
            {
                Debug.Log("temp = " + temp);
                break;
            }

            yield return null;

        }

    }

    public void moveAwayAllOrbs(int nbTimes) {
        World_ATK_Circle_Single_Orb_Controller tempController;

        foreach (GameObject orb in orbs) {
  
            tempController = orb.GetComponent<World_ATK_Circle_Single_Orb_Controller>();
            StartCoroutine(tempController.moveAwayAOrb(nbTimes));

        }
    }

    public void approachAllOrbs(int nbTimes)
    {

        World_ATK_Circle_Single_Orb_Controller tempController;

        foreach (GameObject orb in orbs)
        {
  

            tempController = orb.GetComponent<World_ATK_Circle_Single_Orb_Controller>();
            StartCoroutine(tempController.approachAOrb(nbTimes));
        }
    }


    public void allOrbsToCenter()
    {

        World_ATK_Circle_Single_Orb_Controller tempController;

        foreach (GameObject orb in orbs)
        {


            tempController = orb.GetComponent<World_ATK_Circle_Single_Orb_Controller>();
            StartCoroutine(tempController.goToCenter());
        }

        StartCoroutine(waitToHide());
    }


    private IEnumerator waitToHide()
    {
        //wait time synchrinised with single orb movement duration
        yield return new WaitForSeconds(1f);
        StartCoroutine(hideOrbs());

    }




    private IEnumerator rotate()
    {
        float percetageDur;

        Vector3 start = new Vector3(0,0,0);
        Vector3 end = new Vector3(0, 0, -360);


        while (rotationElapsedT < rotationDur/rotationAcceleration)
        {
            percetageDur = rotationElapsedT / (rotationDur / rotationAcceleration);

            transform.eulerAngles = Vector3.Lerp(start, end, curve.Evaluate(percetageDur));


            rotationElapsedT += Time.deltaTime;

            yield return null;

        }


        rotationElapsedT = 0f;

        StartCoroutine(rotate());

    }

    public IEnumerator changeRotationSpeed(float speed)
    {
        float percetageDur;

        float start = rotationAcceleration;


        while (changeSpeedElapsedT < changeSpeedDur)
        {
            percetageDur = changeSpeedElapsedT / changeSpeedDur;

            rotationAcceleration = Mathf.Lerp(start, speed, curve.Evaluate(percetageDur));


            changeSpeedElapsedT += Time.deltaTime;

            yield return null;

        }


        changeSpeedElapsedT = 0f;


    }


}
