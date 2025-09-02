using UnityEngine;
using System.Collections;

public class CannonTower : MonoBehaviour
{
    public float m_shootInterval = 0.5f;
    public float m_range = 4f;
    public GameObject m_projectilePrefab;
    public Transform m_shootPoint;

    private float m_lastShotTime = -0.5f;
    private Vector3 _projectileDirection;
    public Vector3 ProjectileDirection => _projectileDirection;

    void Update()
    {
        if (m_projectilePrefab == null || m_shootPoint == null) return;

        var monster = FindNearestTarget();

        if (monster == null) return;

        float projectileSpeed = m_projectilePrefab.GetComponent<CannonProjectile>().m_speed;

        if (CalculateInterceptDirection(m_shootPoint.position, monster.transform.position,
                CalculateMonsterSpeedVector(monster), projectileSpeed, out _projectileDirection))
        {
            Debug.DrawRay(m_shootPoint.position, _projectileDirection * 100, Color.red, 2f);

            if (m_lastShotTime + m_shootInterval <= Time.time)
            {
                Shoot(_projectileDirection);
                m_lastShotTime = Time.time;
            }
        }
    }

    private Monster FindNearestTarget()
    {
        float closestDistance = m_range;
        Monster closest = null;

        foreach (var monster in FindObjectsOfType<Monster>())
        {
            float distance = Vector3.Distance(transform.position, monster.transform.position);

            if (distance <= closestDistance)
            {
                closestDistance = distance;
                closest = monster;
            }
        }

        return closest;
    }

    private void Shoot(Vector3 direction)
    {
        Instantiate(m_projectilePrefab, m_shootPoint.position, Quaternion.LookRotation(direction));
    }

    private bool CalculateInterceptDirection(Vector3 shooterPos, Vector3 monsterPos, Vector3 monsterVelocity,
        float projectileSpeed, out Vector3 direction)
    {
        Vector3 displacement = monsterPos - shooterPos;
        float a = Vector3.Dot(monsterVelocity, monsterVelocity) - projectileSpeed * projectileSpeed;
        float b = 2f * Vector3.Dot(monsterVelocity, displacement);
        float c = Vector3.Dot(displacement, displacement);

        float discriminant = b * b - 4 * a * c;

        if (discriminant < 0f)
        {
            direction = Vector3.zero;

            return false;
        }

        float sqrtD = Mathf.Sqrt(discriminant);
        float t1 = (-b - sqrtD) / (2 * a);
        float t2 = (-b + sqrtD) / (2 * a);

        float t = Mathf.Min(t1, t2);

        if (t < 0f)
            t = Mathf.Max(t1, t2);

        if (t < 0f)
        {
            direction = Vector3.zero;

            return false;
        }

        Vector3 interceptPoint = monsterPos + monsterVelocity * t;
        direction = (interceptPoint - shooterPos).normalized;

        return true;
    }

    private Vector3 CalculateMonsterSpeedVector(Monster monster)
    {
        if (monster.m_moveTarget == null)
            return Vector3.zero;

        Vector3 dir = (monster.m_moveTarget.transform.position - monster.transform.position).normalized;

        return dir * monster.m_speed;
    }
}