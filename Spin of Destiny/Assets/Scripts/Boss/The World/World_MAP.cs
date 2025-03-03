using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World_MAP : MonoBehaviour
{
    private GameObject orbPrefab;
    private GameObject orb = null;
    private Vector2 spawnPos = Vector2.zero;

    [Header("Atk Pace")]
    private float waitTimer = 0;
    private float waitDuration = 2f;

    
    // Start is called before the first frame update
    void Start()
    {
        orbPrefab = (GameObject)Resources.Load("World_MAP_Orb", typeof(GameObject));

        spawnOrb();
    }

    // Update is called once per frame
    void Update()
    {
 
        if (orb == null)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer > waitDuration) {

                spawnOrb();

                waitTimer = 0f;

            } 

        }

        
        
    }


 
    private Vector2 newSpawnPos()
    {
        return new Vector2(Random.Range(-13.2f, 17.5f), Random.Range(6.6f, -10f));

    }

    private void spawnOrb()
    {
        spawnPos = newSpawnPos();
        orb = Instantiate(orbPrefab, spawnPos, transform.rotation, transform.parent);

    }
}
