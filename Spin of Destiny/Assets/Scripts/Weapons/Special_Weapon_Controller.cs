using UnityEngine;

public class Special_Weapon_Controller : MonoBehaviour
{
    [Header("Init")]
    private SpriteRenderer mySprd;


    [Header("Description")]
    public Scriptable_Weapon Scriptable_Weapon;
    public int damage = 0;
    public Enum_Weapons weapon;

    [Header("Main Weapon")]
    public GameObject pistol;

    [Header("Roulette")]
    public GameObject roulette;
    private Weapon_Roulette rouletteController;

    // Start is called before the first frame update
    void Start()
    {
        mySprd = GetComponent<SpriteRenderer>();
        rouletteController = roulette.GetComponent<Weapon_Roulette>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    public void setWeapon()
    {
        pistol.SetActive(false);
        roulette.SetActive(false);

        damage = Scriptable_Weapon.damage;

        weapon = Scriptable_Weapon.weapon;
        findWeaponScript();

        mySprd.sprite = Scriptable_Weapon.sprite;
        mySprd.enabled = true;

    }

    public void resetWeapon()
    {
        pistol.SetActive(true);
        roulette.SetActive(true);

        damage = 0;

        mySprd.enabled = false;
    }



    private void findWeaponScript()
    {
        switch (weapon) {
            case Enum_Weapons.Sword:
                this.gameObject.AddComponent<Sword>();
                break;
        
        }
    }
}
