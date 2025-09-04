using System;
using UnityEngine;

namespace Gameplay.Monsters
{
    public class MonsterHealth : MonoBehaviour
    {
        [SerializeField] private int _maxHP = 30;

        private int _currentHP;
        public Action OnDeath;

        private void Start()
        {
            _currentHP = _maxHP;
        }

        public void TakeDamage(int amount)
        {
            _currentHP -= amount;

            if (_currentHP <= 0)
            {
                Die();
            }

            Debug.Log($"Hit! Current HP: {_currentHP}, Damage: {amount}");
        }

        private void Die()
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}