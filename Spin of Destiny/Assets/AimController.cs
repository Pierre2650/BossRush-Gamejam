using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AimController : MonoBehaviour
{
    private Vector3 mousePos;

    private Vector3 lastPosition = Vector3.zero;
    public bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        this.transform.position = new Vector2( mousePos.x,mousePos.y);

       /* if (lastPosition != transform.position)
        {
            isMoving = true;
            lastPosition = transform.position;

        }
        else { 
            isMoving = false;
        }*/
        
    }
}
