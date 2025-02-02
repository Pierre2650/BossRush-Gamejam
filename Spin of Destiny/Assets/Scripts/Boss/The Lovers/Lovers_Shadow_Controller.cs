using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lovers_Shadow_Controller : MonoBehaviour
{
    [Header("init")]
    private LineRenderer myLR;

    [Header("Aim")]
    private GameObject player;
    private Vector2 lastPlayerPos;
    private Vector2 attackDir;
    private Vector2 lastSpawnPos;

    [Header("Attack")]
    private GameObject flamePrefab;
    private float spawnIntervalElapsed = 0f;
    private float spawnIntervalDur = 0.02f;
    private float stopDur = 0.1f;
    private Coroutine stopCoroutine = null;
    private int nAttacks = 2;
    private int maxAttacks = 2;

    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        myLR = GetComponent<LineRenderer>();
        player = GameObject.Find("Player");
        flamePrefab = (GameObject)Resources.Load("ShadowAttack", typeof(GameObject));
        StartCoroutine(waitToAttack(1.5f));
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
           isAttacking = true;
           setVisibleLine(false);
        }




        if (isAttacking)
        {
            //check is arrived at last position
            if (Vector2.Distance(lastSpawnPos, lastPlayerPos) <= 1f)
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

        findAttackDir(lastPlayerPos);

        lastSpawnPos = new Vector2(transform.position.x + attackDir.x, transform.position.y + attackDir.y);

    }

    private void spawnAttack()
    {

        spawnIntervalElapsed += Time.deltaTime;
        if (spawnIntervalElapsed > spawnIntervalDur) {

            lastSpawnPos = new Vector2(lastSpawnPos.x + attackDir.x, lastSpawnPos.y + attackDir.y);

            Instantiate(flamePrefab, lastSpawnPos, transform.rotation ,transform.parent);

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

        StartCoroutine(waitToAttack(1f));

    }

    private IEnumerator waitToAttack(float dur)
    {
        

        if (nAttacks == 0)
        {
            yield return new WaitForSeconds(dur+1.5f);
            setVisibleLine(true);
            nAttacks = maxAttacks;
            yield return new WaitForSeconds(1.5f);
        }
        else
        {
            yield return new WaitForSeconds(dur);

        }

        

        isAttacking = true;
        setVisibleLine(false);

    }





    private void findAttackDir(Vector2 target)
    {
        /*calculate the nearest direction to the Boss,
         * 1. take current Player position
         * 2. add a cos sin  vector from the possible direction to the player position
         * 3. compare the distance from this new vector to the Boss , to the distance from the current "nearest" cos sin  vector to the Boss
         */



        Vector2 posToTest, currentNpos;

        //Optimized Version 
        //  divide pi cirlcle on 4 
        //  find where section we are
        //  only find the neares position in this section

        float[] startEnd = findPISection(target);


        for (float i = startEnd[0]; i < startEnd[1]; i = i + 0.05f)
        {
            posToTest = new Vector2(transform.position.x + Mathf.Cos(i), transform.position.y + Mathf.Sin(i));
            currentNpos = new Vector2(transform.position.x + attackDir.x, transform.position.y + attackDir.y);


            if (Vector2.Distance(posToTest, target) < Vector2.Distance(currentNpos, target))
            {
                attackDir = new Vector2(Mathf.Cos(i), Mathf.Sin(i));
            }
        }





    }

    private float[] findPISection(Vector2 target)
    {
        float[] startEnd = new float[2];


        if (target.x > transform.position.x && target.y > transform.position.y)
        {
            // Debug.Log("cross hair on X Positive, Y Positive");

            startEnd[0] = 0;
            startEnd[1] = (Mathf.PI / 2);

        }
        else if (target.x < transform.position.x && target.y > transform.position.y)
        {
            // Debug.Log("cross hair on X Negative, Y Positive");

            startEnd[0] = (Mathf.PI / 2);
            startEnd[1] = (Mathf.PI);
        }
        else if (target.x < transform.position.x && target.y < transform.position.y)
        {
            // Debug.Log("cross hair on X Negative, Y Negative");
            startEnd[0] = (Mathf.PI);
            startEnd[1] = ((3 * Mathf.PI) / 2);
        }
        else if (target.x > transform.position.x && target.y < transform.position.y)
        {
            //Debug.Log("cross hair on X Positive, Y Negative");

            startEnd[0] = ((3 * Mathf.PI) / 2);
            startEnd[1] = (Mathf.PI * 2);

        }
        else
        {
            Debug.Log("Not in PI cercle?");

            startEnd[0] = 0;
            startEnd[1] = (Mathf.PI * 2);

        }

        return startEnd;

    }


    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(lastSpawnPos, 0.25f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(lastPlayerPos, 0.25f);

    }*/

}
