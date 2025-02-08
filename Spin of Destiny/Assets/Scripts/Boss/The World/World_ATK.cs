using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World_ATK : MonoBehaviour
{

    private Vector2 startPos;

    [Header("Berzier Mouvement")]
    private float speed = 1.0f;
    private float t = 0f;
    private Vector3 P0;
    private Vector3 P1;
    private Vector3 P2;
    private float maxD = 7f;


    [Header("Jump")]
    private bool goJump = false;
    private int nJumps = 1;


    [Header("The Player")]
    public GameObject player;

    [Header("Boss Controller")]
    private Enemy_Controller mainController;


    [Header("ATK")]
    private bool isAttacking = true;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        mainController = GetComponent<Enemy_Controller>();
        player = mainController.thePlayer;

        stateMachine();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) > maxD && isAttacking)
        {
            P2 = player.transform.position;
        }


        if (goJump)
        {
            jump();

        }



    }

    private void jump()
    {
        //quadratic Berzier exemple
        if (t < 1f)
        {
            transform.position = P1 + Mathf.Pow((1 - t), 2) * (P0 - P1) + Mathf.Pow(t, 2) * (P2 - P1);

            t = t + speed * Time.deltaTime;

        }
        else
        {
            goJump = false;
            t = 0f;

            if (isAttacking)
            {
                nJumps--;
                //showAttack();
            }
            else
            {
                StartCoroutine(waitOnSpawn());

            }
        }

    }

    private void stateMachine()
    {
        if (nJumps == 0)
        {
            isAttacking = false;

        }

        StartCoroutine(waitToJump());
    }
    private IEnumerator waitToJump()
    {
        yield return new WaitForSeconds(0.5f);

        P0 = transform.position;
        P1 = new Vector3(P2.x, 12f);

        if (!isAttacking)
        {
            P2 = startPos;
        }


        goJump = true;
    }

    private IEnumerator waitOnSpawn()
    {
        yield return new WaitForSeconds(2f);
        isAttacking = true;
        nJumps = 1;
        stateMachine();
    }
}
