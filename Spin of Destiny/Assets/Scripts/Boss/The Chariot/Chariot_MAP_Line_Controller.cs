using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chariot_MAP_Line_Controller : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;

    [Header("Line Parameters")]
    private Vector2 startPos;
    private float startPosCalibration = 2.5f;
    private Vector2 startSize;
    private Vector2 endSize;

    private char dir = 'L';


    [Header("Stretch")]
    private float stretchElapsedT = 0f;
    private float stretchDur = 1;
    public AnimationCurve curve;

    [Header("Charge")]
    private GameObject chargePrefab;

    // Start is called before the first frame update
    void Start()
    {
        chargePrefab = (GameObject)Resources.Load("Chariot_MAP_Charge", typeof(GameObject));
        
    }

    // Update is called once per frame
    void Update()
    {



        /*if (Input.GetKeyDown(KeyCode.Z)) {

            setDirection();

            StartCoroutine(stretch());
        }*/
        
    }

    private void setDirection()
    {
        int rand = Random.Range(0, 4);

        switch (rand) { 
            case 0:
                //from left to right 
                dir = 'R';

                transform.position = new Vector2(-19, player.transform.position.y + startPosCalibration);
                startSize = new Vector2(-1, 5);
                endSize = new Vector2(39, 5);

                stretchDur = 0.5f;

                break;
            case 1:
                //from right to left 
                dir = 'L';
                transform.position = new Vector2(19, player.transform.position.y + startPosCalibration);
                startSize = new Vector2(1, 5);
                endSize = new Vector2(-39, 5);

                stretchDur = 0.5f;

                break;
            case 2:
                //from up to down
                dir = 'D';

                startPosCalibration *= -1;

                transform.position = new Vector2(player.transform.position.x + startPosCalibration, 12);
                startSize = new Vector2(5, 1);
                endSize = new Vector2(5, 23);

                stretchDur = 0.3f;

                break;
            case 3:
                //from down to up
                dir = 'U';

                startPosCalibration *= -1;

                transform.position = new Vector2(player.transform.position.x + startPosCalibration, -12);
                startSize = new Vector2(5, -1);
                endSize = new Vector2(5, -23);

                stretchDur = 0.3f;

                break;
        }
    
    }

    public IEnumerator stretch()
    {
        float percetageDur;

        Vector2 start = startSize;
        Vector2 end = endSize ;


        while (stretchElapsedT < stretchDur) { 
            percetageDur = stretchElapsedT/ stretchDur;

            transform.localScale = Vector2.Lerp(start, end , curve.Evaluate(percetageDur));


            stretchElapsedT += Time.deltaTime;

            yield return null;

        }


        yield return new WaitForSeconds(0.5f);

        spawnCharge();

        stretchElapsedT = 0f;

        startPosCalibration = 2.5f;

    }


    private void spawnCharge()
    {
        Vector2 tempPos;
        
        if(dir == 'R' || dir == 'L')
        {
            tempPos = new Vector2(transform.position.x, transform.position.y - startPosCalibration);
        }
        else
        {
            tempPos = new Vector2(transform.position.x - startPosCalibration , transform.position.y);
        }
        

        GameObject temp = Instantiate(chargePrefab, tempPos, transform.rotation, transform.parent.transform.parent);
        Chariot_MAP_Charge_Controller tempController = temp.GetComponent<Chariot_MAP_Charge_Controller>();
        tempController.direction = dir;


    }

}
