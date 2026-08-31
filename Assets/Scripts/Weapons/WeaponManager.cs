using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private readonly List<WeaponBase> activeWeapons = new();

    public IReadOnlyList<WeaponBase> ActiveWeapons => activeWeapons;

    private void Start()
    {
        AddWeapon(WeaponType.Pistol);
    }

    public void AddWeapon(WeaponType weaponType)
    {
        GameObject weaponObject = new($"{weaponType}Weapon");
        weaponObject.transform.SetParent(transform);
        weaponObject.transform.localPosition = Vector3.zero;

        WeaponBase weapon = weaponType switch
        {
            WeaponType.Pistol => weaponObject.AddComponent<PistolWeapon>(),
            WeaponType.Axe => weaponObject.AddComponent<AxeWeapon>(),
            WeaponType.Bomb => weaponObject.AddComponent<BombWeapon>(),
            _ => null
        };

        if (weapon != null)
        {
            activeWeapons.Add(weapon);
        }
    }

    public int GetWeaponCount(WeaponType weaponType)
    {
        int count = 0;
        foreach (WeaponBase weapon in activeWeapons)
        {
            if (weapon == null)
            {
                continue;
            }

            switch (weaponType)
            {
                case WeaponType.Pistol when weapon is PistolWeapon:
                case WeaponType.Axe when weapon is AxeWeapon:
                case WeaponType.Bomb when weapon is BombWeapon:
                    count++;
                    break;
            }
        }

        return count;
    }
}
