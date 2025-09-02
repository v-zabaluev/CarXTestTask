using UnityEngine;
using UnityEngine.Serialization;

public class MonsterHealth : MonoBehaviour
{
    [SerializeField] private int _maxHP = 30;

    private int _currentHP;

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
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}