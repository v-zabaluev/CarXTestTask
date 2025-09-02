using UnityEngine;

namespace Gameplay.Towers.Cannon
{
    public class CannonProjectile : BaseProjectile
    {
        private bool _initialized = false;

        public override void Initialize(Vector3 targetPosition, float speed, int damage, float maxHeight = 0f)
        {
            Speed = speed;
            Damage = damage;
            _initialized = true;
        }

        private void FixedUpdate()
        {
            if (!_initialized) return;
            transform.position += transform.forward * Speed * Time.fixedDeltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            var monsterHealth = other.gameObject.GetComponent<MonsterHealth>();
            if (monsterHealth == null) return;

            monsterHealth.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}