using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.WSA;

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
    private float scanDuration = 1.5f;
    public float length;

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
            myLR.enabled = true;
            setPoints();
            timer += Time.deltaTime;

            if (timer > scanDuration)
            {

                lastPosition = player.transform.position;
                timer = 0f;
                isAiming = false;


            }

        }



    }


    private void setPoints()
    {

        myLR.SetPosition(0, boss.transform.position);
        myLR.SetPosition(1, boss.transform.position + (player.transform.position - boss.transform.position).normalized * length);

    }

    public void setVisibleLine(bool visible)
    {
        myLR.enabled = visible;

    }
}
