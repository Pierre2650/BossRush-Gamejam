using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class WeaponManager : MonoBehaviour
{
    public Weapon main;
    public float mainOffset;

    public Weapon secondaryWeapon1;
    public float secondaryOffset1;
    public int secondaryCooldown1;
    public BooleanWrapper isReady1 = new BooleanWrapper(true);

    public Weapon secondaryWeapon2;
    public float secondaryOffset2;
    public int secondaryCooldown2;
    public BooleanWrapper isReady2 = new BooleanWrapper(true);

    bool isMouseUsed=false;
    [HideInInspector]public bool canAttack=true;

    public PlayerInputActions playerInputActions;


    public Weapon_Coldowns uiManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        main.gameObject.SetActive(true);
        secondaryWeapon1.gameObject.SetActive(false);
        secondaryWeapon2.gameObject.SetActive(false);

        playerInputActions = GetComponent<PlayerController>().playerInputActions;
        playerInputActions.Player.Aim.performed+=setWeaponDir;
        playerInputActions.Player.Special_1.performed+=specialAttack1;
        playerInputActions.Player.Special_2.performed+=specialAttack2;

        main.setOffset(mainOffset);
        secondaryWeapon1.setOffset(secondaryOffset1);
        secondaryWeapon2.setOffset(secondaryOffset2);

        main.setPlayer(transform);
        secondaryWeapon1.setPlayer(transform);
        secondaryWeapon2.setPlayer(transform);

        secondaryWeapon1.wp = this;
        secondaryWeapon2.wp = this;

        main.wp = this;

    }

    // Update is called once per frame
    void Update()
    {
        if(isMouseUsed){
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = (mousePos - (Vector2)transform.position).normalized;
            main.setDir(dir);
            secondaryWeapon1.setDir(dir);
            secondaryWeapon2.setDir(dir);
        }
    }

    void specialAttack1(InputAction.CallbackContext context){
        if(isReady1.activator && canAttack){
            canAttack = false;
            isReady1.activator = false;
            StartCoroutine(CooldownCoroutine(secondaryCooldown1, isReady1));

            uiManager.axeOnColdown(secondaryCooldown1);

            main.gameObject.SetActive(false);
            secondaryWeapon1.gameObject.SetActive(true);
            secondaryWeapon1.attack(context);

            
        }
    }

    void specialAttack2(InputAction.CallbackContext context){

        if(isReady2.activator && canAttack){

            canAttack = false;
            isReady2.activator = false;
            StartCoroutine(CooldownCoroutine(secondaryCooldown2, isReady2));
            uiManager.shotGunOnColdown(secondaryCooldown2);

            main.gameObject.SetActive(false);
            secondaryWeapon2.gameObject.SetActive(true);
            secondaryWeapon2.attack(context);

            
        }
    }

    IEnumerator CooldownCoroutine(int time, BooleanWrapper activator){
        yield return new WaitForSeconds(time);
        activator.activator = true;
    }


    void setWeaponDir(InputAction.CallbackContext context){

        if (context.control.device is Mouse)
        {
            isMouseUsed=true;
            
        }
        if (context.control.device is Gamepad)
        {
            isMouseUsed = false;
            Vector2 inputDir = playerInputActions.Player.Aim.ReadValue<Vector2>();
            main.setDir(inputDir);
            secondaryWeapon1.setDir(inputDir);
            secondaryWeapon2.setDir(inputDir);
        }
    }

    public class BooleanWrapper{
        public bool activator;

        public BooleanWrapper(bool v)
        {
            this.activator = v;
        }
    }
}
