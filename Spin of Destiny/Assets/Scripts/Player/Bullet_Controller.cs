using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Controller : MonoBehaviour
{
    private Rigidbody2D myRb;
    private float speed = 30;
    public Vector2 direction;



    [Header("Destroy Timer")]
    private float elapsedT = 0;
    private float duration = 1.5f;


    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        
    }

    private void Update()
    {
        elapsedT += Time.deltaTime;
        if (elapsedT > duration) {
            Destroy(this.gameObject);
        }
    }


    private void FixedUpdate()
    {
        myRb.linearVelocity = direction * speed;
        
    }

}
