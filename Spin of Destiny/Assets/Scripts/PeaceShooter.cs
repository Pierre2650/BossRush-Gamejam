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


    [Header("Projectile")]
    public GameObject bulletPrefab;



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
            spawnBullet();
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

    private  void getDirNearestCrossHair()
    {
        /*calculate the nearest direction to the crosshair,
         * 1. take current pistol position
         * 2. add a cos sin  vector from the possible direction to the pistol position
         * 3. compare the distance from this new vector to the crosshair , to the distance from the current "nearest" cos sin  vector to the crosshair
         */



        Vector2 posToTest, currentNpos;

        //Optimized Version 
        //  divide pi cirlcle on 4 
        //  find where section we are
        //  only find the neares position in this section

        float[] startEnd = findPISection();


        for (float i = startEnd[0]; i < startEnd[1]; i = i + 0.01f)
        {
            posToTest = new Vector2(transform.position.x + Mathf.Cos(i), transform.position.y + Mathf.Sin(i));
            currentNpos = new Vector2(transform.position.x + NearDirToCross.x, transform.position.y + NearDirToCross.y);


            if (Vector2.Distance(posToTest, chTransform.transform.position) < Vector2.Distance(currentNpos, chTransform.transform.position))
            {
                NearDirToCross = new Vector2(Mathf.Cos(i), Mathf.Sin(i));
            }
        }





    }

    private float[] findPISection()
    {
        float[] startEnd = new float[2];


        if (chTransform.transform.position.x > transform.position.x && chTransform.transform.position.y > transform.position.y)
        {
           // Debug.Log("cross hair on X Positive, Y Positive");

            startEnd[0] = 0;
            startEnd[1] = (Mathf.PI/2);

        }
        else if (chTransform.transform.position.x < transform.position.x && chTransform.transform.position.y > transform.position.y)
        {
           // Debug.Log("cross hair on X Negative, Y Positive");

            startEnd[0] = (Mathf.PI / 2);
            startEnd[1] = (Mathf.PI);
        }
        else if (chTransform.transform.position.x < transform.position.x && chTransform.transform.position.y < transform.position.y)
        {
           // Debug.Log("cross hair on X Negative, Y Negative");
            startEnd[0] = (Mathf.PI);
            startEnd[1] = ((3 * Mathf.PI) / 2);
        }
        else if (chTransform.transform.position.x > transform.position.x && chTransform.transform.position.y < transform.position.y)
        {
           //Debug.Log("cross hair on X Positive, Y Negative");

            startEnd[0] = ((3 * Mathf.PI) / 2);
            startEnd[1] = (Mathf.PI * 2);

        }
        else
        {
           // Debug.Log("Not in PI cercle?");

            startEnd[0] = 0;
            startEnd[1] = (Mathf.PI * 2);

        }

        return startEnd;

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


    private void spawnBullet()
    {
        Vector2 spawnPos = new Vector2(transform.position.x + NearDirToCross.x / 3,  transform.position.y + NearDirToCross.y / 3f);
        Quaternion spawnRot = transform.rotation;


        GameObject temp = Instantiate(bulletPrefab, spawnPos, spawnRot , transform.parent);
        Bullet_Controller temp2 = temp.GetComponent<Bullet_Controller>();
        temp2.direction = NearDirToCross;
    }


    public Vector2 getNearDirToCross()
    {
        return NearDirToCross;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x + NearDirToCross.x, transform.position.y + NearDirToCross.y), 0.2f);

        //Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(new Vector2(transform.position.x + NearDirToCross.x/3, transform.position.y + NearDirToCross.y/3f), 0.2f);


    }
}
