using System.Collections;
using UnityEngine;
using Gameplay.Monsters.Movement.MovementVariants;

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
        private Coroutine _circularRoutine;

        public void SetTargetPoints(Transform moveTarget, Transform orbitTarget)
        {
            _moveTarget = moveTarget;
            _orbitTarget = orbitTarget;
        }

        public MonsterMovementType GetCurrentMovementType() => _currentType;

        public void SwitchMovementType(MonsterMovementType newType)
        {
            if (_circularRoutine != null)
            {
                StopCoroutine(_circularRoutine);
            }

            if (newType == MonsterMovementType.Circular && _circular != null)
            {
                _circularRoutine = StartCoroutine(SwitchToCircularRoutine());
            }
            else
            {
                ApplyMovementType(newType);
            }
        }

        private IEnumerator SwitchToCircularRoutine()
        {
            _circular.Construct(_orbitTarget);
            ApplyMovementType(MonsterMovementType.Linear);

            Vector3 orbitPoint = _circular.GetClosestOrbitPoint(transform.position);
            _linear.StartMoveToPoint(orbitPoint);

            while (!_circular.IsOnOrbit(transform.position))
            {
                yield return new WaitForFixedUpdate();
            }

            _linear.StopMoveToPoint();
            ApplyMovementType(MonsterMovementType.Circular);
        }

        private void ApplyMovementType(MonsterMovementType type)
        {
            _currentType = type;

            DeactivateMoveMode();

            switch (type)
            {
                case MonsterMovementType.Linear:
                    if (_linear != null)
                    {
                        _linear.enabled = true;
                        _linear.SetTarget(_moveTarget);
                    }

                    break;

                case MonsterMovementType.Accelerated:
                    if (_accelerated != null)
                    {
                        _accelerated.enabled = true;
                        _accelerated.SetTarget(_moveTarget);
                    }

                    break;

                case MonsterMovementType.Circular:
                    if (_circular != null)
                    {
                        _circular.Construct(_orbitTarget);
                        _circular.enabled = true;
                    }

                    break;

                case MonsterMovementType.PathFollow:
                    if (_pathFollow != null)
                        _pathFollow.enabled = true;

                    break;
            }
        }

        private void DeactivateMoveMode()
        {
            if (_linear != null) _linear.enabled = false;
            if (_accelerated != null) _accelerated.enabled = false;
            if (_circular != null) _circular.enabled = false;
            if (_pathFollow != null) _pathFollow.enabled = false;
        }

        public bool GetInterceptInfo(Vector3 shootPointPosition, float projectileSpeed,
            out Vector3 projectileDirection, out Vector3 interceptPoint)
        {
            switch (_currentType)
            {
                case MonsterMovementType.Linear:
                    return _linear.CalculateIntercept(shootPointPosition, projectileSpeed,
                        out projectileDirection, out interceptPoint);
                case MonsterMovementType.Accelerated:
                    return _accelerated.CalculateIntercept(shootPointPosition, projectileSpeed,
                        out projectileDirection, out interceptPoint);
                case MonsterMovementType.Circular:
                    return _circular.CalculateIntercept(shootPointPosition, projectileSpeed,
                        out projectileDirection, out interceptPoint);
                case MonsterMovementType.PathFollow:
                    return _pathFollow.CalculateIntercept(shootPointPosition, projectileSpeed,
                        out projectileDirection, out interceptPoint);
                default:
                    projectileDirection = Vector3.zero;
                    interceptPoint = Vector3.zero;

                    return false;
            }
        }
    }
}