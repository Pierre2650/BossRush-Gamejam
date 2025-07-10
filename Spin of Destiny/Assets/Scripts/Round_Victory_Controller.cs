using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Round_Victory_Controller : MonoBehaviour
{
    public GameObject Boss;
    public GameObject Player;

    public GameObject game;
    public GameObject cardSelection;

    public Boss_Creation_Controller bossCreationC;
    public void victory()
    {
        StartCoroutine(waitToEnd());
    }

    private IEnumerator waitToEnd()
    {
        //bossCreationC.stopController();
        bossCreationC.removeControllers();

        foreach (Image i in bossCreationC.bossHealthBar)
        {
            i.enabled = false;
        }

        Boss.GetComponent<SpriteRenderer>().enabled = false;
        Boss.GetComponent<BoxCollider2D>().enabled = false;
        Boss.GetComponent<Enemy_Controller>().reStartPos();

        yield return new WaitForSeconds(4);

        resetBoss();
        resetPlayer();
        backToCardSelection();
    }
    private void backToCardSelection()
    {
        cardSelection.SetActive(true);
        game.SetActive(false);

    }

    private void resetBoss()
    {
        //bossCreationC.resetControllers();
        Boss.GetComponent<BoxCollider2D>().enabled = true;
        Boss.GetComponent<Health>().resetHealth();
        

    }
   private void resetPlayer()
    {
        Player.GetComponent<PlayerController>().resetPlayer();

    }
}
