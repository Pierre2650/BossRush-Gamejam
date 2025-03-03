using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Creation_Controller : MonoBehaviour
{
    public Card_Selection_Controller CSController;
    public GameObject boss;
    public List<MonoBehaviour> Controllers;

    private void OnEnable()
    {

        findCard();
    }


    private void findCard()
    {

        foreach (Card_Enum_Type card in CSController.cards)
        {
            switch (card.value)
            {
                case Enum_Card.Chariot:

                    Chariot ch = new Chariot();

                    ch.chooseController(card.type, boss, Controllers);

                    break;


                case Enum_Card.Tower:

                    Tower tw = new Tower();

                    tw.chooseController(card.type, boss, Controllers);

                    break;

                case Enum_Card.World:

                    World wr = new World();

                    wr.chooseController(card.type, boss, Controllers);

                    break;


                case Enum_Card.Star:

                    Star st = new Star();

                    st.chooseController(card.type, boss, Controllers);

                    break;

                case Enum_Card.Devil:

                    Devil dv = new Devil();

                    dv.chooseController(card.type, boss, Controllers);

                    break;


                case Enum_Card.Lovers:

                    Lovers lv = new Lovers();

                    lv.chooseController(card.type, boss, Controllers);

                    break;


            }
        }

    }


    

}
