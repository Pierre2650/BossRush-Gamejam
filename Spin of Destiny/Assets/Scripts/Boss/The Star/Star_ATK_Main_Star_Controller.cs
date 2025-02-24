using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Star_ATK_Main_Star_Controller : MonoBehaviour
{
    [Header("To Init")]
    private Rigidbody2D myRB;
    public GameObject theBoss;
    private LineRenderer myLR;
    private SpriteRenderer mySprR;


    [Header("aim")]
    private bool aim = true;

    [Header("Mouvement")]
    private Vector2 dir;
    private float speed = 20;

    [Header("Player")]
    public GameObject player;

    [Header("bounce")]
    private int nbBouces = 12;

    [Header("Mini Stars")]
    public GameObject miniPrefab;
    private int nbMini = 6;

    private float spawnMiniElapsed = 0;
    private float spawnInterval = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        myLR = GetComponent<LineRenderer>();
        mySprR = GetComponent<SpriteRenderer>();
        miniPrefab = (GameObject)Resources.Load("Star_ATK_Mini_Stars", typeof(GameObject)); 
        
        StartCoroutine(waitToStart());

       
    }

    private void Update()
    {
        if (aim)
        {
            setPoints();

        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        nbBouces--;

        if(nbBouces <= 0)
        {
            StartCoroutine(waitToStopBounce());
            
        }


        if (nbBouces % 3 == 0)
        {
            speed += 2;

            dir = (player.transform.position - transform.position).normalized;
            myRB.velocity = dir * speed;



            if (nbBouces > 0) { 
                spawnMiniStars(collision.contacts[0].point);
            }   
        }

       
    }


    private IEnumerator waitToStart()
    {
        spawnMiniStars(Vector2.zero);
        yield return new WaitForSeconds(3f);

        myLR.enabled = true;

        float duration = 2;

        yield return new WaitForSeconds(duration / 5);
        myLR.enabled = false;

        yield return new WaitForSeconds(duration / 5);
        myLR.enabled = true;

        yield return new WaitForSeconds(duration / 5);
        myLR.enabled = false;

        yield return new WaitForSeconds(duration / 5);
        myLR.enabled = true;

        yield return new WaitForSeconds(duration / 5);
        myLR.enabled = false;

        dir = (player.transform.position - transform.position).normalized;
        myRB.velocity = dir * speed;

    }

    private void spawnMiniStars(Vector2 collisionPoint)
    {
        float tempDelay = 0;


        for (int i = 0; i < nbMini; i++)
        {

            GameObject temp = Instantiate(miniPrefab, transform.position, transform.rotation, transform.parent);
            Star_ATK_Mini_Stars tempController = temp.GetComponent<Star_ATK_Mini_Stars>();
            tempController.player = player;
            tempController.starContactPoint = collisionPoint;
            tempController.delay += tempDelay;

            tempDelay += 0.1f;
        }

    }




    private IEnumerator waitToStopBounce()
    {
        //OverallTime 2.5

        yield return new WaitForSeconds(1.5f);

        float duration = 0.1f;

        mySprR.enabled = false;
        yield return new WaitForSeconds(duration);

        mySprR.enabled = true;
        yield return new WaitForSeconds(duration );


        mySprR.enabled = false;
        yield return new WaitForSeconds(duration );

        mySprR.enabled = true;
        yield return new WaitForSeconds(duration );


        mySprR.enabled = false;
        yield return new WaitForSeconds(duration);

        mySprR.enabled = true;
        yield return new WaitForSeconds(1f);

        stopBounce(); 
    }


    private void stopBounce()
    {
        myRB.velocity = Vector2.zero;
        speed = 20;

        theBoss.transform.position = transform.position;
        theBoss.SetActive(true);
        theBoss.GetComponent<Star_ATK>().startJumpSpawn();

        Destroy(this.gameObject);

        //become slime;

    }

    private void setPoints()
    {
        Vector2 aimDir = (player.transform.position - transform.position).normalized;
        aimDir *= 4f;

        Vector2 temp = new Vector2(transform.position.x + aimDir.x, transform.position.y + aimDir.y) ;

        myLR.SetPosition(0, transform.position);
        myLR.SetPosition(1, temp );

    }


}
