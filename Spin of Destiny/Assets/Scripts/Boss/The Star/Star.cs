using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Star : MonoBehaviour
{
    [Header("FallingStar")]
    public Vector2 fallPos;
    public int  nbStars = 16;
    public GameObject fallZonePrefab;

    [Header("Spawn Interval")]
    private float intervalElapsed = 0f;
    private float interval = 0.8f;

    [Header("Player")]
    public GameObject player;
    private Vector2 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        
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


       /* if (spawnZone == 0) {
            spawnPos = playerPos;
        }
        else { 
            spawnPos = generateSpawnPos(spawnZone); 
        }

        spawnFallZone(spawnPos);
       */




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
