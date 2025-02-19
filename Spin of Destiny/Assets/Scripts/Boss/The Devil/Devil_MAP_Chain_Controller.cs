using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil_MAP_Chain_Controller : MonoBehaviour
{
    public GameObject player;
    public bool aim = true;

    public List<GameObject> chain = new List<GameObject>();

  
    // Update is called once per frame
    void Update()
    {
        if (aim) { 
             transform.right = (player.transform.position - transform.position).normalized;
        }

    }

    public void destroyChain()
    {
        foreach (GameObject c in chain)
        {
            Destroy(c);
        }

        chain.Clear();
    }
}
