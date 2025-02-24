using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World_MAP_Orb : MonoBehaviour
{

    [Header("Evolution")]
    private float evoElapsedTime = 0f;
    private float evoTime = 5f;
    private int size = 1;

    [Header("MiniOrbs")]
    private GameObject miniOrbsPrefab;
    private bool spawn = false;
    private float toSpawnElapsed = 0.6f;
    private float tospawnTime = 0.5f;
    private float nbOrbs = 3;

    // Start is called before the first frame update
    void Start()
    {

        miniOrbsPrefab = (GameObject)Resources.Load("World_MAP_orbSpawn", typeof(GameObject));
    }

    // Update is called once per frame
    void Update()
    {

        evoElapsedTime += Time.deltaTime;
        if (evoElapsedTime > evoTime)
        {
            evolve();

            spawn = true;
            evoElapsedTime = 0;
        }


        if (spawn) { 
            spawnOrb();
        }



          


        
    }

    private void evolve()
    {

        size++;

        if (size == 4)
        {
            Destroy(this.gameObject);
        }

        transform.localScale = new Vector2(size, size);
    }

    private void spawnOrb()
    {

        toSpawnElapsed += Time.deltaTime;
        if (toSpawnElapsed > tospawnTime)
        {

            Instantiate(miniOrbsPrefab, this.transform.position, this.transform.rotation, this.transform.parent);
            nbOrbs--;
            toSpawnElapsed = 0;
        }

        if (nbOrbs == 0)
        {
            toSpawnElapsed = 0.5f;
            spawn = false;
            nbOrbs = 3;
        }

    }
}
