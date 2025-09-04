using System;
using System.Collections.Generic;
using Gameplay.Monsters.Movement;
using Gameplay.Monsters.Movement.MovementVariants;
using UnityEngine;

namespace Gameplay.Monsters.Spawn
{
    public class MonsterFactory
    {
        private readonly GameObject _monsterPrefab;
        private readonly Transform _moveTarget;
        private readonly Transform _orbitTarget;
        private readonly List<Transform> _pathCheckpoints;

        public MonsterFactory(GameObject prefab, Transform moveTarget, Transform orbitTarget, List<Transform> pathCheckpoints)
        {
            _monsterPrefab = prefab;
            _moveTarget = moveTarget;
            _orbitTarget = orbitTarget;
            _pathCheckpoints = pathCheckpoints;
        }

        public GameObject CreateMonster(Vector3 spawnPosition, Quaternion rotation, MonsterMovementType movementType, Action<GameObject> onDeathCallback)
        {
            GameObject monster = UnityEngine.Object.Instantiate(_monsterPrefab, spawnPosition, rotation);

            MonsterMovementController movementController = monster.GetComponent<MonsterMovementController>();
            movementController.SetTargetPoints(_moveTarget, _orbitTarget);

            var pathFollow = monster.GetComponent<MonsterMovementPathFollow>();
            if (pathFollow != null)
            {
                pathFollow.SetPath(_pathCheckpoints);
            }

            movementController.SwitchMovementType(movementType);

            MonsterHealth health = monster.GetComponent<MonsterHealth>();
            health.OnDeath += () => onDeathCallback(monster);

            return monster;
        }
    }
}