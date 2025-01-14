using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    public GameObject Actions;
    public GameObject AttackUI;

    private GameObject currentUI;


    // Start is called before the first frame update
    void Start()
    {
        currentUI = Actions;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            goBack();

        }
        
    }


    public void chooseAttack()
    {
        currentUI = AttackUI;

        AttackUI.SetActive(true);

        Actions.SetActive(false);
    }

    public void goBack()
    {
        currentUI.SetActive(false);

        Actions.SetActive(true);
    }


}
