using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldOrb : MonoBehaviour
{
    private GameObject turnbased;
    private Turn_Controller Turn_Controller;
    
    // Start is called before the first frame update
    void Start()
    {
        turnbased = GameObject.Find("TurnBase_Manager");
        Turn_Controller = turnbased.GetComponent<Turn_Controller>();
    }

    // Update is called once per frame
    void Update()
    {

        switch (Turn_Controller.turnCount)
        {

            case 1:
                this.transform.localScale = new Vector2(2, 2);
                break;

            case 2:
                this.transform.localScale = new Vector2(3, 3);
                break;
        }


        
    }
}
