using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Star_ATK : MonoBehaviour
{
    public SpriteRenderer mySprR;
    
    [Header("Boss Controller")]
    private Enemy_Controller mainController;

    [Header("Bounce Walls")]
    private GameObject grid;
    private GameObject mapPrefab;

    private Vector2 startPos;

    [Header("Berzier Mouvement")]
    private float speed = 1.0f;
    private float t = 0f;
    private Vector3 P0;
    private Vector3 P1;
    private Vector3 P2;

    private bool toSpawn = false;

    [Header("MainStar")]
    private GameObject mainStarPrefab;

    public CameraShake cameraSH;
    // Start is called before the first frame update
    void Start()
    {
        mainController = GetComponent<Enemy_Controller>();
        mySprR = GetComponent<SpriteRenderer>();
        cameraSH = GetComponent<CameraShake>();

        grid = mainController.Grid;

        mainStarPrefab = (GameObject)Resources.Load("Star_ATK_Main_Star", typeof(GameObject));
        mapPrefab = (GameObject)Resources.Load("Star_ATK_Bounce_Walls", typeof(GameObject));

        

        Instantiate(mapPrefab, grid.transform);

        startPos = transform.position;
        StartCoroutine(waitToAttack());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (toSpawn)
        {
            jumpToSpawn();
        }
    }

   private IEnumerator waitToAttack()
    {
        yield return new WaitForSeconds(2f);

        spawnMainStar();

        //transform.gameObject.SetActive(false);
        mySprR.enabled = false;

    }


    private void spawnMainStar()
    {
        GameObject temp = Instantiate(mainStarPrefab, transform.position, transform.rotation, transform.parent);
        Star_ATK_Main_Star_Controller tempController = temp.GetComponent<Star_ATK_Main_Star_Controller>();
        tempController.theBoss = this.gameObject;
        tempController.cameraSH = cameraSH;
        tempController.player = mainController.thePlayer;
    }

    public void startJumpSpawn()
    {
        StartCoroutine(waitToJumpSpawn());
    }

    private IEnumerator waitToJumpSpawn()
    {
        yield return new WaitForSeconds(4f);
        setControlPoints();
        toSpawn = true;
    }

    private void jumpToSpawn()
    {
        //quadratic Berzier exemple
        if (t < 1f)
        {
            transform.position = P1 + Mathf.Pow((1 - t), 2) * (P0 - P1) + Mathf.Pow(t, 2) * (P2 - P1);
            t = t + speed * Time.deltaTime;
        }
        else
        {
            t = 0f;
            toSpawn = false;
            StartCoroutine(waitToAttack());
        }

    }

    private void setControlPoints()
    {
        P0 = transform.position;
        P2 = startPos;
        P1 = new Vector3(P0.x, 11f);
    }
}
