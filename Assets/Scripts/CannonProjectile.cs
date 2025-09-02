using UnityEngine;
using System.Collections;

public class CannonProjectile : MonoBehaviour
{
    public float m_speed = 0.2f;
    public int m_damage = 10;

    private Vector3 m_direction;

    public void SetDirection(Vector3 dir)
    {
        m_direction = dir.normalized;
    }

    void Update()
    {
        transform.position += m_direction * m_speed;
    }

    void OnTriggerEnter(Collider other)
    {
        var monster = other.gameObject.GetComponent<Monster>();

        if (monster == null)
            return;

        Debug.Log("Попал!");
        monster.m_hp -= m_damage;

        if (monster.m_hp <= 0)
        {
            Destroy(monster.gameObject);
        }

        Destroy(gameObject);
    }
}