using Gameplay.Monsters.Movement;
using Infrastructure.Factory;
using StaticData.Projectile;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Towers.SimpleTower
{
    public class SimpleTower : BaseTower
    {
        [SerializeField] private GuidedProjectileType _projectileType;

        private void Update()
        {
            if (_targetsInRange.Count == 0) return;

            MonsterMovementController monster = GetValidTarget();

            if (monster == null) return;

            if (_lastShotTime + _shootInterval <= Time.time)
            {
                // shot
                var projectile = GameFactory.Instance.CreateGuidedProjectile(_projectileType,
                    transform.position + Vector3.up * 1.5f, Quaternion.identity, monster.gameObject.transform);

                _lastShotTime = Time.time;
            }
        }

        private MonsterMovementController GetValidTarget()
        {
            _targetsInRange.RemoveAll(m => m == null);

            return _targetsInRange.Count > 0 ? _targetsInRange[0] : null;
        }
    }
}