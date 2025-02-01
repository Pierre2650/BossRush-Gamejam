using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lovers : MonoBehaviour
{
    [Header("Shadow/s")]
    public GameObject shadowPrefab;
    public List<GameObject> shadows = new List<GameObject>();
    public int nShadows = 3;

    [Header("Spawn")]
    public Vector2 spawnPos = Vector2.zero;
    private Vector2 lastPos = Vector2.zero;


    [Header("player")]
    public GameObject player;


    public float intervaltest = 0f;


    // Start is called before the first frame update
    void Start()
    {
        SpawnShadow();
        nShadows--;


    }

    // Update is called once per frame
    void Update()
    {
        if (nShadows > 0) { 
            intervaltest += Time.deltaTime;
        }

        if (intervaltest > 1.5f) {
            SpawnShadow();
            nShadows--;
            intervaltest = 0f;
        }

        
    }

    private void SpawnShadow()
    {
       

        int spawnZone = Random.Range(1, 4);
        int temp = 0;

        do
        {
            spawnPos = generateSpawnPos(spawnZone);
            temp++;

        } while (Vector2.Distance(lastPos, spawnPos) < 3f && Vector2.Distance(player.transform.position, spawnPos) < 6f && temp < 200);

        if (temp != 1)
        {
            Debug.Log("SpawnPos on Shadow, Temp = " + temp);
        }

        lastPos = spawnPos;

        GameObject tempShadow = Instantiate(shadowPrefab, spawnPos, transform.rotation, transform.parent);
        shadows.Add(tempShadow);

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
}
