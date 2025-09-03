using UnityEngine;

public class MonsterMovementAccelerated : MonsterMovementBase
{
    private const float CalculateInterceptTimeStep = 0.01f;
    private const float MaxPredictionInterceptTime = 6f; //Max flight time

    [SerializeField] private float _initialSpeed = 5f;
    [SerializeField] private float _acceleration = 2f;

    private float _currentSpeed;

    private void Start()
    {
        _currentSpeed = _initialSpeed;
    }

    protected override void Move()
    {
        _currentSpeed += _acceleration * Time.fixedDeltaTime;
        var translation = (_moveTarget.transform.position - transform.position).normalized;
        transform.Translate(translation * _currentSpeed * Time.fixedDeltaTime);
    }

    public override Vector3 GetSpeedVector()
    {
        return (_moveTarget.transform.position - transform.position).normalized * _currentSpeed;
    }

    public Vector3 GetAccelerationVector()
    {
        return (_moveTarget.transform.position - transform.position).normalized * _acceleration;
    }

    public override bool CalculateIntercept(Vector3 shooterPos, float projectileSpeed,
        out Vector3 direction, out Vector3 interceptPoint)
    {
        for (float t = CalculateInterceptTimeStep; t < MaxPredictionInterceptTime; t += CalculateInterceptTimeStep)
        {
            Vector3 predictedPos = transform.position +
                                   GetSpeedVector() * t +
                                   0.5f * GetAccelerationVector() * t * t;

            float dist = (predictedPos - shooterPos).magnitude;
            float projectileTime = dist / projectileSpeed;

            if (Mathf.Abs(projectileTime - t) <
                CalculateInterceptTimeStep * 2f) //2 is number of steps where could be a time of intersection
            {
                interceptPoint = predictedPos;
                direction = (interceptPoint - shooterPos).normalized;

                return true;
            }
        }

        direction = Vector3.zero;
        interceptPoint = Vector3.zero;

        return false;
    }
}