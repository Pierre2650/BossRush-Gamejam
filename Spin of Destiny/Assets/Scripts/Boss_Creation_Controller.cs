using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Boss_Creation_Controller : MonoBehaviour
{
    public Card_Selection_Controller CSController;
    public GameObject Boss;
    public Image[] bossHealthBar;
    
    public List<MonoBehaviour> Controllers;
    private bool spawnBoss = false;

    public Tutorial_Controller tutorial;

    private void OnEnable()
    {
        spawnBoss = false;
        //findCard();
    }

    private void Update()
    {
        if(tutorial.tutoEnd && !spawnBoss)
        {
            StartCoroutine(findCard());
            spawnBoss = true;

        }
    }


    private IEnumerator findCard()
    {
        foreach(Image i in bossHealthBar)
        {
            i.enabled = true;
        }

        yield return new WaitForSeconds(4);

        foreach (Card_Enum_Type card in CSController.cards)
        {
            switch (card.value)
            {
                case Enum_Card.Chariot:

                    Chariot ch = new Chariot();

                    ch.chooseController(card.type, Boss, Controllers);

                    break;


                case Enum_Card.Tower:

                    Tower tw = new Tower();

                    tw.chooseController(card.type, Boss, Controllers);

                    break;

                case Enum_Card.World:

                    World wr = new World();

                    wr.chooseController(card.type, Boss, Controllers);

                    break;


                case Enum_Card.Star:

                    Star st = new Star();

                    st.chooseController(card.type, Boss, Controllers);

                    break;

                case Enum_Card.Devil:

                    Devil dv = new Devil();

                    dv.chooseController(card.type, Boss, Controllers);

                    break;


                case Enum_Card.Lovers:

                    Lovers lv = new Lovers();

                    lv.chooseController(card.type, Boss, Controllers);

                    break;


            }
        }


        Boss.GetComponent<SpriteRenderer>().enabled = true;
        Boss.GetComponent<BoxCollider2D>().enabled = true;

    }


    public void removeControllers()
    {
        foreach (MonoBehaviour c in Controllers.ToList())
        {
            Controllers.Remove(c);
            Destroy(c);
        }
    }


}
