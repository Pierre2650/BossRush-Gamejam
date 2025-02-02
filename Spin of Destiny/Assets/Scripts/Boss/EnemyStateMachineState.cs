using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn_Controller : MonoBehaviour
{
    public bool isPlayersTurn = true;

    public int turnCount = 0;


    public float bossTurnTimer = 0.0f;
    public float bossTurnDuration = 5f;


    public Boss_Creation_Controller BSController;


    [Header("Player")]
    public GameObject UI;
    public Player_controller plController;

    [Header("Boss")]
    private GameObject idk;
    // make something so you can enable and unable  tarot scripts
    public Enemy_Controller bossController;
    // Update is called once per frame
    void Update()
    {
        if (!isPlayersTurn)
        {
            bossTurnTimer += Time.deltaTime;

            if (bossTurnTimer > bossTurnDuration)
            {
                bossTurnTimer = 0;
                playerTurn();
            }

        }

        
    }

    public void playerTurn()
    {
        turnCount++;
        isPlayersTurn = true;
        UI.SetActive(true);
        plController.enabled = false;

        bossController.reStartPos();

        // Unhide UI
        // able player controller

        foreach (Tarot_Controllers i in BSController.Controllers)
        {
            i.enabled = false;


        }
    }


    public void BossTurn()
    {
        isPlayersTurn = false;
        UI.SetActive(false);
        plController.enabled = true;

        //hide UI
        //enable boss controller

        foreach (Tarot_Controllers i in BSController.Controllers)
        {
            i.enabled = true;


        }

    }



}
