using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Single_Tower_Controller : Enemy_Controller
{
    [Header("init")]
    private LineRenderer myLR;
    private CircleCollider2D myCC;

    [Header("Dad")]
    public Tower_ATK dadController;

    [Header("Aim")]
    public GameObject player;
    private Vector2 lastPlayerPos;
    private Vector2 attackDir;
    private Vector2 SpawnFlamesPos;

    [Header("Spawn")]
    public float waitT;
    public float aimT;
    public float waitToAtkT;

    [Header("Attack")]
    private GameObject flamePrefab;
    private float spawnIntervalElapsed = 0f;
    private float spawnIntervalDur = 0.02f;

    private float stopDur = 0.1f;
    private Coroutine stopCoroutine = null;

    private int maxAttacks = 1;
    private int nAttacks;

    private bool isAttacking = false;


    [Header("Health")]
    private GameObject uiPrefab;
    private GameObject uiBar;
    private List<Image> uiHealthBar = new List<Image>();
    private Coroutine barVisibility = null;

    public Enemy_Controller mainController;






    // Start is called before the first frame update
    void Start()
    {
        myLR = GetComponent<LineRenderer>();
        myHealth = GetComponent<Health>();
        myAni = GetComponent<Animator>();
        myCC = GetComponent<CircleCollider2D>();
        hurtSFX = GetComponent<AudioSource>();

        flamePrefab = (GameObject)Resources.Load("TowerAttack", typeof(GameObject));
        StartCoroutine(spawnWait(waitT,aimT, waitToAtkT));


        // set prefab and a fonction to instantiated?
        uiPrefab = (GameObject)Resources.Load("Tower_ATK_HealthBar", typeof(GameObject));
        uiBar = Instantiate(uiPrefab, new Vector2(transform.position.x, transform.position.y + 5.5f), transform.rotation, mainController.bossUI.transform);
        myHealth.healthBar = uiBar.transform.GetChild(0).GetComponent<HealthBar>();
        uiHealthBar.Add(uiBar.transform.GetChild(0).GetChild(0).GetComponent<Image>());
        uiHealthBar.Add(uiBar.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>());

        setVisibleHealth(false);
    }


    private void setVisibleHealth(bool state)
    {
        foreach (Image im in uiHealthBar)
        {
            im.enabled = state;
        }

    }

    private IEnumerator spawnWait(float wDur, float aDur, float waitToAtkT)
    {
        //Wait time aftter spawn
        yield return new WaitForSeconds(wDur);

        //Wait time to attack
        yield return new WaitForSeconds(aDur);
        setVisibleLine(true);
        StartCoroutine(waitToAttack(2f));
    }

    // Update is called once per frame
    void Update()
    {

        if (isAttacking)
        {
            //check is arrived at last position
            if (Vector2.Distance(SpawnFlamesPos, lastPlayerPos) <= 1f)
            {

                if (stopCoroutine == null)
                {
                     stopCoroutine = StartCoroutine(stopAttack());
                }

            }

            spawnFlames();

        }
        else { 
            aim();
        }


    }

    private void setPoints()
    {

        myLR.SetPosition(0, transform.position);
        myLR.SetPosition(1, player.transform.position);

    }

    public void setVisibleLine(bool visible)
    {
        if (myLR.enabled != visible){ 
            myLR.enabled = visible;
        }

    }


    private void aim()
    {


        setPoints();
           
        lastPlayerPos = player.transform.position;

        attackDir = (player.transform.position - transform.position).normalized;

        SpawnFlamesPos = new Vector2(transform.position.x + attackDir.x/3, transform.position.y + attackDir.y/3);

    }

    private void spawnFlames()
    {

        spawnIntervalElapsed += Time.deltaTime;
        if (spawnIntervalElapsed > spawnIntervalDur) {

            SpawnFlamesPos = new Vector2(SpawnFlamesPos.x + attackDir.x, SpawnFlamesPos.y + attackDir.y);

            Instantiate(flamePrefab, SpawnFlamesPos, transform.rotation ,transform.parent);

            spawnIntervalElapsed = 0f;

        }


    }

  

    private IEnumerator stopAttack()
    {
        yield return new WaitForSeconds(stopDur);
        isAttacking = false;
        
        nAttacks--;
        if (nAttacks == 0)
        {
            setVisibleLine(false);
        }
        else {
            setVisibleLine(true);
        }

        stopCoroutine = null;
    }

    public IEnumerator waitToAttack(float dur)
    {



        setVisibleLine(true);
        nAttacks = maxAttacks;

        //-------push player --------
        myCC.enabled = true;
        yield return new WaitForSeconds(0.3f);
        myCC.enabled = false;
        //---------------------------

        yield return new WaitForSeconds((dur/2) - 0.3f);
        StartCoroutine(flickerAim(dur/2));
        yield return new WaitForSeconds(dur/2);

        isAttacking = true;
        setVisibleLine(false);

    }

    private IEnumerator flickerAim(float remainsT)
    {
        float nbFlickers = 3;
        float t = remainsT / (nbFlickers * 2);
        int count = 0;
        bool temp = false;


        while (count < (nbFlickers * 2))
        {
            setVisibleLine(temp);
            yield return new WaitForSeconds(t);

            temp = !temp;
            count++;

        }

        setVisibleLine(temp);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAtk")
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

        }

        if (collision.tag == "Player")
        {
            if (myCC.IsTouching(collision.GetComponent<Collider2D>()))
            {
                PlayerController playerController = collision.GetComponent<PlayerController>();

                if (playerController != null) { 
                    Vector2 knockbackDir = (playerController.transform.position - transform.position).normalized;
                    playerController.startKnockBack(knockbackDir.normalized, 0.5f, 35f, 0.2f, 0.2f);
                }
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

    

    private void OnDestroy()
    {
        dadController.resetAttack();
        dadController.towers.Remove(this);
        StopAllCoroutines();
        Destroy(uiBar);
    }

}
