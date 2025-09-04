using Gameplay.Monsters;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Towers.SimpleTower
{
    public class GuidedProjectile : BaseProjectile<GuidedProjectile>
    {
        private Vector3 _targetPosition;

        public override void Initialize(Vector3 targetPosition, float speed, int damage, float maxHeight = 0f)
        {
            _initialized = true;
            _targetPosition = targetPosition;
            Speed = speed;
            Damage = damage;
            StartCoroutine(StartDestroyProcess());
        }

        private void Update()
        {
            if (!_initialized) return;

            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            var monsterHealth = other.gameObject.GetComponent<MonsterHealth>();

            if (monsterHealth == null) return;

            monsterHealth.TakeDamage(Damage);
            DespawnProjectile();
        }
    }
}