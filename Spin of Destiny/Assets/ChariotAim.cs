using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.WSA;

public class ChariotAim : MonoBehaviour
{
    private GameObject player;
    public GameObject Boss;

    public Vector2 lastPosition;

    private LineRenderer myLR;

    public bool stop = false;
    private float timer = 0f;
    public float scanDuration = 3f;

    // Start is called before the first frame update
    void Start()
    {
        myLR = GetComponent<LineRenderer>();
        myLR.positionCount = 2;
        player = GameObject.Find("Player");
        Boss = GameObject.Find("Boss");
    }

    // Update is called once per frame
    void Update()
    {

        if (!stop)
        {
            myLR.enabled = true;
            setPoints();
            timer += Time.deltaTime;

            if (timer > scanDuration)
            {
                lastPosition = player.transform.position;
                timer = 0f;

                stop = true;

            }

        }
        else { 
            myLR.enabled = false;
        }

        

    }


    private void setPoints()
    {

        myLR.SetPosition(0, Boss.transform.position);
        myLR.SetPosition(1, player.transform.position);

    }

    public void resetAim()
    {
        //stop = false;
    }


  

}
