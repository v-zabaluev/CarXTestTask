using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Towers.SimpleTower
{
    public class SimpleTower : BaseTower
    {
        public GuidedProjectile _projectilePrefab;

        private void Update()
        {
            if (_projectilePrefab == null)
                return;

            if (_targetsInRange.Count == 0) return;

            Monster monster = GetValidTarget();

            if (monster == null) return;

            if (_lastShotTime + _shootInterval <= Time.time)
            {
                // shot
                var projectile =
                    Instantiate(_projectilePrefab, transform.position + Vector3.up * 1.5f,
                        Quaternion.identity);

                var projectileBeh = projectile.GetComponent<GuidedProjectile>();
                projectileBeh.SetTarget(monster.gameObject);

                _lastShotTime = Time.time;
            }
        }

        private Monster GetValidTarget()
        {
            _targetsInRange.RemoveAll(m => m == null);

            return _targetsInRange.Count > 0 ? _targetsInRange[0] : null;
        }
    }
}