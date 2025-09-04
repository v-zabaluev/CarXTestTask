using UnityEngine;

namespace Gameplay.Monsters.Movement
{
    public class MonsterMovementController : MonoBehaviour
    {
        private MonsterMovementType _currentType = MonsterMovementType.Linear;

        [SerializeField] private MonsterMovementLinear _linear;
        [SerializeField] private MonsterMovementAccelerated _accelerated;
        [SerializeField] private MonsterMovementCircular _circular;
        [SerializeField] private MonsterMovementPathFollow _pathFollow;

        private Transform _moveTarget;
        private Transform _orbitTarget;

        public void SetTargetPoints(Transform moveTarget, Transform orbitTarget)
        {
            _moveTarget = moveTarget;
            _orbitTarget = orbitTarget;
        }

        public void SwitchMovementType(MonsterMovementType newType)
        {
            ApplyMovementType(newType);
        }

        public MonsterMovementType GetCurrentMovementType()
        {
            return _currentType;
        }

        private void ApplyMovementType(MonsterMovementType type)
        {
            _currentType = type;

            if (_linear != null)
            {
                _linear.enabled = type == MonsterMovementType.Linear;
                if (_linear.enabled) _linear.SetTarget(_moveTarget);
            }

            if (_accelerated != null)
            {
                _accelerated.enabled = type == MonsterMovementType.Accelerated;
                if (_accelerated.enabled) _accelerated.SetTarget(_moveTarget);
            }

            if (_circular != null)
            {
                _circular.enabled = type == MonsterMovementType.Circular;

                if (_circular.enabled)
                {
                    _circular.Construct(_orbitTarget);
                }
            }
            
            if (_pathFollow != null)
            {
                _pathFollow.enabled = type == MonsterMovementType.PathFollow;
            }

        }

        public bool GetInterceptInfo(Vector3 shootPointPosition, float projectileSpeed,
            out Vector3 projectileDirection, out Vector3 interceptPoint)
        {
            switch (_currentType)
            {
                case MonsterMovementType.Linear:
                    _linear.CalculateIntercept(shootPointPosition, projectileSpeed, out projectileDirection,
                        out interceptPoint);

                    return true;

                case MonsterMovementType.Accelerated:
                    _accelerated.CalculateIntercept(shootPointPosition, projectileSpeed, out projectileDirection,
                        out interceptPoint);

                    return true;

                case MonsterMovementType.Circular:
                    _circular.CalculateIntercept(shootPointPosition, projectileSpeed, out projectileDirection,
                        out interceptPoint);

                    return true;
                
                case MonsterMovementType.PathFollow:
                    _pathFollow.CalculateIntercept(shootPointPosition, projectileSpeed, out projectileDirection,
                        out interceptPoint);
                    return true;

                default:
                    projectileDirection = Vector3.zero;
                    interceptPoint = Vector3.zero;

                    return false;
            }
        }
    }
}