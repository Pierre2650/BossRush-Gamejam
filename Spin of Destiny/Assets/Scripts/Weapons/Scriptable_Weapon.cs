using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScrptWeapon")]
public class Scriptable_Weapon : ScriptableObject
{
    public int damage;
    public Enum_Weapons weapon;
    public Sprite sprite;


}
