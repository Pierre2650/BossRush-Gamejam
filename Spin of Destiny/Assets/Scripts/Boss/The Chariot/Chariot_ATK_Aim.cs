using System;
using System.Collections;
using UnityEngine;

public class ChariotAim : MonoBehaviour
{
    [Header("To Init")]
    private LineRenderer myLR;
    public GameObject player;
    public GameObject boss;


    [Header("Aim")]
    public Vector2 lastPosition;
    [HideInInspector]public bool isAiming;
    private float timer = 0f;
    private float aimDuration = 2f;
    public float length;

    private bool flickering = false;

    // Start is called before the first frame update
    void Start()
    {
        myLR = GetComponent<LineRenderer>();
        myLR.positionCount = 2;
        isAiming = true;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (isAiming)
        {
            setPoints();


            timer += Time.deltaTime;

            if (timer >= aimDuration - 1 && timer < aimDuration && !flickering)
            {
                StartCoroutine(flickerAim(aimDuration-timer));
                flickering = true;

            }

            if (timer > aimDuration)
            {

                lastPosition = player.transform.position;
                isAiming = false;
                
                timer = 0f;
                flickering = false;
            }

        }



    }



    private void setPoints()
    {

        myLR.SetPosition(0, boss.transform.position);
        Vector3 temp = (boss.transform.position + ((player.transform.position - boss.transform.position).normalized)*length);

        myLR.SetPosition(1, temp);
    }

    public void setVisibleLine(bool visible)
    {

        myLR.enabled = visible;

    }

    private IEnumerator flickerAim(float remainsT)
    {
        float nbFlickers = 3;
        float t = remainsT / (nbFlickers*2);
        int count = 0;
        bool temp = false;

      
        while(count < (nbFlickers * 2))
        {
            setVisibleLine(temp);
            yield return new WaitForSeconds(t);

            temp = !temp;
            count++;

        }

        setVisibleLine(temp);

    }

}
