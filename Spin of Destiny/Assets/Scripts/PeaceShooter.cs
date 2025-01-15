using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PeaceShooter : MonoBehaviour
{
    private SpriteRenderer mySpR;


    public GameObject crossHair;
    private Transform chTransform;
    private AimController chConrtoller;
    private Vector2 NearDirToCross = Vector2.zero;


    private bool atkOnColdown = false;

    [Header("Animations")]
    public Sprite[] gunSprites;
    private int currentIndexSprite = 0;
    public float gunShootAnimSpeed = 1.0f;



    // Start is called before the first frame update
    void Start()
    {
        chTransform = crossHair.GetComponent<Transform>();
        chConrtoller = crossHair.GetComponent<AimController>();
        mySpR = GetComponent<SpriteRenderer>();
        


    }

    // Update is called once per frame
    void Update()
    {
        
       followCross();

        if (Input.GetMouseButton(0) && !atkOnColdown)
        {
            StartCoroutine(gunShootAnim());
        }



    }


    private void followCross()
    {
        float angleZ = 0f;

        //Get information needed
        //  The Targed vector , the Gun vector

        getDirNearestCrossHair();
        Vector3 playerPos = new Vector3(0, Mathf.Abs(transform.position.y), 0);

        //Angle Calculation
        angleZ = Vector3.Angle(NearDirToCross, playerPos);

        //Calibration
        angleZ -= 90;
        angleZ *= -1;


        float angleY = 0f;

        if (transform.position.x < chTransform.position.x)
        {
            angleY = 0f;

        }

        if (transform.position.x > chTransform.position.x)
        {

            angleY = 180f;

        }


        transform.rotation = Quaternion.Euler(0, angleY, angleZ);





    }

    protected virtual void getDirNearestCrossHair()
    {
        /*calculate the nearest direction to the player,
         * 1. take current enemy pos
         * 2. add a unit vector from the possible direction to the enemy pos 
         * 3. compare the distance from this new vector to the player to the distance from the current "nearest" unit vector to the player
         */



        Vector2 posToTest, currentNpos;


        for (float i = 0; i < (Mathf.PI * 2); i = i + 0.1f)
        {
            posToTest = new Vector2(transform.position.x + Mathf.Cos(i) , transform.position.y + Mathf.Sin(i));
            currentNpos = new Vector2(transform.position.x + NearDirToCross.x, transform.position.y + NearDirToCross.y);


            if (Vector2.Distance(posToTest, chTransform.transform.position) < Vector2.Distance(currentNpos, chTransform.transform.position))
            {
                NearDirToCross = new Vector2(Mathf.Cos(i), Mathf.Sin(i));
            }
        }




    }

    private IEnumerator gunShootAnim()
    {
        atkOnColdown = true;
        bool end = false;

        yield return new WaitForSeconds(gunShootAnimSpeed);

        if (currentIndexSprite > 9)
        {

            end = true;


        }


        mySpR.sprite = gunSprites[currentIndexSprite];
        currentIndexSprite += 1;

        if (!end)
        {
            StartCoroutine(gunShootAnim());

        }
        else {
            currentIndexSprite = 0;
            atkOnColdown = false ;
        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x + NearDirToCross.x, transform.position.y + NearDirToCross.y), 0.2f);

        
    }
}
