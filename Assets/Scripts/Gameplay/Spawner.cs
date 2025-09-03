using System;
using System.Collections.Generic;
using Gameplay.Movement;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private MonsterMovementType _monsterMovementType;

        [SerializeField] private float _interval = 3;
        [SerializeField] private FlyingShield _flyingShieldPrefab;

        [Header("Monster Prefabs")]

        [SerializeField] private GameObject _monsterPrefab;

        [Header("For moving to target")]

        [SerializeField] private Transform _moveTarget;

        [Header("For moving around target")]

        [SerializeField] private Transform _orbitTarget;

        private float _lastSpawn = -1;
        private List<GameObject> _spawnedMonsters = new List<GameObject>();

        private void Start()
        {
            _lastSpawn = -_interval;
        }

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
            MonsterMovementController movementController = monster.GetComponent<MonsterMovementController>();
            
            movementController.SetTargetPoints(_moveTarget, _orbitTarget);
            
            movementController.SwitchMovementType(_monsterMovementType);
            monster.GetComponent<MonsterHealth>().OnDeath += () => { _spawnedMonsters.Remove(monster); };
            _spawnedMonsters.Add(monster);

            return monster;
        }

        public void SwitchMovementToAcceleration()
        {
            foreach (GameObject monster in _spawnedMonsters)
            {
                MonsterMovementController movementController = monster.GetComponent<MonsterMovementController>();
                movementController.SetTargetPoints(_moveTarget, _orbitTarget);
                movementController.SwitchMovementType(MonsterMovementType.Accelerated);
            }
        }
        
        public void SwitchMovementToLinear()
        {
            foreach (GameObject monster in _spawnedMonsters)
            {
                MonsterMovementController movementController = monster.GetComponent<MonsterMovementController>();
                movementController.SetTargetPoints(_moveTarget, _orbitTarget);
                movementController.SwitchMovementType(MonsterMovementType.Linear);
            }
        }
        
        public void SwitchMovementToCircular()
        {
            foreach (GameObject monster in _spawnedMonsters)
            {
                if (monster == null)
                {
                    _spawnedMonsters.Remove(monster);
                    return;
                }
                MonsterMovementController movementController = monster.GetComponent<MonsterMovementController>();
                movementController.SetTargetPoints(_moveTarget, _orbitTarget);
                movementController.SwitchMovementType(MonsterMovementType.Circular);
            }
        }
    }
}