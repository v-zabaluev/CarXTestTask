using Gameplay.Towers.Cannon;
using UnityEngine;

namespace StaticData.Projectile
{
    [CreateAssetMenu(fileName = "MortarProjectile", menuName = "StaticData/Projectile/Mortar")]
    public class MortarProjectileData : ScriptableObject
    {
        public MortarProjectileType Type;
        public MortarProjectile Prefab;
        public float Speed = 20;
        public int Damage = 10;
        public float MaxHeight = 10f;
    }
    
    public enum MortarProjectileType
    {
        Base,
        Super
    }
}