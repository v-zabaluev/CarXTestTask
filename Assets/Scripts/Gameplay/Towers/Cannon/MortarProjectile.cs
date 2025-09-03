using Infrastructure.Pools;
using UnityEngine;

namespace Gameplay.Towers.Cannon
{
    public class MortarProjectile : BaseProjectile<MortarProjectile>
    {
        private Vector3 _velocity;
        private float _gravity;
        private float _maxHeight;

        public Vector3 Velocity => _velocity;
        public override void Initialize(Vector3 targetPosition, float speed, int damage, float maxHeight = 0f)
        {
            Speed = speed;
            Damage = damage;
            _maxHeight = maxHeight;

            Vector3 displacement = targetPosition - transform.position;
            Vector3 horizontalDisplacement = new Vector3(displacement.x, 0, displacement.z);

            float time = horizontalDisplacement.magnitude / Speed;
            float startVy = 2 * (_maxHeight - transform.position.y) / (time / 2);
            _gravity = 2 * (startVy * time - displacement.y) / (time * time);
            Vector3 horizontalVelocity = horizontalDisplacement / time;

            _velocity = horizontalVelocity + Vector3.up * startVy;
            _initialized = true;
            StartCoroutine(StartDestroyProcess());
        }

        private void FixedUpdate()
        {
            if (!_initialized) return;

            _velocity += Vector3.down * _gravity * Time.fixedDeltaTime;
            transform.position += _velocity * Time.fixedDeltaTime;
            transform.rotation = Quaternion.LookRotation(_velocity);
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