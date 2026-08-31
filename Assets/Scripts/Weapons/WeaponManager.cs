using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private readonly List<WeaponBase> activeWeapons = new();
    private Transform weaponRoot;

    public IReadOnlyList<WeaponBase> ActiveWeapons => activeWeapons;

    private void Awake()
    {
        GameObject weaponsObject = new("Weapons");
        weaponsObject.transform.SetParent(transform);
        weaponsObject.transform.localPosition = Vector3.zero;
        weaponRoot = weaponsObject.transform;
    }

    private void Start()
    {
        AddWeapon(WeaponType.Pistol);
    }

    public void AddWeapon(WeaponType weaponType)
    {
        GameObject weaponObject = new($"{weaponType}Weapon");
        weaponObject.transform.SetParent(weaponRoot != null ? weaponRoot : transform);
        weaponObject.transform.localPosition = GetWeaponOffset(weaponType, GetWeaponCount(weaponType));

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

    private static Vector3 GetWeaponOffset(WeaponType weaponType, int existingCount)
    {
        Vector3[] pistolOffsets =
        {
            new(-0.35f, 0.30f, 0f),
            new(0.35f, 0.30f, 0f),
            new(0f, 0.55f, 0f),
            new(-0.50f, 0.05f, 0f),
            new(0.50f, 0.05f, 0f)
        };

        Vector3[] bombOffsets =
        {
            new(-0.25f, -0.35f, 0f),
            new(0.25f, -0.35f, 0f),
            new(0f, -0.55f, 0f)
        };

        Vector3[] axeOffsets =
        {
            new(0f, 0f, 0f),
            new(-0.20f, 0f, 0f),
            new(0.20f, 0f, 0f)
        };

        Vector3[] offsets = weaponType switch
        {
            WeaponType.Pistol => pistolOffsets,
            WeaponType.Bomb => bombOffsets,
            WeaponType.Axe => axeOffsets,
            _ => axeOffsets
        };

        return offsets[existingCount % offsets.Length];
    }
}
