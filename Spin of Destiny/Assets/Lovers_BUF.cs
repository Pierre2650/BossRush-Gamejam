using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lovers_BUF : MonoBehaviour
{

    public GameObject boss;
    private GameObject copie;
    private Vector2 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = new Vector2(transform.position.x + 2, transform.position.y);
        boss.transform.position = new Vector2(transform.position.x - 2, transform.position.y);

        copyBoss();

        StartCoroutine(waitToSpawn());
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Instantiate(boss, transform.right,transform.rotation,transform.parent);
        }
        
    }
    
    private void copyBoss()
    {
        copie = Instantiate(boss, startPos, transform.rotation, transform.parent);
        Lovers_BUF temp3 = copie.GetComponent<Lovers_BUF>();
        copie.SetActive(false);
        temp3.enabled = false;
    }

    private IEnumerator waitToSpawn()
    {
        yield return new WaitForSeconds(1f);
        copie.SetActive(true);


    }
}
