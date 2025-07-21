using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_ATK : MonoBehaviour
{
    [Header("Tower/s")]
    private GameObject towerPrefab;
    public List<Single_Tower_Controller> towers = new List<Single_Tower_Controller>();
    private int nTowers = 4;

    [Header("Spawn")]
    private Vector2[] spawnPos = new Vector2[4];
    private float spawnInterval = 0.3f;

    private Coroutine ToRespawn = null;
    //private Vector2 lastPos = Vector2.zero;

    [Header("Attack")]
    private float atkInterval = 0f;
    private bool isAttacking = false;
    private float waitToAtkT = 1.5f;
    [Header("player")]
    private GameObject player;


    [Header("Controller")]
    private Enemy_Controller mainController;
    private Health mainHealth;



    // Start is called before the first frame update
    void Start()
    {
        mainController = GetComponent<Enemy_Controller>();
        mainHealth = GetComponent<Health>();
        player = mainController.thePlayer;
        towerPrefab =  (GameObject)Resources.Load("Tower", typeof(GameObject));

        generateStartPos();

        StartCoroutine(spawnTowers());



    }

    private void generateStartPos()
    {
        List<Vector2> possiblePos = new List<Vector2>();
        possiblePos.Add(new Vector2(-1.5f, 1.5f));
        possiblePos.Add(new Vector2(-1.5f, -7.5f));
        possiblePos.Add(new Vector2(-11f, -2.5f));
        possiblePos.Add(new Vector2(8f, -2.5f));

        int rand, t = 0, i = 0 ;
        while (possiblePos.Count > 0)
        {

            rand  = Random.Range(0, possiblePos.Count);

            spawnPos[i] = possiblePos[rand];
            possiblePos.RemoveAt(rand);

            i++;
            t++;
        }



    }

    private void generateRandPos()
    {
        List<Vector2> possiblePos = new List<Vector2>();
        Vector2 testPos;
        int spawnZone, i = 0,  treshold = 0;

        while (possiblePos.Count < 4)
        {
            if(treshold > 200) { Debug.Log("generateRandPos() Treshold reached"); break; }

            spawnZone = Random.Range(1, 4);
            testPos = generateSpawnPos(spawnZone);
            
            if(Vector2.Distance(testPos,player.transform.position) >= 2f && checkTowerDistance(testPos, possiblePos) > 2f)
            {
                possiblePos.Add(testPos);
            }

            treshold++;
        }


        for(i=0; i < spawnPos.Length; i++)
        {
            spawnPos[i] = possiblePos[i];
        }

    }

    private  float checkTowerDistance(Vector2 test, List<Vector2> currentPos)
    {
        float min = 999;

        foreach(Vector2 p in currentPos)
        {
            if(Vector2.Distance(p,test) < min)
            {
                min = Vector2.Distance(p, test);
            }
        }

        return min;

    }
    private Vector2 generateSpawnPos(int zone)
    {
        switch (zone)
        {
            case 1:

                return new Vector2(Random.Range(-15, -9), Random.Range(-9, 9));

            case 2:
                return new Vector2(Random.Range(-9, 5), Random.Range(-9, 3));

            case 3:

                return new Vector2(Random.Range(5, 15), Random.Range(-9, 9));

            default:
                return Vector2.zero;

        }

    }

 

    // Update is called once per frame
    void Update()
    {


        if (towers.Count < 1 && isAttacking)
        {
            Debug.Log("towers < 1 and aTTacking,  start waitToRespawn");
            StartCoroutine(waitToRespawn());
            isAttacking = false;
        }

        if (isAttacking && towers.Count >= 1)
        {
            atkInterval += Time.deltaTime;

            if (atkInterval > 3f)
            {
                chooseTowerThatATK();
                atkInterval = 0f;
            }


        }

        

    }

    private IEnumerator waitToRespawn()
    {
        //Future animation
        mainHealth.takeDamage(mainHealth.maxHealth/3 + 5f);
        mainController.isHit();

        yield return new WaitForSeconds(3);

        
        atkInterval = 0;

        
        generateRandPos();
        StartCoroutine(spawnTowers());
    }

    private IEnumerator spawnTowers()
    {
        Debug.Log("Start SpawnTowers");
        float towerWaitTime = 1 + spawnInterval*4;
        float towerWaitAtkTime = 0;

        while (nTowers > 0) { 
            int temp = Mathf.Abs(nTowers - 4);

            GameObject tempShadow = Instantiate(towerPrefab, spawnPos[temp], transform.rotation, transform.parent);
            Single_Tower_Controller tempController = tempShadow.GetComponent<Single_Tower_Controller>();
            tempController.player = player;
            tempController.dadController = this;
            tempController.mainController = mainController;

            tempController.waitT = towerWaitTime;
            tempController.aimT = towerWaitAtkTime;
            tempController.waitToAtkT = waitToAtkT;
            towers.Add(tempController);


            nTowers--;
            towerWaitTime -= spawnInterval;
            towerWaitAtkTime += 0.7f;

            yield return new WaitForSeconds(spawnInterval);
        }

        nTowers = 4;


        yield return new WaitForSeconds(1.5f + towerWaitAtkTime);
        isAttacking = true;
        
        
    }


    private void chooseTowerThatATK()
    {
        int rand = 0;
        //int nTowers = towers.Count;
        if (towers.Count >= 2){ 
            rand = Random.Range(0, towers.Count);
            StartCoroutine(towers[rand].waitToAttack(waitToAtkT));
        }
        
        if(towers.Count == 1)
        {
            StartCoroutine(towers[rand].waitToAttack(waitToAtkT));
        }
    }

    public void resetAttack()
    {
        StopAllCoroutines();
        atkInterval = 4;
        if (!isAttacking) { isAttacking = true; }
    }

    private void allTowersATK()
    {
        //int nTowers = towers.Count;
        int rand = Random.Range(0, towers.Count);

        StartCoroutine(towers[rand].waitToAttack(waitToAtkT));

    }

}
