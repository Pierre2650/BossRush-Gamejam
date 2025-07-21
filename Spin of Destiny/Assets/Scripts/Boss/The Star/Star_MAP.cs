using System.Collections;
using UnityEngine;

public class Star_MAP : MonoBehaviour
{
    [Header("FallingStar")]
    private int  nbStars = 25;
    private GameObject fallZonePrefab;

    private Vector2 lastStarPos = Vector2.zero;

    [Header("Spawn Interval")]
    private float intervalElapsed = 0f;
    private float interval = 0.3f;

    private int counToPattern = 0;

    [Header("Player")]
    private GameObject player;
    private Vector2 playerPos;


    [Header("Coldown")]
    private float coldownElapsed = 0f;
    private float coldownDur = 5f;

    private Enemy_Controller mainController;

    [Header("Patterns")]
    Vector2 startPos, endPos;
    Coroutine currentPattern = null;

    // Start is called before the first frame update
    void Start()
    {
        fallZonePrefab = (GameObject)Resources.Load("StarFallZone", typeof(GameObject));

        mainController = GetComponent<Enemy_Controller>();
        player = mainController.thePlayer;
        currentPattern = StartCoroutine(waitToStart());
    }

    private IEnumerator waitToStart()
    {
        yield return new WaitForSeconds(5);
        currentPattern = null;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;

        if (currentPattern == null)
        {
            if (nbStars > 0)
            {
                intervalElapsed += Time.deltaTime;

                if (intervalElapsed > interval)
                {

                    fallZoneGeneration();
                    nbStars--;
                    intervalElapsed = 0f;
                }
            }
            else
            {
                coldownElapsed += Time.deltaTime;

                if (coldownElapsed > coldownDur)
                {
                    nbStars = 30;
                    lastStarPos = Vector2.zero;
                    coldownElapsed = 0f;
                    counToPattern++;
                }

            }

        }
        


        if(counToPattern == 2)
        {
            chooseFallPattern();
            counToPattern = 0;
        }
        
    }

    private void chooseFallPattern()
    {
        int rand = Random.Range(1, 4);

        switch (rand)
        {
            case 1:
                currentPattern = StartCoroutine(fallZonePattern1());
                break;
            case 2:
                currentPattern = StartCoroutine(fallZonePattern2());
                break;
            case 3:
                currentPattern = StartCoroutine(fallZonePattern3());
                break;

        }

    }

    private void fallZoneGeneration()
    {
        Vector2 spawnPos = Vector2.zero;
        int spawnZone;
        int i = Mathf.Abs(nbStars - 10);

        if (i < 3)
        {
            spawnZone = i + 1;
        }
        else
        {
            spawnZone = Random.Range(1, 4);
        }



        int temp = 0;

        if (nbStars % 4 == 0)
        {
            spawnPos = new Vector2(playerPos.x, playerPos.y - 0.4f);
        }
        else
        {

            do
            {
                spawnPos = generateSpawnPos(spawnZone);
                temp++;

            } while (Vector2.Distance(lastStarPos, spawnPos) < 2.5f && temp < 200);

            lastStarPos = spawnPos;

            if (temp != 1) { 
                Debug.Log("SpawnPos on Star, Temp = "+temp);
            }
            
        }

        spawnFallZone(spawnPos);
       




    }

    private IEnumerator fallZonePattern1( )
    {
        Vector2 start = Vector2.zero, end = Vector2.zero;

        float sign = 1;
        float diagonalSign = 1;
        float spawnStep = 1.5f;
        float movePosStep = 4f;
        int dir = Random.Range(1, 5);

        switch (dir)
        {
            case 1:
                start = new Vector2(-18.5f, -10.2f);
                end = new Vector2(18.5f, 10.5f);
                sign *= 1;

                startPos = new Vector2(start.x + spawnStep+(1*sign), start.y);
                endPos = new Vector2(start.x, start.y + spawnStep);

                break;
            case 2:
                start = new Vector2(18.5f, 10.5f);
                end = new Vector2(-18.5f, -10.2f);
                sign *= -1;

                startPos = new Vector2(start.x, start.y - spawnStep);
                endPos = new Vector2(start.x - spawnStep+(1 * sign), start.y );
                break;
            case 3:

                start = new Vector2(-18.5f, 10.5f);
                end = new Vector2(18.5f, -10.2f);
                sign *= -1;

                diagonalSign *= -1;

                startPos = new Vector2(start.x, start.y - spawnStep);
                endPos = new Vector2(start.x + spawnStep + 1, start.y);
                break;
            case 4:

                start = new Vector2(18.5f, -10.2f);
                end = new Vector2(-18.5f, 10.5f);
                sign *= -1;

                diagonalSign *= -1;

                startPos = new Vector2(start.x - spawnStep + (1 * sign), start.y);
                endPos = new Vector2(start.x, start.y + spawnStep);
                break;
             
        }

        
        Vector2 spawnPos = startPos;
        

        int temp = 0;
        while (Vector2.Distance(startPos, end) > 2f)
        {
            if (temp > 299)
            {
                Debug.Log("Treshhold reached  on fallZonePattern1() out of loop");
                break;
            }

            StartCoroutine(spawnDiagonalLine(spawnPos,endPos,spawnStep, diagonalSign));

            yield return new WaitForSeconds(0.5f);

            


            chooseMouvement(dir, end, movePosStep, sign);


             spawnPos = startPos;
            temp++;

            
        }




        currentPattern = null;

    }


    private void chooseMouvement(int dir, Vector2 end, float movePosStep, float sign)
    {

        switch (dir)
        {
            case 1:

                if (Mathf.Abs(startPos.x - end.x) <= 2)
                {
                    startPos = new Vector2(startPos.x, startPos.y + (movePosStep * sign));
                }
                else
                {
                    startPos = new Vector2(startPos.x + ((movePosStep+2) * sign), startPos.y);
                }

                if (Mathf.Abs(endPos.y - end.y) <= 2f)
                {
                    endPos = new Vector2(endPos.x + ((movePosStep + 2) * sign), endPos.y);
                }
                else
                { 
                    endPos = new Vector2(endPos.x, endPos.y + (movePosStep * sign));        
                }

                break;

            case 2:

                if (Mathf.Abs(startPos.y - end.y) <= 2f)
                {  
                    startPos = new Vector2(startPos.x + ((movePosStep + 2) * sign), startPos.y);
                }
                else
                {
                    startPos = new Vector2(startPos.x, startPos.y + (movePosStep * sign));
                }

                if (Mathf.Abs(endPos.x - end.x) <= 2f)
                { 
                    endPos = new Vector2(endPos.x, endPos.y + (movePosStep * sign));
                }
                else
                {
                    endPos = new Vector2(endPos.x + ((movePosStep + 2) * sign), endPos.y);                 
                }

                break;
            case 3:

                if (Mathf.Abs(startPos.y - end.y) <= 2f)
                {
                    startPos = new Vector2(startPos.x + ((movePosStep + 2)), startPos.y);
                }
                else
                {
                    startPos = new Vector2(startPos.x, startPos.y + (movePosStep * sign));
                }

                if (Mathf.Abs(endPos.x - end.x) <= 2f)
                {
                    endPos = new Vector2(endPos.x, endPos.y + (movePosStep * sign));
                }
                else
                {
                    endPos = new Vector2(endPos.x + ((movePosStep + 2)), endPos.y);
                }
                break;
            case 4:

                if (Mathf.Abs(startPos.x - end.x) <= 2)
                {
                    startPos = new Vector2(startPos.x, startPos.y + (movePosStep));
                }
                else
                {
                    startPos = new Vector2(startPos.x + ((movePosStep + 2) * sign), startPos.y);
                }

                if (Mathf.Abs(endPos.y - end.y) <= 2f)
                {
                    endPos = new Vector2(endPos.x + ((movePosStep + 2) * sign), endPos.y);
                }
                else
                {
                    endPos = new Vector2(endPos.x, endPos.y + (movePosStep));
                }
                break;
        }

    }

    private IEnumerator spawnDiagonalLine(Vector2 spawnPos, Vector2 endpos, float spawnStep , float sign)
    {
        int temp2 = 0;
        float limitR = 21f, limitL = -20.6f, limitU = 12f, limitD = -11.8f
            ;
        while ((spawnPos.x < limitR && spawnPos.x > limitL) && (spawnPos.y < limitU && spawnPos.y > limitD) )
        {
            if (temp2 > 299)
            {
                Debug.Log("Treshhold reached  on fallZonePattern1() out of loop");
                break;
            }



            spawnFallZone(spawnPos);

            spawnPos = new Vector2(spawnPos.x - spawnStep*sign, spawnPos.y + spawnStep);

            yield return new WaitForSeconds(0.1f);


            temp2++;

        }

    }

    private IEnumerator fallZonePattern2()
    {
        float elapsedT = 0;
        float dur = 5f;

        int temp = 0;
        while (elapsedT < dur)
        {
            if (temp > 299)
            {
                Debug.Log("Treshhold reached  on fallZonePattern1() out of loop");
                break;
            }

            
            spawnFallZone(new Vector2(playerPos.x, playerPos.y - 0.3f));

            elapsedT += Time.deltaTime + 0.5f;
            yield return new WaitForSeconds(0.5f);

            temp++;

        }

        currentPattern = null;

    }

    private IEnumerator fallZonePattern3()
    {

        Vector2 start = Vector2.zero, end = Vector2.zero;

        float sign = 1;
        float spawnStep = 1.5f;
        float movePosStep = 4f;
        int dir = Random.Range(1, 5);

        switch (dir)
        {
            case 1:
                start = new Vector2(-18.5f, -10.2f);
                end = new Vector2(18.5f, -10.2f);
                sign *= 1;

                startPos = new Vector2(start.x + spawnStep +1, start.y );
                endPos = new Vector2(start.x + spawnStep + 1, 10.5f);

                break;
            case 2:
                start = new Vector2(18.5f, -10.2f);
                end = new Vector2(-18.5f, -10.2f);
                sign *= -1;

                startPos = new Vector2(start.x - spawnStep - 1, start.y );
                endPos = new Vector2(start.x - spawnStep - 1, 10.5f);
                break;
            case 3:

                start = new Vector2(-18.5f, 10.5f);
                end = new Vector2(-18.5f, -10.5f);
                sign *= -1;

    

                startPos = new Vector2(start.x , start.y -spawnStep);
                endPos = new Vector2(start.x , start.y - spawnStep);
                break;
            case 4:

                start = new Vector2(-18.5f, -10.5f);
                end = new Vector2(-18.5f, 10.5f);
                sign *= 1;

               
                startPos = new Vector2(start.x, start.y + spawnStep);
                endPos = new Vector2(start.x, start.y + spawnStep);
                break;

        }


        Vector2 spawnPos = startPos;


        int temp = 0;
        while (Vector2.Distance(startPos, end) > 2f)
        {
            if (temp > 299)
            {
                Debug.Log("Treshhold reached  on fallZonePattern1() out of loop");
                break;
            }

            StartCoroutine(spawnLine(dir, spawnPos, endPos, spawnStep));

            yield return new WaitForSeconds(0.5f);




            chooseMouvement2(dir, end, movePosStep, sign);


            spawnPos = startPos;
            temp++;


        }

        currentPattern = null;
    }

    private IEnumerator spawnLine(int dir,Vector2 spawnPos, Vector2 endPos, float spawnStep)
    {
        int temp2 = 0;
        float limitR = 21f, limitL = -20.6f, limitU = 12f, limitD = -11.8f;


            while ((spawnPos.x < limitR && spawnPos.x > limitL) && (spawnPos.y < limitU && spawnPos.y > limitD))
            {
                if (temp2 > 299)
                {
                    Debug.Log("Treshhold reached  on fallZonePattern3() out of loop");
                    break;
                }



                spawnFallZone(spawnPos);

                switch (dir)
                {
                    case 1:
                    case 2:
                        spawnPos = new Vector2(spawnPos.x, spawnPos.y + spawnStep);
                        yield return new WaitForSeconds(0.1f);
                    break;

                    case 3:
                    case 4:
                        spawnPos = new Vector2(spawnPos.x + spawnStep , spawnPos.y);
                        yield return new WaitForSeconds(0.05f);

                    break;

                    default:
                        yield return new WaitForSeconds(0.1f);
                    break;

            }



                //yield return new WaitForSeconds(0.1f);


                temp2++;


            }
    }

    private void chooseMouvement2(int dir, Vector2 end, float movePosStep, float sign)
    {

        switch (dir)
        {
            case 1:

                
                    startPos = new Vector2(startPos.x + (movePosStep * sign), startPos.y);
                

                break;

            case 2:

                
                    startPos = new Vector2(startPos.x + (movePosStep * sign), startPos.y );
                

                break;
            case 3:

                
                    startPos = new Vector2(startPos.x , startPos.y + (movePosStep * sign));
                
              
                break;
            case 4:

                    startPos = new Vector2(startPos.x, startPos.y + (movePosStep * sign));
             
                break;
        }

    }



    private Vector2 generateSpawnPos(int zone)
    {
        switch (zone)
        {
            case 1:
                return new Vector2(Random.Range(-15, -8), Random.Range(-9, 9));

            case 2:
                return new Vector2(Random.Range(-8, 5), Random.Range(-9, 2));

            case 3:
                return new Vector2(Random.Range(5, 16), Random.Range(-9, 9));

            default:
                return Vector2.zero;

        }

    }

    private void spawnFallZone(Vector2 spawnPos)
    {
        Instantiate(fallZonePrefab,spawnPos,transform.rotation,transform.parent);

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(startPos, 0.5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(endPos, 0.5f);
    }
}
