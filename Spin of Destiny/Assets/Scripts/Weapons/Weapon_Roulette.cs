using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.XR;


public class Weapon_Roulette : MonoBehaviour
{
    [Header("Spawn")]
    public Vector2 spawnPos = Vector2.zero;
    private Vector2 lastPos = Vector2.zero;

    [Header("Weapon")]
    Enum_Weapons lastRoll;



    [Header("player")]
    public GameObject player;
    public GameObject specialWeapon;
    private Special_Weapon_Controller spwController;

    [Header("ObjMouvement")]
    private PickableObj_Mouvement PickableObj_Mouvement;

    // Start is called before the first frame update
    void Awake()
    {
        PickableObj_Mouvement = GetComponent<PickableObj_Mouvement>();
        spwController = specialWeapon.GetComponent<Special_Weapon_Controller>();

    }

    private void OnEnable()
    {
        SpawnRoulette();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //Debug.Log("spawn");
            //SpawnRoulette();
        }
        
    }

    private void SpawnRoulette()
    {
        PickableObj_Mouvement.enabled = false;

        int spawnZone = Random.Range(1, 4);
        int temp = 0;

        do
        {
            spawnPos = generateSpawnPos(spawnZone);
            temp++;

        } while (Vector2.Distance(lastPos, spawnPos) < 2.5f && Vector2.Distance(player.transform.position, spawnPos) < 6f && temp < 200);

        if (temp != 1)
        {
            Debug.Log("SpawnPos on Star, Temp = " + temp);
        }

        lastPos = spawnPos;
        transform.position = spawnPos;

        PickableObj_Mouvement.enabled = true;

    }

    private Vector2 generateSpawnPos(int zone)
    {
        switch (zone)
        {
            case 1:
                return new Vector2(Random.Range(-15, -8), Random.Range(-9, 9));

            case 2:
                return new Vector2(Random.Range(-8, 5), Random.Range(-9, 2));

            case 3:
                return new Vector2(Random.Range(5, 16), Random.Range(-9, 9));

            default:
                return Vector2.zero;

        }

    }

    private void Spin()
    {
        int roll = Random.Range(1, 4);
        roll = 1;
        switch (roll)
        {
            case 1:

                lastRoll = Enum_Weapons.Sword;
                spwController.Scriptable_Weapon = Resources.Load<Scriptable_Weapon>("Sword");
                spwController.setWeapon();
                break;
                

            case 2:
                lastRoll = Enum_Weapons.Crossbow;
                break;

            case 3:
                lastRoll = Enum_Weapons.Katana;
                break;

            default:
                break;

        }
    } 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Spin();

        }
    }
}
