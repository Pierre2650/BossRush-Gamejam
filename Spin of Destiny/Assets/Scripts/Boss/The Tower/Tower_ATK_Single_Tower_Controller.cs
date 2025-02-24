using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Single_Tower_Controller : MonoBehaviour
{
    [Header("init")]
    private LineRenderer myLR;

    [Header("Aim")]
    public GameObject player;
    private Vector2 lastPlayerPos;
    private Vector2 attackDir;
    private Vector2 SpawnFlamesPos;

    [Header("Attack")]
    private GameObject flamePrefab;
    private float spawnIntervalElapsed = 0f;
    private float spawnIntervalDur = 0.02f;
    private float stopDur = 0.1f;
    private Coroutine stopCoroutine = null;
    private int nAttacks = 1;
    private int maxAttacks = 1;

    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        myLR = GetComponent<LineRenderer>();
        flamePrefab = (GameObject)Resources.Load("TowerAttack", typeof(GameObject));
        StartCoroutine(waitToAttack(1.5f));
        
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

            spawnAttack();

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
        myLR.enabled = visible;

    }


    private void aim()
    {
        setPoints();
           
        lastPlayerPos = player.transform.position;

        //findAttackDir(lastPlayerPos);

        attackDir = (player.transform.position - transform.position).normalized;

        SpawnFlamesPos = new Vector2(transform.position.x + attackDir.x/3, transform.position.y + attackDir.y/3);

    }

    private void spawnAttack()
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

        //StartCoroutine(waitToAttack(1f));

    }

    public IEnumerator waitToAttack(float dur)
    {

        setVisibleLine(true);
        nAttacks = maxAttacks;

        yield return new WaitForSeconds(dur);


        isAttacking = true;
        setVisibleLine(false);

    }






    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(lastSpawnPos, 0.25f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(lastPlayerPos, 0.25f);

    }*/

}
