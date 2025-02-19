using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_ATK : Tarot_Controllers
{
    [Header("Tower/s")]
    private GameObject towerPrefab;
    public List<Single_Tower_Controller> towers = new List<Single_Tower_Controller>();
    private int nTowers = 4;

    [Header("Spawn")]
    private Vector2[] spawnPos = new Vector2[4];
    private float spawnInterval = 0f;

    //private Vector2 lastPos = Vector2.zero;

    [Header("Attack")]
    private float atkInterval = 0f;


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

        spawnPos[0] = new Vector2(-1.5f, 1.5f);
        spawnPos[1] = new Vector2(-1.5f, -7.5f);
        spawnPos[2] = new Vector2(-11f, -2.5f);
        spawnPos[3] = new Vector2(8f, -2.5f);

        SpawnTower();



    }

    // Update is called once per frame
    void Update()
    {
        if (nTowers > 0) { 
            spawnInterval += Time.deltaTime;

            if (spawnInterval > 0.3f)
            {
                SpawnTower();
                spawnInterval = 0f;
            }
        }
        else
        {
            atkInterval += Time.deltaTime;

            if (atkInterval > 3f)
            {
                chooseTowerThatATK();
                atkInterval = 0f;
            }


        }




       

        
    }

    private void SpawnTower()
    {
        int temp = Mathf.Abs(nTowers - 4);

        nTowers--;

        GameObject tempShadow = Instantiate(towerPrefab, spawnPos[temp], transform.rotation, transform.parent);
        Single_Tower_Controller tempController = tempShadow.GetComponent<Single_Tower_Controller>();
        tempController.player = player;
        towers.Add(tempController);

    }


    private void  chooseTowerThatATK()
    {
        //int nTowers = towers.Count;
        int rand = Random.Range(0, towers.Count);

        StartCoroutine(towers[rand].waitToAttack(1.5f));

    }

}
