using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Star_ATK_Main_Star_Controller : MonoBehaviour
{
    [Header("To Init")]
    private Rigidbody2D myRB;
    public GameObject theBoss;
    


    [Header("Mouvement")]
    private Vector2 dir;
    public float speed;

    [Header("Player")]
    public GameObject player;

    [Header("bounce")]
    private int nbBouces = 12;

    [Header("Mini Stars")]
    public GameObject miniPrefab;
    public int nbMini;

    private float spawnMiniElapsed = 0;
    private float spawnInterval = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        miniPrefab = (GameObject)Resources.Load("Star_ATK_Mini_Stars", typeof(GameObject)); 
        dir = (player.transform.position - transform.position).normalized;
        myRB.velocity = dir * speed;
    }

    // Update is called once per frame
    void Update()
    {

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

            


            spawnMiniStars(collision.contacts[0].point);   
        }

       
    }

    private void spawnMiniStars(Vector2 collisionPoint)
    {
        float tempDelay = 0;


        for (int i = 0; i < nbMini; i++)
        {

            GameObject temp = Instantiate(miniPrefab, transform.position, transform.rotation);
            Star_ATK_Mini_Stars tempController = temp.GetComponent<Star_ATK_Mini_Stars>();
            tempController.player = player;
            tempController.starContactPoint = collisionPoint;
            tempController.delay += tempDelay;

            tempDelay += 0.1f;
        }

    }


  

    private IEnumerator waitToStopBounce()
    {
        yield return new WaitForSeconds(2f);

        myRB.velocity = Vector2.zero;
        speed = 20;

        theBoss.transform.position = transform.position;
        theBoss.SetActive(true);
        theBoss.GetComponent<Star_ATK>().startJumpSpawn();

        Destroy(this.gameObject);

        //become slime;
    }


}
