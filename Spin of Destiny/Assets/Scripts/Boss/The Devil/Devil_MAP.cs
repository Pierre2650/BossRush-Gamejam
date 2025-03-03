using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Devil_MAP : MonoBehaviour
{
    [Header("The Player")]
    private Enemy_Controller main_controller;
    private GameObject player;

    [Header("Player Shadow")]
    private GameObject plShadowPrefab;
    private GameObject plShadow;
    private Devil_MAP_Shadow_Controller plShadowController;

    [Header("Spawn Pos")]
    private Vector2 pos;
    private Vector2 lastSpawnPos = Vector2.zero;


    [Header("Mark")]
    private Sprite[] masks = new Sprite[2];
    private GameObject maskPrefab;
    private GameObject mask;
    private SpriteRenderer maskRenderer;

    // Start is called before the first frame update
    void Start()
    {
        plShadowPrefab = (GameObject)Resources.Load("Devil_MAP_Player_Shadow", typeof(GameObject));
        
        maskPrefab = (GameObject)Resources.Load("Devil_MAP_Mark", typeof(GameObject));

        masks[0] = Resources.Load<Sprite>("Devil_Mask_0");
        masks[1] = Resources.Load<Sprite>("Devil_Mask_1");

        main_controller = GetComponent<Enemy_Controller>();
        player = main_controller.thePlayer;

        StartCoroutine(waitToStart());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator waitToStart()
    {
        yield return new WaitForSeconds(3f);
        spawnPlayerShadow();
    }

    private void markPlayer()
    {
        Vector2 spawnPos = new Vector2(player.transform.position.x, player.transform.position.y + 1.9f);
        mask = Instantiate(maskPrefab, spawnPos, player.transform.rotation, player.transform);
        maskRenderer = mask.GetComponent<SpriteRenderer>();


    }

    private void changeMask(int i)
    {
        maskRenderer.sprite = masks[i];

    }
    private void spawnPlayerShadow()
    {
        Vector2 spawnPos = spawnPosGeneration();
        plShadow = Instantiate(plShadowPrefab, spawnPos, transform.rotation, transform.parent);
        plShadowController = plShadow.GetComponent<Devil_MAP_Shadow_Controller>();
        plShadowController.player = player;

        StartCoroutine(countdownToPull());
    
    }

    private void removePlayerShadow()
    {
        plShadowController.destroyChain();
        Destroy(plShadow);
    }

    private Vector2 spawnPosGeneration()
    {
        Vector2 spawnPos = Vector2.zero;
        Vector2 playerPos = player.transform.position;


        int temp = 0;
        
         int spawnZone = Random.Range(1, 4);
     


        do
        {
            spawnPos = generateSpawnPos(spawnZone);

            temp++;
        }
        while (spawnPos == playerPos && temp < 200 && Vector2.Distance(lastSpawnPos, spawnPos) < 6);

        lastSpawnPos = spawnPos;


        return spawnPos;


    }


    private Vector2 generateSpawnPos(int zone)
    {
        switch (zone)
        {
            case 1:

                return new Vector2(Random.Range(-15, -9), Random.Range(-9, 9));

            case 2:
                return new Vector2(Random.Range(-9, 5), Random.Range(-9, 3));

            case 3:
                    
                return new Vector2(Random.Range(5, 15), Random.Range(-9, 9));

            default:
                return Vector2.zero;

        }

    }

    private void chainPlayer()
    {
        plShadowController.chainPlayer = true;
        Destroy(mask);

    }

    private IEnumerator countdownToPull()
    {
         yield return new WaitForSeconds(3f);

        markPlayer();

        yield return new WaitForSeconds(5f);

        int i = 1;

        changeMask(i);

        
        float dur = 2f;
        
        yield return new WaitForSeconds(dur/4);
        changeMask(--i);
        yield return new WaitForSeconds(dur/4);
        changeMask(++i);
        yield return new WaitForSeconds(dur/4);
        changeMask(--i);
        yield return new WaitForSeconds(dur/4);
        changeMask(++i);

        yield return new WaitForSeconds(2);

        chainPlayer();

        StartCoroutine(waitToDestroy());

    }


    private IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(11f);
        removePlayerShadow();

        yield return new WaitForSeconds(5f);
        StartCoroutine(waitToStart());
    }


}
