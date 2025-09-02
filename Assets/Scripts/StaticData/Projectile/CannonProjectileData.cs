using UnityEngine;

namespace StaticData.Projectile
{
    [CreateAssetMenu(fileName = "CannonProjectile", menuName = "StaticData/Projectile/Cannon")]
    public class CannonProjectileData : ScriptableObject
    {
        public CannonProjectileType Type;
        public float Speed = 20;
        public float Damage = 10;
    }

    public enum CannonProjectileType
    {
        Base,
        Super
    }
}