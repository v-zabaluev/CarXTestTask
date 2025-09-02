using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private float _interval = 3;
        public GameObject _moveTarget;
        [SerializeField] private FlyingShield _flyingShieldPrefab;
        [SerializeField] private GameObject _monsterPrefab;
        private float _lastSpawn = -1;

        private void Update()
        {
            if (Time.time > _lastSpawn + _interval)
            {
                // var newMonster = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                // var r = newMonster.AddComponent<Rigidbody>();
                // r.useGravity = false;
                // newMonster.transform.position = transform.position;
                // var monsterBeh = newMonster.AddComponent<Monster>();
                GameObject monster = Instantiate(_monsterPrefab, transform.position, Quaternion.identity);
                monster.GetComponent<MonsterMovement>().SetTarget(_moveTarget);

                if (_flyingShieldPrefab != null)
                    Instantiate(_flyingShieldPrefab, monster.transform);

                _lastSpawn = Time.time;
            }
        }
    }
}