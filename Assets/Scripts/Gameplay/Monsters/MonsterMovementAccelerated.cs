using UnityEngine;

public class MonsterMovementAccelerated : MonsterMovementBase
{
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
}