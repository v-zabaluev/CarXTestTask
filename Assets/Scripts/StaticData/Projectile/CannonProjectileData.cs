using System;
using Gameplay.Towers.Cannon;
using Gameplay.Towers.Cannon.Projectile;
using UnityEngine;

namespace StaticData.Projectile
{
    [CreateAssetMenu(fileName = "CannonProjectile", menuName = "StaticData/Projectile/Cannon")]
    public class CannonProjectileData : ScriptableObject, IProjectileData<CannonProjectileType>
    {
        [SerializeField] private CannonProjectileType _type;
        public CannonProjectileType Type => _type;
        public CannonProjectile Prefab;
        public float Speed = 20;
        public int Damage = 10;
    }

    public enum CannonProjectileType
    {
        Base,
        Super
    }
}