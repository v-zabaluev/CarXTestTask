using Gameplay.Towers.Cannon;
using Gameplay.Towers.SimpleTower;
using UnityEngine;

namespace StaticData.Projectile
{
    [CreateAssetMenu(fileName = "GuidedProjectile", menuName = "StaticData/Projectile/Guided")]
    public class GuidedProjectileData : ScriptableObject, IProjectileData<GuidedProjectileType>
    {
        [SerializeField] private GuidedProjectileType _type;
        public GuidedProjectileType Type => _type;
        public GuidedProjectile Prefab;
        public float Speed = 20;
        public int Damage = 10;
    }
    
    public enum GuidedProjectileType
    {
        Base,
        Super
    }
}