using UnityEngine;

namespace Gameplay.Towers.Cannon
{
    public interface ITowerShootingMode
    {
        void RotateBarrel(Vector3 targetPoint);
        bool IsAimed();
        void Shoot(Vector3 targetPoint);
        float GetProjectileSpeed();
    }
}