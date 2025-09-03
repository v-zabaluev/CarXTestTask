using Gameplay.Towers.Cannon;
using Infrastructure.Pools;
using StaticData.Projectile;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private static GameFactory _instance;

        private readonly CannonProjectilePool _cannonProjectilePool;
        private readonly MortarProjectilePool _mortarProjectilePool;

        public static GameFactory Instance => _instance ??= new GameFactory();

        private GameFactory()
        {
            _cannonProjectilePool = new CannonProjectilePool();
            _mortarProjectilePool = new MortarProjectilePool();
        }

        public GameObject CreateCannonProjectile(CannonProjectileType type, Vector3 spawnPosition, Quaternion rotation, Vector3 targetPosition)
        {
            var projectile = _cannonProjectilePool.Get(spawnPosition, rotation, targetPosition, type);
            return projectile?.gameObject;
        }

        public GameObject CreateMortarProjectile(MortarProjectileType type, Vector3 spawnPosition, Quaternion rotation, Vector3 targetPosition)
        {
            var projectile = _mortarProjectilePool.Get(spawnPosition, rotation, targetPosition, type);
            return projectile?.gameObject;
        }
    }
}