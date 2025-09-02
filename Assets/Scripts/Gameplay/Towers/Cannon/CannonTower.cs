using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Towers.Cannon
{
    public class CannonTower : BaseTower
    {
        //[SerializeField] private float _range = 4f;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private CannonType _type;

        private Vector3 _projectileDirection;
        private Vector3 _interceptPoint;
        public Vector3 ProjectileDirection => _projectileDirection;

        private void Awake()
        {
            _triggerObserver.TriggerEnter += OnTriggerEnterInvoked;
            _triggerObserver.TriggerExit += OnTriggerExitInvoked;
        }

        private void OnDestroy()
        {
            _triggerObserver.TriggerEnter -= OnTriggerEnterInvoked;
            _triggerObserver.TriggerExit -= OnTriggerExitInvoked;
        }

        private void Update()
        {
            if (_projectilePrefab == null || _shootPoint == null) return;
            if (_targetsInRange.Count == 0) return;

            MonsterMovement monster = GetValidTarget();

            if (monster == null) return;

            float projectileSpeed = _projectilePrefab.GetComponent<CannonProjectileData>().Speed;

            if (CalculateInterceptDirection(_shootPoint.position, monster.transform.position,
                    CalculateMonsterSpeedVector(monster), projectileSpeed, out _projectileDirection,
                    out _interceptPoint))
            {
                Debug.DrawRay(_shootPoint.position, _projectileDirection * 30, Color.red, 1f);

                if (_lastShotTime + _shootInterval <= Time.time)
                {
                    Shoot(_projectileDirection);
                    _lastShotTime = Time.time;
                }
            }
        }

        private MonsterMovement GetValidTarget()
        {
            _targetsInRange.RemoveAll(m => m == null);

            return _targetsInRange.Count > 0 ? _targetsInRange[0] : null;
        }

        private void Shoot(Vector3 direction)
        {
            Debug.DrawRay(_shootPoint.position, direction * 30, Color.blue, 2f);

            GameObject projectile =
                Instantiate(_projectilePrefab, _shootPoint.position, Quaternion.LookRotation(direction));

            projectile.GetComponent<CannonProjectileData>().Initialize(_type, _interceptPoint);
        }

        private bool CalculateInterceptDirection(Vector3 shooterPos, Vector3 monsterPos, Vector3 monsterVelocity,
            float projectileSpeed, out Vector3 direction, out Vector3 interceptPoint)
        {
            Vector3 displacement = monsterPos - shooterPos;
            float a = Vector3.Dot(monsterVelocity, monsterVelocity) - projectileSpeed * projectileSpeed;
            float b = 2f * Vector3.Dot(monsterVelocity, displacement);
            float c = Vector3.Dot(displacement, displacement);

            float discriminant = b * b - 4 * a * c;

            if (discriminant < 0f)
            {
                direction = Vector3.zero;
                interceptPoint = Vector3.zero;

                return false;
            }

            float sqrtD = Mathf.Sqrt(discriminant);
            float t1 = (-b - sqrtD) / (2 * a);
            float t2 = (-b + sqrtD) / (2 * a);

            float t = Mathf.Min(t1, t2);

            if (t < 0f)
                t = Mathf.Max(t1, t2);

            if (t < 0f)
            {
                direction = Vector3.zero;
                interceptPoint = Vector3.zero;

                return false;
            }

            interceptPoint = monsterPos + monsterVelocity * t;
            direction = (interceptPoint - shooterPos).normalized;

            return true;
        }

        private Vector3 CalculateMonsterSpeedVector(MonsterMovement monster)
        {
            return monster.GetSpeedVector();
        }
    }
}