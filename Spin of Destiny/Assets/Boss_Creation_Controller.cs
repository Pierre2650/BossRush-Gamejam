using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Creation_Controller : MonoBehaviour
{
    public Card_Selection_Controller CSController;
    public GameObject boss;
    public List<Tarot> Cards;



    private void OnEnable()
    {
        foreach (Enum_Card i in CSController.cards)
        {
            switch (i)
            {
                case Enum_Card.Chariot:
                    boss.AddComponent<Chariot>();
                    Chariot temp = boss.GetComponent<Chariot>();
                    Cards.Add(temp);
                    temp.enabled = false;
                    break;


                case Enum_Card.Tower:
                    boss.AddComponent<Tower>();
                    Tower temp1 = boss.GetComponent<Tower>();
                    Cards.Add(temp1);
                  
                    break;


            }
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
