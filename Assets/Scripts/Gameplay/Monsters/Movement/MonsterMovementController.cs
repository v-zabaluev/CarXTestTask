using System;
using UnityEngine;

namespace Gameplay.Movement
{
    public class MonsterMovementController : MonoBehaviour
    {
        private MonsterMovementType _currentType = MonsterMovementType.Linear;

        [SerializeField] private MonsterMovementLinear _linear;
        [SerializeField] private MonsterMovementAccelerated _accelerated;
        [SerializeField] private MonsterMovementCircular _circular;

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
        }
    }
}