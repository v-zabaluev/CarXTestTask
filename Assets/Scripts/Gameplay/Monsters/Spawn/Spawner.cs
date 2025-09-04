using System.Collections.Generic;
using Gameplay.Monsters.Movement;
using Gameplay.Monsters.Movement.MovementVariants;
using UnityEngine;

namespace Gameplay.Monsters.Spawn
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private MonsterMovementType _monsterMovementType;

        [SerializeField] private float _interval = 3;

        [Header("Monster Prefabs")]

        [SerializeField] private GameObject _monsterPrefab;

        [Header("For moving to target")]

        [SerializeField] private Transform _moveTarget;

        [Header("For moving around target")]

        [SerializeField] private Transform _orbitTarget;

        [Header("For path following")]

        [SerializeField] private List<Transform> _pathCheckpoints;

        private float _lastSpawn = -1;
        private List<GameObject> _spawnedMonsters = new List<GameObject>();

        private MonsterFactory _factory;
        private MonsterMovementManager _movementManager;

        private void Awake()
        {
            _factory = new MonsterFactory(_monsterPrefab, _moveTarget, _orbitTarget, _pathCheckpoints);
            _movementManager = new MonsterMovementManager(_moveTarget, _orbitTarget, _pathCheckpoints);
        }

        private void Update()
        {
            if (Time.time > _lastSpawn + _interval)
            {
                GameObject monster = _factory.CreateMonster(transform.position, Quaternion.identity,
                    _monsterMovementType, RemoveMonster);

                _spawnedMonsters.Add(monster);

                _lastSpawn = Time.time;
            }
        }

        private void RemoveMonster(GameObject monster)
        {
            _spawnedMonsters.Remove(monster);
        }

        public void SwitchMovement(MonsterMovementType type)
        {
            _movementManager.SwitchMovement(_spawnedMonsters, type);
            _monsterMovementType = type;
        }
    }
}