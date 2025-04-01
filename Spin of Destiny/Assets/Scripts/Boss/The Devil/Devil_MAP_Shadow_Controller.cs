using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil_MAP_Shadow_Controller : MonoBehaviour
{
    //Debug to do : when the player is close 

    [Header("The Player")]
    public GameObject player;
    private Rigidbody2D plRB;

    public bool chainPlayer = false;
    private GameObject chainPrefab;

    [Header("Pull Chains")]
    private Vector2 spawnDir = Vector2.zero;

    private Vector2 FirstChainPos;
    private Vector2 SpawnChainPos;

    private float spawnIntervalElapsed = 0;
    private float spawnIntervalDur = 0.02f;

    public List<GameObject> pullChain = new List<GameObject>();
    private int index;

    [Header("Pull Player")]
    private bool attractPlayer = false;
    public bool changeChain = false;

    private Vector2 dirToShadow;
    private float speed = 20;

    [Header("Hinged join Chains")]
    private GameObject chainParent;
    private Devil_MAP_Chain_Controller chainHJController;

    // Start is called before the first frame update
    void Start()
    {
        plRB = player.GetComponent<Rigidbody2D>();
        chainPrefab = (GameObject)Resources.Load("Devil_MAP_Single_Chain_Prefab", typeof(GameObject)); 
        chainParent = transform.GetChild(0).gameObject;
        chainHJController = chainParent.GetComponent<Devil_MAP_Chain_Controller>();
        chainHJController.player = player;
    }

    // Update is called once per frame
    void Update()
    {
        spawnDir = (player.transform.position - new Vector3(transform.position.x, transform.position.y - 0.46f)).normalized;

        if (chainPlayer)
        {
            //check is arrived at last position
            if (Vector2.Distance(SpawnChainPos, player.transform.position) < 0.5f )
            {
                index = pullChain.Count - 1;
                Debug.Log("index inside update chainPLayer = "+ index);

                if (index > 0)
                {
                    dirToShadow = pullChain[index].GetComponent<Devil_Map_Single_Chain_Controller>().directionToPlayer * -1;
                }

                //stop Player
                //player.GetComponent<PlayerController>().restrainMouvement();

                attractPlayer = true;

                chainPlayer = false;

            }
            
                spawnPullChain();
           

        } else if (attractPlayer) {


            //check is arrived at last position
            if (Vector2.Distance(player.transform.position, transform.position) < 6.5f) {

                //index = pullChain.Count - 1;

                Debug.Log("index inside update attractPLayer = " + index);

                //free Player
                player.GetComponent<Player_controller>().freeMouvement();

                Debug.Log("Attrac PLayer = false");
                attractPlayer = false;


                // generate hinged joint chain
                spawnChain();

                

            }



            pullPlayer();

            

        }
        else {
            aim();
        }

    }

    private void aim()
    {

        SpawnChainPos = new Vector2(transform.position.x + spawnDir.x / 3, (transform.position.y - 0.46f) + spawnDir.y / 3);

    }


    private void spawnPullChain()
    {

        spawnIntervalElapsed += Time.deltaTime;
        if (spawnIntervalElapsed > spawnIntervalDur)
        {

            GameObject temp = Instantiate(chainPrefab, SpawnChainPos, transform.rotation, transform.parent);
            temp.GetComponent<BoxCollider2D>().enabled = true;

            pullChain.Add(temp);


            Devil_Map_Single_Chain_Controller tempController = temp.GetComponent<Devil_Map_Single_Chain_Controller>();
            tempController.shadowController = this;
            tempController.player = player;
            


            Vector2 newDir =  (player.transform.position - new Vector3( SpawnChainPos.x, SpawnChainPos.y) ).normalized;
            tempController.directionToPlayer = newDir;
            SpawnChainPos = new Vector2(SpawnChainPos.x + newDir.x, SpawnChainPos.y + newDir.y);

            spawnIntervalElapsed = 0f;

        }

    }

    private void spawnChain()
    {
        int i;

        aim();
        Rigidbody2D rigidbody = chainParent.GetComponentInChildren<Rigidbody2D>();

        //set rigidbody(first chain has to have ancho body

        GameObject temp = Instantiate(chainPrefab, SpawnChainPos, chainParent.transform.rotation, chainParent.transform);
        chainHJController.chain.Add(temp);
        Devil_Map_Single_Chain_Controller tempController = temp.GetComponent<Devil_Map_Single_Chain_Controller>();
        tempController.player = player;

        HingeJoint2D tempHJ = temp.GetComponent<HingeJoint2D>();
        tempHJ.enabled = true;
        tempHJ.connectedBody = rigidbody;

        rigidbody = temp.GetComponent<Rigidbody2D>();
        SpawnChainPos = new Vector2(SpawnChainPos.x + spawnDir.x, SpawnChainPos.y + spawnDir.y);

        int size = pullChain.Count;
        Debug.Log(size);
        for ( i = 1; i < size ; i++) { 

            //new hinjed join chain prefab
            /*
             * 1.instantiate prefab on chain parent
             * 2. get  hinjed join component 
               3. set connected RB
               4. update rigidbody, spawnchainpos
             */

            temp = Instantiate(chainPrefab, SpawnChainPos, chainParent.transform.rotation, chainParent.transform);
            chainHJController.chain.Add(temp);
            tempController = temp.GetComponent<Devil_Map_Single_Chain_Controller>();
            tempController.player = player;

            tempHJ = temp.GetComponent<HingeJoint2D>();
            tempHJ.enabled = true;
            tempHJ.connectedBody  = rigidbody;

            rigidbody = temp.GetComponent<Rigidbody2D>();
            SpawnChainPos = new Vector2(SpawnChainPos.x + spawnDir.x, SpawnChainPos.y + spawnDir.y);

        }

        //3. set rigid body of last spawned chain  to player 
        player.GetComponent<HingeJoint2D>().connectedBody = rigidbody;
        player.GetComponent<HingeJoint2D>().enabled = true;

        chainHJController.aim = false;
        destroyPullChain();
    }


    private void pullPlayer()
    {
        // pull until limit
        //on limit destroy chain
        //change dir

        if (changeChain)
        {
            index--;
            Debug.Log("index inside pullPLayer() = " + index);


            Devil_Map_Single_Chain_Controller tempController = pullChain[index].GetComponent<Devil_Map_Single_Chain_Controller>();

            dirToShadow = tempController.directionToPlayer * -1;

            changeChain = false;

            

        }

        plRB.linearVelocity = dirToShadow * speed;


    }

    private void destroyPullChain()
    {
        foreach (GameObject c in pullChain) {
            Destroy(c);
        }

        pullChain.Clear();
    }

    public void destroyChain()
    {
        chainHJController.destroyChain();
    }
}
