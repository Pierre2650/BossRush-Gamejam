using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World_MAP : MonoBehaviour
{
    private GameObject orbPrefab;
    private GameObject orb = null;
    private Vector2 spawnPos = Vector2.zero;

    [Header("Atk Pace")]
    [SerializeField] private float waitTimer = 0;
    private float waitDuration = 5f;

    private GameObject thePlayer;

    public CameraShake CameraShake;

    
    // Start is called before the first frame update
    void Start()
    {
        orbPrefab = (GameObject)Resources.Load("World_MAP_Orb", typeof(GameObject));
        thePlayer = GetComponent<Enemy_Controller>().thePlayer;
        CameraShake = GetComponent<CameraShake>();

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
        int zone = Random.Range(1, 4);

        switch (zone)
        {
            case 1:

                return new Vector2(Random.Range(-17.5f, -7.15f), Random.Range(-9.0f, 10f));
                

            case 2:

                return new Vector2(Random.Range(-7.10f, 4.10f), Random.Range(-9f, 3.2f));

            case 3:

                return new Vector2(Random.Range(4.15f, 17.5f), Random.Range(-9f, 10f));

            default:

                return new Vector2(Random.Range(-13.2f, 17.5f), Random.Range(-9f , 6.6f));
        }


    }

    private void spawnOrb()
    {
        spawnPos = newSpawnPos();
        orb = Instantiate(orbPrefab, spawnPos, transform.rotation, transform.parent);
        World_MAP_Orb temp = orb.GetComponent<World_MAP_Orb>();
        temp.mapController = this;
        temp.mainController = GetComponent<Enemy_Controller>();
        temp.player = thePlayer;

        waitTimer = 0f;

    }
}
