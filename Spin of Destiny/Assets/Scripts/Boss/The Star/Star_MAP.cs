using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Star_MAP : Tarot_Controllers
{
    [Header("FallingStar")]
    private int  nbStars = 20;
    private GameObject fallZonePrefab;

    private Vector2 lastStarPos = Vector2.zero;

    [Header("Spawn Interval")]
    private float intervalElapsed = 0f;
    private float interval = 0.3f;

    [Header("Player")]
    private GameObject player;
    private Vector2 playerPos;


    [Header("Coldown")]
    private float coldownElapsed = 0f;
    private float coldownDur = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        fallZonePrefab = (GameObject)Resources.Load("StarFallZone", typeof(GameObject));
        player = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;

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
        else {
            coldownElapsed += Time.deltaTime;

            if (coldownElapsed > coldownDur)
            {
                nbStars = 20;
                lastStarPos = Vector2.zero;
                coldownElapsed = 0f;
            }

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
            Debug.Log(nbStars);

            spawnPos = new Vector2(playerPos.x, playerPos.y - 0.7f);
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
}
