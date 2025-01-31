using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class MeshDevilAtkZone : MonoBehaviour
{
    public MeshRenderer myMshR;
    public Devil controller;

    [Header("Paramters")]
    private float radius = 4f;
    private int n = 100;
    public Vector2 Dir = Vector2.zero;
    

    [Header("Real Scrythe")]
    public GameObject child;


    // Start is called before the first frame update
    void Start()
    {
        myMshR = GetComponent<MeshRenderer>();
        myMshR.enabled = false;
        generateAttackZone();

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

    private void OnDrawGizmos()
    {
        Vector2 temp = new Vector2(transform.position.x, transform.position.y + radius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(temp, 0.2f);



    }
}
