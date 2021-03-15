using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ProjectileSpellDescription : SpellDescription
{
    [Header("Projectile")]
    public GameObject ProjectilePrefab;
    public float ProjectileSpawnDelay;
}
