using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Star_ATK_Mini_Stars : MonoBehaviour
{
    private ParticleSystem myPrtSys;

    [Header("The player")]
    public GameObject player;


    // cuadratic berzier  for parabolic mouvement
    [Header("Berzier Mouv")]
    private float t = 0f;
    private Vector3 P0;
    private Vector3 P1;
    private Vector3 P2;

    private float mouvSpeed;

    private bool toPlayer = false;

    public float delay = 0f;


    [Header("Start Mouvement")]
    private Vector3 startMouvStart;
    private Vector3 startMouvEnd;

    private float startMouvElapsed = 0;
    private float startMouvDur = 0.5f;

    public AnimationCurve curve;

    [Header("Parent")]
    public Vector2 starContactPoint;

    [Header("child Mark")]
    public GameObject dirMark;

    [Header("child particles")]
    public GameObject particles;
    private Star_ATK_Mini_Particles_Controller  particlesController;
    
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        myPrtSys = GetComponent<ParticleSystem>();
        particlesController = particles.GetComponent<Star_ATK_Mini_Particles_Controller>();
        

        generateStartMouvDir();
        StartCoroutine(startMouvement());
        StartCoroutine(waitToATK());

    }

    // Update is called once per frame
    void Update()
    {

        if (toPlayer)
        {

            dirMark.transform.position = P2;

            attack();

        }
    }

    private void attack()
    {
        if (t < 1f)
        {
            Vector3 nextDir = P1 + Mathf.Pow((1 - t), 2) * (P0 - P1) + Mathf.Pow(t, 2) * (P2 - P1);
            particlesController.setRotation(nextDir);

            transform.position = nextDir;

            t = t + mouvSpeed * Time.deltaTime;

        }
        else
        {
            toPlayer = false;

            t = 0;

            //this.gameObject.SetActive(false);
            Destroy(this.gameObject);

        }
    }


    private void setControlPoints()
    {
        //1. set,\ P2. P1
        P0 = transform.position;
        P0.z = 0;

        P2 = player.transform.position;

        int distance = 5;

        float distChoice = Vector2.Distance(P0, P2);
        
        
        if (distChoice <= 11)
        {
            distance = 5;
            mouvSpeed = 2.5f;
        }
        else if( distChoice > 11 && distChoice <= 15)
        {
            distance = 10;

            mouvSpeed = 2f;

        }
        else{
            distance = 15;
            mouvSpeed = 1.7f;
        }

        setP1(distance);

       

    }

    private void setP1( int distance)
    {
        int i = Random.Range(0, 2);

        if (P2.x > P0.x && P2.y > P0.y)
        {
            // Debug.Log("cross hair on X Positive, Y Positive");

            if (i == 1)
            {
                P1 = new Vector3(P0.x + distance, P0.y);
            }
            else
            {
                P1 = new Vector3(P0.x, P0.y + distance);
            }

        }
        else if (P2.x < P0.x && P2.y > P0.y)
        {
            // Debug.Log("cross hair on X Negative, Y Positive");

            if (i == 1)
            {
                P1 = new Vector3(P0.x, P0.y + distance);
            }
            else
            {

                P1 = new Vector3(P0.x - distance, P0.y);
            }
        }
        else if (P2.x < P0.x && P2.y < P0.y)
        {
            if (i == 1)
            {
                P1 = new Vector3(P0.x - distance, P0.y);
            }
            else
            {
                P1 = new Vector3(P0.x, P0.y - distance);
            }
        }
        else if (P2.x > P0.x && P2.y < P0.y)
        {
            //Debug.Log("cross hair on X Positive, Y Negative");

            if (i == 1)
            {
                P1 = new Vector3(P0.x, P0.y - distance);
            }
            else
            {
                P1 = new Vector3(P0.x + distance, P0.y);
            }

        }


    }


    public void generateStartMouvDir()
    {
        float rand;
        Vector2 temp;
        int i=0;

        do {
            rand = Random.Range(0.05f, (Mathf.PI * 2));

            temp = new Vector2(Mathf.Cos(rand), Mathf.Sin(rand));

            temp = temp * 4;

            startMouvEnd = new Vector3(transform.position.x + temp.x, transform.position.y + temp.y);

            i++;

            if (i > 20)
            {
                Debug.Log("Limit Reached");
                break;

            }

        } while (Vector2.Distance(starContactPoint, startMouvEnd) < 3.5f);
        
        

    }


    private void goToPlayer()
    {
        setControlPoints();
        dirMark.transform.position = P2;
        dirMark.SetActive(true);


        particles.transform.position = new Vector3(particles.transform.position.x, particles.transform.position.y, 0);
        myPrtSys.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);

        toPlayer = true;
    }

    private IEnumerator startMouvement()
    {

        float percentageDur = 0;

        Vector2 start = transform.position;
        Vector2 end = startMouvEnd;



        while (startMouvElapsed < startMouvDur)
        {

            percentageDur = startMouvElapsed / startMouvDur;

            transform.position = Vector2.Lerp(start, end, curve.Evaluate(percentageDur));

            startMouvElapsed += Time.deltaTime;
            yield return null;

        }

        startMouvElapsed = 0;


    }

    private IEnumerator waitToATK()
    {
        yield return new WaitForSeconds(2 + delay);

        

        goToPlayer();

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(P0, 0.25f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(P1, 0.25f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(P2, 0.25f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="Player"){
            Health playerHealth = other.GetComponent<Health>();
            if(!playerHealth.isInvincible){
                playerHealth.takeDamage(damage);
            }
            

        }
    }

}



