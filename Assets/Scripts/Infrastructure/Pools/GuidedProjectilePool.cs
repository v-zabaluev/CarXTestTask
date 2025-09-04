using Gameplay.Towers.SimpleTower;
using Services;
using StaticData.Projectile;
using UnityEngine;
using UnityEngine.Pool;

namespace Infrastructure.Pools
{
    public class GuidedProjectilePool
    {
        private readonly ObjectPool<GuidedProjectile> _pool;

        public GuidedProjectilePool(GuidedProjectileType type = GuidedProjectileType.Base, int capacity = 10)
        {
            _pool = new ObjectPool<GuidedProjectile>(
                createFunc: () =>
                {
                    var data = StaticDataService.GetProjectile<GuidedProjectileData, GuidedProjectileType>(type);
                    var go = Object.Instantiate(data.Prefab.gameObject);
                    var projectile = go.GetComponent<GuidedProjectile>();
                    go.SetActive(false);

                    return projectile;
                },
                actionOnGet: p => p.gameObject.SetActive(true),
                actionOnRelease: p => p.gameObject.SetActive(false),
                actionOnDestroy: p => Object.Destroy(p.gameObject),
                collectionCheck: false,
                defaultCapacity: capacity
            );
        }

        public GuidedProjectile Get(Vector3 spawnPosition, Quaternion rotation, Transform target,
            GuidedProjectileType type)
        {
            var data = StaticDataService.GetProjectile<GuidedProjectileData, GuidedProjectileType>(type);

            if (data == null) return null;

            var projectile = _pool.Get();
            projectile.transform.SetPositionAndRotation(spawnPosition, rotation);
            projectile.SetTargetTransform(target);
            projectile.Initialize( data.Speed, data.Damage);
            projectile.OnDespawn += Release;

            return projectile;
        }

        public void Release(GuidedProjectile projectile) => _pool.Release(projectile);
    }
}