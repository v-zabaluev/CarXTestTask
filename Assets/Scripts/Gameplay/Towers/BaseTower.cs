using System.Collections.Generic;
using Gameplay.Movement;
using UnityEngine;

namespace Gameplay.Towers
{
    public abstract class BaseTower : MonoBehaviour
    {
        [SerializeField] protected float _shootInterval = 0.5f;
        protected float _lastShotTime = -0.5f;

        [SerializeField] protected TriggerObserver _triggerObserver;
        protected readonly List<MonsterMovementController> _targetsInRange = new();

        protected virtual void Awake()
        {
            _triggerObserver.TriggerEnter += OnTriggerEnterInvoked;
            _triggerObserver.TriggerExit += OnTriggerExitInvoked;
        }

        protected virtual void OnDestroy()
        {
            _triggerObserver.TriggerEnter -= OnTriggerEnterInvoked;
            _triggerObserver.TriggerExit -= OnTriggerExitInvoked;
        }

        protected void OnTriggerEnterInvoked(Collider other)
        {
            if (other.TryGetComponent(out MonsterMovementController  monster))
            {
                if (!_targetsInRange.Contains(monster))
                    _targetsInRange.Add(monster);
            }
        }

        protected void OnTriggerExitInvoked(Collider other)
        {
            if (other.TryGetComponent(out MonsterMovementController  monster))
            {
                _targetsInRange.Remove(monster);
            }
        }
    }
}