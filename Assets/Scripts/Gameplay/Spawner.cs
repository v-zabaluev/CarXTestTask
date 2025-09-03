using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private MonsterMovementType _monsterMovementType;

        [SerializeField] private float _interval = 3;
        [SerializeField] private FlyingShield _flyingShieldPrefab;
        [SerializeField] private GameObject _monsterPrefab;
        [SerializeField] private GameObject _moveTarget;

        private float _lastSpawn = -1;

        private void Update()
        {
            if (Time.time > _lastSpawn + _interval)
            {
                GameObject monster = Spawn();

                if (_flyingShieldPrefab != null)
                    Instantiate(_flyingShieldPrefab, monster.transform);

                _lastSpawn = Time.time;
            }
        }

        private GameObject Spawn()
        {
            GameObject monster = Instantiate(_monsterPrefab, transform.position, Quaternion.identity);
            SetMovementType(monster, _monsterMovementType);

            return monster;
        }

        private void SetMovementType(GameObject monster, MonsterMovementType movementType)
        {
            if (monster.TryGetComponent(out MonsterMovementLinear linearMovement))
            {
                linearMovement.enabled = movementType == MonsterMovementType.Linear;
                linearMovement.SetTarget(_moveTarget);
            }

            if (monster.TryGetComponent(out MonsterMovementAccelerated acceleratedMovement))
            {
                acceleratedMovement.enabled = movementType == MonsterMovementType.Accelerated;
                acceleratedMovement.SetTarget(_moveTarget);
            }
        }
    }

    public enum MonsterMovementType
    {
        Linear,
        Accelerated,
    }
}