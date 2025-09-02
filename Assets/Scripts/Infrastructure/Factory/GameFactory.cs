using Gameplay.Towers.Cannon;
using Services;
using StaticData.Projectile;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private static GameFactory _instance;

        public static GameFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameFactory();
                }

                return _instance;
            }
        }

        public GameObject CreateCannonProjectile(CannonProjectileType type, Vector3 spawnPosition, Quaternion rotation,
            Vector3 targetPosition)
        {
            CannonProjectileData data = StaticDataService.GetCannonProjectile(type);

            if (data == null) return null;

            GameObject projectileGO = Object.Instantiate(data.Prefab.gameObject, spawnPosition, rotation);
            var projectile = projectileGO.GetComponent<CannonProjectile>();
            projectile.Initialize(targetPosition, data.Speed, data.Damage);

            return projectileGO;
        }

        public GameObject CreateMortarProjectile(MortarProjectileType type, Vector3 spawnPosition, Quaternion rotation,
            Vector3 targetPosition)
        {
            var data = StaticDataService.GetMortarProjectile(type);

            if (data == null) return null;

            GameObject projectileGO = Object.Instantiate(data.Prefab.gameObject, spawnPosition, rotation);
            var projectile = projectileGO.GetComponent<MortarProjectile>();
            projectile.Initialize(targetPosition, data.Speed, data.Damage, data.MaxHeight);

            return projectileGO;
        }
    }
}