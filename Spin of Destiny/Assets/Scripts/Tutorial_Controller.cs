using UnityEngine;

public class Tutorial_Controller : MonoBehaviour
{
    public GameObject tutoSpear;
    private bool spearDone = false;

    public GameObject tutoAxe;
    private bool axeDone = false;

    public GameObject tutoGun;
    private bool gunDone = false;
    //public GameObject player;

    public bool tutoEnd = false;


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0) && !spearDone)
        {
            tutoSpear.SetActive(false);
            spearDone = true;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && !axeDone)
        {
            tutoAxe.SetActive(false);
            axeDone = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !gunDone)
        {
            tutoGun.SetActive(false);
            gunDone = true;
        }

        if(spearDone && axeDone && gunDone)
        {
            tutoEnd = true;
        }
    }
}
