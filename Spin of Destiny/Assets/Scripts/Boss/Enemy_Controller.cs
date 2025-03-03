using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{

    // à enlever 
    public int Health = 100;
    public GameObject thePlayer;
    public GameObject Grid;
    public void hurt(int amount)
    {
        Health -= amount;
    }


    public void reStartPos()
    {
        this.transform.position = new Vector2(-2.46f, 6.67f);
    }
    
}
