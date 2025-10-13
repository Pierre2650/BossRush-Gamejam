using System.Collections;
using UnityEngine;

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

    [Header("Conditions")]
    private bool worldATK = false;

    // Start is called before the first frame update
    void Start()
    {
        plShadowPrefab = (GameObject)Resources.Load("Devil_MAP_Player_Shadow", typeof(GameObject));
        
        maskPrefab = (GameObject)Resources.Load("Devil_MAP_Mark", typeof(GameObject));

        masks[0] = Resources.Load<Sprite>("Devil_Mask_0");
        masks[1] = Resources.Load<Sprite>("Devil_Mask_1");

        main_controller = GetComponent<Enemy_Controller>();
        player = main_controller.thePlayer;

        if(GetComponent<World_ATK>() != null)
        {
            worldATK = true;
        }


        StartCoroutine(waitToStart());
    }

    private IEnumerator waitToStart()
    {
        yield return new WaitForSeconds(5f);
        spawnPlayerShadow();
    }

    private void markPlayer()
    {
        Vector2 spawnPos = new Vector2(player.transform.position.x, player.transform.position.y + 1.5f);
        mask = Instantiate(maskPrefab, spawnPos, player.transform.rotation, player.transform);
        maskRenderer = mask.GetComponent<SpriteRenderer>();
        mask.GetComponent<AudioSource>().Play();


    }

    private void changeMask(bool i)
    {
        if (i)
        {
            maskRenderer.sprite = masks[1];
            mask.GetComponent<AudioSource>().pitch = 0.7f;
            mask.GetComponent<AudioSource>().Play();
        }
        else
        {
            maskRenderer.sprite = masks[0];
            mask.GetComponent<AudioSource>().pitch = 1f;
            mask.GetComponent<AudioSource>().Play();
        }

    }
    private void spawnPlayerShadow()
    {
        Vector2 spawnPos;
        if (worldATK) {

            spawnPos = spawnPosCorners();
        }
        else
        {

            spawnPos = spawnPosGeneration();

        }

        plShadow = Instantiate(plShadowPrefab, spawnPos, transform.rotation, transform.parent);
        plShadowController = plShadow.GetComponent<Devil_MAP_Shadow_Controller>();
        plShadowController.player = player;
        plShadowController.fatherController = this;

        StartCoroutine(countdownToPull());
    
    }

    private Vector2 spawnPosCorners()
    {
        int spawnZone, temp = 0;
        
        Vector2 pos = Vector2.zero;
        float posX = 0, posY = 0;


        

        if (transform.position.x > -17.2 && transform.position.x < -7.9f)
        {
            return new Vector2(Random.Range(7.5f, 17), Random.Range(-9f, 9f));

        }
        else if (transform.position.x > 4.9 && transform.position.x < 17.5f)
        {
            return new Vector2(Random.Range(-17, -10.5f), Random.Range(-9f, 9f));
        }
        else
        {
            int randSidesZones = Random.Range(0, 2);

            if (randSidesZones == 0) 
            {

                return new Vector2(Random.Range(7.5f, 17), Random.Range(-9f, 9f));

            }
            else
            {
                return new Vector2(Random.Range(-17, -10.5f), Random.Range(-9f, 9f));
            }
        }

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

                return new Vector2(Random.Range(-17, -10.5f), Random.Range(-9f, 9f));

            case 2:
                return new Vector2(Random.Range(-10f, 7.5f), Random.Range(-9, 0f));

            case 3:
                    
                return new Vector2(Random.Range(7.5f, 17), Random.Range(-9, 9));

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
        yield return new WaitForSeconds(2f);

        markPlayer();

        yield return new WaitForSeconds(4f);

        bool i = true;

        changeMask(i);
        i = !i;

        float dur = 2f;
        int count = 0;

        while(count < 4)
        {

            yield return new WaitForSeconds(dur / 4);
            changeMask(i);
            i = !i;

            count++;
        }
        mask.GetComponent<PickableObj_Mouvement>().enabled = false;
        
        
        yield return new WaitForSeconds(3);

        chainPlayer();

    }

    public void callWaitToDestroy()
    {
        StartCoroutine(waitToDestroy());
    }
    private IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(10f);
      
        removePlayerShadow();

        yield return new WaitForSeconds(9f);
        StartCoroutine(waitToStart());
    }


    private void OnDestroy()
    {
        if(mask != null)
        {
            Destroy(mask);
        }
    }

}
