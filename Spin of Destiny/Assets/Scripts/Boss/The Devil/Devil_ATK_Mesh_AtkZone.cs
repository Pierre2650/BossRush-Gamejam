
using System.Collections.Generic;
using UnityEngine;

public class Devil_ATK_Mesh_AtkZone : MonoBehaviour
{
    public MeshRenderer myMshR;
    public Devil_ATK controller;

    public PolygonCollider2D myPlC;

    private CameraShake cameraShake;
   

    [Header("Paramters")]
    private float radius = 4f;
    private int n = 100;
    public Vector2 Dir = Vector2.zero;
    

    [Header("Real Scrythe")]
    public GameObject child;


    public float damage;


    // Start is called before the first frame update
    void Start()
    {
        myMshR = GetComponent<MeshRenderer>();
        myPlC = GetComponent<PolygonCollider2D>();
        myMshR.enabled = false;
        generateAttackZone();
        cameraShake = transform.parent.GetComponent<CameraShake>();
        child.GetComponent<Devil_ATK_Scythe_Controller>().setCamera(cameraShake);

    }


    private void generateAttackZone()
    {
        Mesh mesh = new Mesh();
        //verticies
        List<Vector3> verticiesList = new List<Vector3> { };
        float x;
        float y;
        for (int i = 0; i < n; i++)
        {
            x = radius * Mathf.Sin((Mathf.PI * i) / n);
            y = radius * Mathf.Cos((Mathf.PI * i) / n);
            verticiesList.Add(new Vector3(x, y, 0f));
        }
        Vector3[] verticies = verticiesList.ToArray();

        //triangles
        List<int> trianglesList = new List<int> { };
        for (int i = 0; i < (n - 2); i++)
        {
            trianglesList.Add(0);
            trianglesList.Add(i + 1);
            trianglesList.Add(i + 2);
        }
        int[] triangles = trianglesList.ToArray();

        mesh.vertices = verticies;

        mesh.triangles = triangles;


        GetComponent<MeshFilter>().mesh = mesh;


    }
    public void aimAttackZoneAtPlayer()
    {
        float angleZ = 0f;

        //Get information needed
        //  The Targed vector , the Gun vector

        Vector3 playerPos = new Vector3(0, Mathf.Abs(transform.position.y), 0);

        //Angle Calculation
        angleZ = Vector3.Angle(Dir, playerPos);

         //Calibration
        angleZ -= 90;
        angleZ *= -1;


        float angleY = 0f;

        if (transform.position.x < controller.player.transform.position.x)
        {
            angleY = 0f;

        }

        if (transform.position.x > controller.player.transform.position.x)
        {

            angleY = 180f;

        }


        transform.rotation = Quaternion.Euler(0, angleY , angleZ);


    }

    public void showScythe()
    {
        child.SetActive(true);

    }

    public void attackEnded()
    {
        child.SetActive(false);
        controller.stateMachine();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Health playerHealth = other.GetComponent<Health>();
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (!playerHealth.isInvincible)
            {
                playerHealth.takeDamage(damage);
                playerController.isHit();
            }

            myPlC.enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 temp = new Vector2(transform.position.x, transform.position.y + radius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(temp, 0.2f);



    }
}
