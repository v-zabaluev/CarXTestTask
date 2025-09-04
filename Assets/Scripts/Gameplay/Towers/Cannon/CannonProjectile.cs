using System;
using System.Collections;
using Gameplay.Monsters;
using Infrastructure.Pools;
using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay.Towers.Cannon
{
    public class CannonProjectile : BaseProjectile<CannonProjectile>
    {
        public override void Initialize(float speed,
            int damage, float maxHeight = 0f)
        {
            Speed = speed;
            Damage = damage;
            _initialized = true;
            
            StartCoroutine(StartDestroyProcess());
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
            DespawnProjectile();
        }

       
    }
}