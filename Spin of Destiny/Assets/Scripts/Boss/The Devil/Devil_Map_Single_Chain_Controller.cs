using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil_Map_Single_Chain_Controller : MonoBehaviour
{
    private BoxCollider2D myBXC;

    [Header("Player")]
    public GameObject player = null;

    [Header("Direction")]
    public Vector2 directionToPlayer;
    public Vector2 lookDirPlayer;
    
    public Devil_MAP_Shadow_Controller shadowController;

    private void Awake()
    {

        myBXC = GetComponent<BoxCollider2D>();
        

    }


    private void Start()
    {
        if (myBXC.enabled)
        {

            directionToPlayer = (player.transform.position - transform.position).normalized;
            transform.right = directionToPlayer;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            shadowController.changeChain = true;
            shadowController.pullChain.Remove(this.gameObject);
            Destroy(this.gameObject);


        }

    }


}
