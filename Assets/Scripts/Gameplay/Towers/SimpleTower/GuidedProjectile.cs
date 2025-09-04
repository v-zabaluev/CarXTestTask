using Gameplay.Monsters;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Towers.SimpleTower
{
    public class GuidedProjectile : BaseProjectile<GuidedProjectile>
    {
        private Transform _target;

        public override void Initialize(float speed, int damage, float maxHeight = 0f)
        {
            _initialized = true;
            Speed = speed;
            Damage = damage;
            StartCoroutine(StartDestroyProcess());
        }

        public void SetTargetTransform(Transform targetTransform)
        {
            _target = targetTransform;
        }

        private void Update()
        {
            if (!_initialized) return;

            if (_target == null)
            {
                DespawnProjectile();
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, _target.position, Speed * Time.deltaTime);
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