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

    //private Vector2 lastPos = Vector2.zero;

    [Header("Attack")]
    private float atkInterval = 0f;
    private bool isAttacking = false;
    private float waitToAtkT = 1.5f;
    [Header("player")]
    private GameObject player;


    [Header("Controller")]
    private Enemy_Controller mainController;



    // Start is called before the first frame update
    void Start()
    {
        mainController = GetComponent<Enemy_Controller>();
        player = mainController.thePlayer;
        towerPrefab =  (GameObject)Resources.Load("Tower", typeof(GameObject));

        //spawnPos[0] = new Vector2(-1.5f, 1.5f);
        //spawnPos[1] = new Vector2(-1.5f, -7.5f);
        //spawnPos[2] = new Vector2(-11f, -2.5f);
        //spawnPos[3] = new Vector2(8f, -2.5f);

        generateRandPos();

        StartCoroutine(spawnTowers());



    }

    private void generateRandPos()
    {
        Vector2 temp = new Vector2(-1.5f, 1.5f);
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

    // Update is called once per frame
    void Update()
    {


        if(towers.Count < 1 && isAttacking)
        {
            StartCoroutine(waitToRespawn());
        }

        if(isAttacking)
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
        yield return new WaitForSeconds(3);

        isAttacking = false;
        atkInterval = 0;

        generateRandPos();
        StartCoroutine(spawnTowers());
    }

    private IEnumerator spawnTowers()
    {
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
        
        yield return new WaitForSeconds(1.5f + towerWaitAtkTime);
        nTowers = 4;
        isAttacking = true;
    }


    private void chooseTowerThatATK()
    {
        int rand = 0;
        //int nTowers = towers.Count;
        if (towers.Count > 2){ 
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
