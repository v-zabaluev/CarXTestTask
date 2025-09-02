using UnityEngine;
using System.Collections;

public class CannonTower : MonoBehaviour
{
    public float m_shootInterval = 0.5f;
    public float m_range = 4f;
    public GameObject m_projectilePrefab;
    public Transform m_shootPoint;

    private float m_lastShotTime = -0.5f;

    void Update()
    {
        if (m_projectilePrefab == null || m_shootPoint == null)
            return;

        foreach (var monster in FindObjectsOfType<Monster>())
        {
            if (Vector3.Distance(transform.position, monster.transform.position) > m_range)
                continue;

            if (m_lastShotTime + m_shootInterval > Time.time)
                continue;

            Vector3 targetVelocity = (monster.m_moveTarget.transform.position - monster.transform.position).normalized * monster.m_speed;

            float distance = Vector3.Distance(m_shootPoint.position, monster.transform.position);
            float t = distance / m_projectilePrefab.GetComponent<CannonProjectile>().m_speed;

            Vector3 predictedPos = monster.transform.position + targetVelocity * t;

            Vector3 direction = (predictedPos - m_shootPoint.position).normalized;

            Debug.DrawRay(m_shootPoint.position, direction * 5f, Color.red, 1f);
            Debug.DrawLine(monster.transform.position, predictedPos, Color.green, 1f);

            var projectile = Instantiate(m_projectilePrefab, m_shootPoint.position, Quaternion.LookRotation(direction));
            projectile.GetComponent<CannonProjectile>().SetDirection(direction);

            m_lastShotTime = Time.time;
        }
    }

    private Vector3 CalculateInterceptPoint(Vector3 shooterPos, float projectileSpeed, Vector3 targetPos,
        Vector3 targetVelocity)
    {
        Vector3 displacement = targetPos - shooterPos;
        float a = Vector3.Dot(targetVelocity, targetVelocity) - projectileSpeed * projectileSpeed;
        float b = 2f * Vector3.Dot(targetVelocity, displacement);
        float c = Vector3.Dot(displacement, displacement);

        float discriminant = b * b - 4f * a * c;

        if (discriminant < 0)
            return targetPos;

        float t1 = (-b + Mathf.Sqrt(discriminant)) / (2f * a);
        float t2 = (-b - Mathf.Sqrt(discriminant)) / (2f * a);

        float t = Mathf.Max(t1, t2);

        if (t < 0)
            return targetPos;

        return targetPos + targetVelocity * t;
    }
}