using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public new string name;

    [Header("Shooting")]
    public float maxDistance;
    public float shootForce;
    public float spread;

    [Header("Reloading")]
    public int magSize;
    public float fireRate;
    public float reloadTime;
}
