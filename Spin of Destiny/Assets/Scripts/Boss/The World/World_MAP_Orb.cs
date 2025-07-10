using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class World_MAP_Orb : Enemy_Controller
{
    // PROBLEM PLAYER  GAMME OBJECT MISSING,  REPLACE :AIN CONTROLLER

    public Enemy_Controller mainController;

    [Header("Init")]
    private SpriteRenderer mySpr;
    private CircleCollider2D myCC;

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
    public GameObject player;
    private bool isfinishing;
    public float timeBeforeDestruction;

    public float touchDamage;

    private float explosionAnimDur = 0f;

    [Header("Health")]
    private GameObject uiPrefab;
    private List<Image> uiHealthBar = new List<Image>();
    private Coroutine barVisibility = null;


    private void OnEnable()
    {
        if (miniOrbsPrefab == null) { 
            miniOrbsPrefab = (GameObject)Resources.Load("World_MAP_orbSpawn", typeof(GameObject));
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        mySpr = GetComponent<SpriteRenderer>();
        myCC = GetComponent<CircleCollider2D>();

        myHealth = GetComponent<Health>();
        myAni = GetComponent<Animator>();
        isfinishing = false;
        lineController.gameObject.SetActive(false);

        miniOrbsPrefab = (GameObject)Resources.Load("World_MAP_orbSpawn", typeof(GameObject));

        // set prefab and a fonction to instantiated?
        uiPrefab = (GameObject)Resources.Load("World_MAP_HealthBar", typeof(GameObject));
        GameObject temp = Instantiate(uiPrefab, new Vector2(transform.position.x, transform.position.y + 1.5f), transform.rotation, mainController.bossUI.transform);
        myHealth.healthBar = temp.transform.GetChild(0).GetComponent<HealthBar>();
        uiHealthBar.Add(temp.transform.GetChild(0).GetChild(0).GetComponent<Image>());
        uiHealthBar.Add(temp.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>());

        setVisibleHealth(false);

        getExplosionAnimDur();

    }

    private void getExplosionAnimDur()
    {
        AnimationClip[] clips = myAni.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (string.Equals(clip.name, "World_Map_explode")) 
            {
                explosionAnimDur = clip.length;
                break;
            } ;
            
        }

    }

    private void setVisibleHealth(bool state)
    {
        foreach (Image im in uiHealthBar)
        {
            im.enabled = state;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!isfinishing) { 
            evoElapsedTime += Time.deltaTime; 
        }

        if (evoElapsedTime > evoTime )
        {
            if (size == 4)
            {
                lineController.gameObject.SetActive(true);
                explosion();
            }
            else {
                evolve();
                StartCoroutine(waitToSpawnMiniOrbs());
          
                evoElapsedTime = 0;
               
            }
            
        }

        if(lineController.radius>=Vector2.Distance(transform.position, player.transform.position) && !isfinishing){
            
        }


        if (spawn ) { 
            spawnOrb();
        }
          


        
    }

    private void evolve()
    {

        size++;
        transform.localScale = new Vector2(size, size);
        StartCoroutine(push());

    }

    private IEnumerator push()
    {
        myCC.enabled = true;
        yield return new WaitForSeconds(1f);
        myCC.enabled = false;
    }

    private void explosion()
    {
        setVisibleHealth(false);
        myAni.SetTrigger("Explode");
        StartCoroutine(waitToHideSprite());

        CircleCollider2D[] colliders = GetComponents<CircleCollider2D>();
        foreach (CircleCollider2D cc in colliders)
        {
            cc.enabled = false;
        }

        
        player.GetComponent<Health>().takeDamage(explosionDamage);
        player.GetComponent<PlayerController>().isHit();


        isfinishing = true;
        evoElapsedTime = 0;
        StartCoroutine(waitToDestroy());
    }

    private IEnumerator waitToHideSprite()
    {
        yield return new WaitForSeconds(explosionAnimDur);
        mySpr.enabled = false;
    }

    private IEnumerator waitToSpawnMiniOrbs()
    {
        yield return new WaitForSeconds(0.5f);
        spawn = true;
    }

    private void spawnOrb()
    {

        toSpawnElapsed += Time.deltaTime;
        if (toSpawnElapsed > tospawnTime)
        {
            
            GameObject temp = Instantiate(miniOrbsPrefab, this.transform.position, this.transform.rotation, this.transform.parent);
            World_MAP_MiniOrb_Controller temp2 = temp.GetComponent<World_MAP_MiniOrb_Controller>();
            temp2.player = this.player;
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


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (barVisibility == null)
        {
            barVisibility = StartCoroutine(waitToHideHealthBar());
        }
        else
        {
            StopCoroutine(barVisibility);
            barVisibility = StartCoroutine(waitToHideHealthBar());
        }


        if (other.tag == "Player")
        {

            Health playerHealth = other.GetComponent<Health>();
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (!playerHealth.isInvincible)
            {
                if (!myCC.IsTouching(other.GetComponent<Collider2D>()))
                {
                    playerHealth.takeDamage(touchDamage);
                    playerController.isHit();
                }
                Vector2 knockbackDir = (playerController.transform.position - transform.position).normalized;
                playerController.startKnockBack(knockbackDir.normalized, 0.5f, 35, 0.2f, 0.2f);
            }
        }
    }


    private IEnumerator waitToHideHealthBar()
    {
        setVisibleHealth(true);
        yield return new WaitForSeconds(2f);

        setVisibleHealth(false);

        barVisibility = null;
    }

    private IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(timeBeforeDestruction);
        myHealth.takeDamage(9999);
        isHit();
    }


}
