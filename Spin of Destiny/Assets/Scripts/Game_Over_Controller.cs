using UnityEngine;

public class Game_Over_Controller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is
    // 

    [Header("Boss")]
    public GameObject Boss;

    [Header("UI")]
    public UI_Controller uiManager;
    

    public void GameOver()
    {
        uiManager.gameOverUI();
    }

}
