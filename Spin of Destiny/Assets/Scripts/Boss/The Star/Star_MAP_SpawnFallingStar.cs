using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFallingStar : MonoBehaviour
{
    
    public GameObject star;
    private Falling_Star_Controller star_Controller;
    private Vector2 starSpawnPos;


    

    // Start is called before the first frame update
    void Start()
    {
        star_Controller = star.GetComponent<Falling_Star_Controller>();
        starSpawnPos = new Vector2(transform.position.x, 12 );

        star.transform.position = starSpawnPos;
        spawnStar();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //star.transform.position = starSpawnPos;
            //spawnStar();

        }


        if (star_Controller.fallEnd)
        {
            Destroy(this.gameObject);
        }
    }

    private void spawnStar()
    {
        star_Controller.makeItFall();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(starSpawnPos, 0.25f);
    }
}
