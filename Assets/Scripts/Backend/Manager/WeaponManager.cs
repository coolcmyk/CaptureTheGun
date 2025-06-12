using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    public WeaponController weaponController;

    void Start()
    {
        if (weaponController == null)
            weaponController = FindObjectOfType<WeaponController>();

        StartCoroutine(weaponController.GetAllWeapons(OnWeaponsReceived));
    }

    void OnWeaponsReceived(List<Weapon> weapons)
    {
        foreach (var w in weapons)
            Debug.Log("Weapon: " + w.name);
    }
}