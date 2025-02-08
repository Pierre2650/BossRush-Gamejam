using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chariot_MAP : Tarot_Controllers
{
    [Header("Boss Controller")]
    private Enemy_Controller MainController;

    [Header("player")]
    private GameObject player;

    [Header("Line")]
    private GameObject linePrefab;

    [Header("Spawn Controller")]
    private float spawnIntervalElapsedT = 0f;
    private float spawnIntervalStep = 4f;

    // Start is called before the first frame update
    void Start()
    {
        MainController = GetComponent<Enemy_Controller>();
        player = MainController.thePlayer;
        linePrefab = (GameObject)Resources.Load("Chariot_MAP_LineATK", typeof(GameObject));

    }

    // Update is called once per frame
    void Update()
    {

        spawnIntervalElapsedT += Time.deltaTime;
        if (spawnIntervalElapsedT > spawnIntervalStep)
        {
            spawn();
            spawnIntervalElapsedT = 0;

        }


    }

    private void instantiateLine()
    {
        GameObject temp = Instantiate(linePrefab, transform.parent);
        Chariot_MAP_Line_Controller temp2 = temp.transform.GetChild(0).GetComponentInChildren<Chariot_MAP_Line_Controller>();
        temp2.player = player;
    }

    private void spawn()
    {
        int rand = Random.Range(0, 2);

        if (rand == 0)
        {
            instantiateLine();
        }
        else
        {
            instantiateLine();
            instantiateLine();
        }

    }
}
