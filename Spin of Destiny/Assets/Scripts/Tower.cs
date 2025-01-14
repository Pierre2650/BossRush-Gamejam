using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Tarot
{
    private GameObject Grid;
    private GameObject newMap;

    // Start is called before the first frame update
    void Start()
    {
        Grid = GameObject.Find("Grid");
        newMap =  (GameObject)Resources.Load("TowerMap", typeof(GameObject));
        GameObject temp = Instantiate(newMap);
        temp.transform.parent = Grid.transform;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
