using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{

    public int Health = 10;



    public void hurt(int amount)
    {
        Health -= amount;
    }


    public void reStartPos()
    {
        this.transform.position = new Vector2(-2.46f, 6.67f);
    }
    
}
