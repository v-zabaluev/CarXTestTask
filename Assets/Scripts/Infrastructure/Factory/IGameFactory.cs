using Gameplay.Towers.Cannon;
using StaticData.Projectile;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameFactory
    {
        GameObject CreateCannonProjectile(CannonProjectileType type, Vector3 spawnPosition, Quaternion rotation,
            Vector3 targetPosition);

        GameObject CreateMortarProjectile(MortarProjectileType type, Vector3 spawnPosition, Quaternion rotation,
            Vector3 targetPosition);
    }
}