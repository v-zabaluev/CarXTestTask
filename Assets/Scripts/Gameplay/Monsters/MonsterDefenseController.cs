using Gameplay.Gameplay;
using UnityEngine;

namespace Gameplay.Monsters
{
    public class MonsterDefenseController : MonoBehaviour
    {
        [SerializeField] private FlyingShieldProtection _shield;
        [SerializeField] private FlyingShieldMovement _movement;

        public bool IsInterceptBlocked(Collider projectileCollide, Vector3 shooterPosition, Vector3 projectileDirection,
            float projectileSpeed, float maxTime = 6f)
        {
            if (_shield == null)
                return false;

            return _shield.WillBlockIntercept(projectileCollide, shooterPosition, projectileDirection, projectileSpeed,
                _movement.CurrentAngle, _movement.AngularSpeed, _movement.Radius, _movement.Offset, maxTime);
        }
    }
}