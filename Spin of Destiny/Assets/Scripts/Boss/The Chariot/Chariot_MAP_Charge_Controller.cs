using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chariot_MAP_Charge_Controller : MonoBehaviour
{
    [Header("To init")]
    private Rigidbody2D myRb;



    [Header("Mouvement")]
    public char direction = 'L';
    private Vector2 dir = Vector2.zero;
    private float speed = 30f;


    [Header("Destroy Timer")]
    private float toDestroyElapsedT = 0f;

    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        setDirection();
        
    }

    private void setDirection()
    {
        switch (direction) {
            case 'R':
                dir = Vector2.right;
                transform.eulerAngles = new Vector3(0, 180, 0);
                break;

            case 'L':
                dir = Vector2.left;
                transform.eulerAngles = new Vector3(0, 0, 0);
                break;

            case 'U':
                dir = Vector2.up;
                transform.eulerAngles = new Vector3(0, 0, -90);
                break;

            case 'D':
                dir = Vector2.down;
                transform.eulerAngles = new Vector3(0, 0, 90);
                break;
        }
    }

    private void FixedUpdate()
    {
        myRb.velocity = dir * speed;
        
    }

    // Update is called once per frame
    void Update()
    {
        toDestroyElapsedT += Time.deltaTime;
        if (toDestroyElapsedT > 3f )
        {
            Destroy(this.gameObject);
        }


    }
}
