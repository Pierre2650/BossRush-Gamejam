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
    
    [Header("Damage")]
    public float explosionDamage;
    public CircleLineController lineController;
    private GameObject player;
    private bool isfinishing;
    public float timeBeforeDestruction;

    // Start is called before the first frame update
    void Start()
    {
        isfinishing = false;
        lineController.gameObject.SetActive(false);
        print("lala");
        player = GameObject.Find("Player");
        miniOrbsPrefab = (GameObject)Resources.Load("World_MAP_orbSpawn", typeof(GameObject));
        if(miniOrbsPrefab == null)print("lala");
    }

    // Update is called once per frame
    void Update()
    {

        evoElapsedTime += Time.deltaTime;
        if (evoElapsedTime > evoTime)
        {
            if (size == 4)
            {
                lineController.gameObject.SetActive(true);                
            }
            else {
                evolve();
            }

            spawn = true;
            evoElapsedTime = 0;
        }

        if(lineController.radius>=Vector2.Distance(transform.position, player.transform.position)&& !isfinishing){
            isfinishing = true;
            player.GetComponent<Health>().takeDamage(explosionDamage);
            Destroy(gameObject,timeBeforeDestruction);
        }


        if (spawn) { 
            spawnOrb();
        }

        if(GetComponent<Health>().isDead){
            Destroy(gameObject);
        }

          


        
    }

    private void evolve()
    {

        size++;
        GetComponent<CircleCollider2D>().radius += new Vector2(size, size).magnitude/2;
        transform.localScale = new Vector2(size, size);
    }

    private void spawnOrb()
    {

        toSpawnElapsed += Time.deltaTime;
        if (toSpawnElapsed > tospawnTime)
        {
            print("hihih");
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
