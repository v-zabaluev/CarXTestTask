using Gameplay.Towers.Cannon;
using Gameplay.Towers.Cannon.Projectile;
using Services;
using UnityEngine;

namespace StaticData.Projectile
{
    [CreateAssetMenu(fileName = "MortarProjectile", menuName = "StaticData/Projectile/Mortar")]
    public class MortarProjectileData : ScriptableObject, IProjectileData<MortarProjectileType>
    {
        [SerializeField] private MortarProjectileType _type;
        public MortarProjectileType Type => _type;
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