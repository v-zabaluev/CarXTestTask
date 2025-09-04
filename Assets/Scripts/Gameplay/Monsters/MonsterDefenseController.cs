using Gameplay.Shield;
using Gameplay.Shield.Gameplay;
using UnityEngine;

namespace Gameplay.Monsters
{
    public class MonsterDefenseController : MonoBehaviour
    {
        [SerializeField] private FlyingShieldProtection _shield;
        [SerializeField] private FlyingShieldMovement _movement;

        public bool IsInterceptBlocked(float projectileRadius, Vector3 shooterPosition, Vector3 projectileDirection,
            float projectileSpeed, float maxTime = 6f)
        {
            if (_shield == null)
                return false;

            return _shield.WillBlockIntercept(shooterPosition, projectileDirection, projectileSpeed,
                _movement.CurrentAngle, _movement.AngularSpeed, _movement.Radius, projectileRadius, _movement.Offset,
                maxTime);
        }
    }
}