using System.Collections.Generic;
using Gameplay.Monsters;
using Gameplay.Monsters.Movement;
using Gameplay.Towers.Cannon;
using Gameplay.Towers.Cannon.Rotators;
using Gameplay.Towers.Cannon.ShootingMode;
using Infrastructure.Factory;
using Services;
using StaticData.Projectile;
using UnityEngine;

namespace Gameplay.Towers.Cannon
{
    public class CannonTower : BaseTower
    {
        [SerializeField] private Transform _shootPoint;

        [SerializeField] private CannonType _type;
        [SerializeField] private CannonProjectileType _cannonProjectileType;
        [SerializeField] private MortarProjectileType _mortarProjectileType;

        [SerializeField] private CannonBarrelRotator _barrelRotator;
        [SerializeField] private MortarBarrelRotator _mortarRotator;

        private ITowerShootingMode _shootingMode;
        private Vector3 _projectileDirection;
        private Vector3 _interceptPoint;

        public CannonProjectileType CannonProjectileType => _cannonProjectileType;
        public MortarProjectileType MortarProjectileType => _mortarProjectileType;
        public Vector3 ProjectileDirection => _projectileDirection;
        public Vector3 InterceptPoint => _interceptPoint;
        public Vector3 ShootPoint => _shootPoint.position;
        public CannonType CannonType => _type;

        protected override void Awake()
        {
            base.Awake();

            switch (_type)
            {
                case CannonType.Cannon:
                    _shootingMode = new CannonShootingMode(this, _barrelRotator, _cannonProjectileType);

                    break;
                case CannonType.Mortar:
                    _shootingMode = new MortarShootingMode(this, _mortarRotator, _mortarProjectileType);

                    break;
            }
        }

        private void Update()
        {
            if (_shootPoint == null || _targetsInRange.Count == 0) return;

            var target = GetValidTargetMovementController();

            if (target == null) return;

            if (target.GetInterceptInfo(_shootPoint.position, _shootingMode.GetProjectileSpeed(),
                    out _projectileDirection, out _interceptPoint))
            {
                Debug.DrawRay(_shootPoint.position, _projectileDirection * 30, Color.red, 1f);

                _shootingMode.RotateBarrel(_type == CannonType.Cannon ? _projectileDirection : _interceptPoint);

                if (WillShieldBlock(target)) return;

                if (_shootingMode.IsAimed() && _lastShotTime + _shootInterval <= Time.time)
                {
                    _shootingMode.Shoot(_projectileDirection);
                    _lastShotTime = Time.time;
                }
            }
        }

        private bool WillShieldBlock(MonsterMovementController monsterMovementController)
        {
            var defense = monsterMovementController.GetComponent<MonsterDefenseController>();

            if (defense == null) return false;

            float projectileRadius = _shootingMode.GetProjectileRadius();

            return defense.IsInterceptBlocked(
                projectileRadius,
                _shootPoint.position,
                _projectileDirection,
                _shootingMode.GetProjectileSpeed(),
                6f
            );
        }

        private MonsterMovementController GetValidTargetMovementController()
        {
            _targetsInRange.RemoveAll(m => m == null);

            return _targetsInRange.Count > 0 ? _targetsInRange[0] : null;
        }
    }
}