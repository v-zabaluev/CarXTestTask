using Gameplay;
using Gameplay.Movement;
using UnityEngine;

public class MonsterMovementCircular : MonsterMovementBase
{
    [SerializeField] private float _angularSpeed = 90f;
    [SerializeField] private float _radius = 3f;
    [SerializeField] private float _tolerance = 0.2f;

    private float _angle;
    private float _initialY;
    public void Construct(Transform target)
    {
        _target = target;
        _initialY = transform.position.y;
        if (_target != null)
        {
            Vector3 offset = transform.position - _target.transform.position;
            _angle = Mathf.Atan2(offset.z, offset.x);
        }
    }

    protected override void Move()
    {
        if (_target == null) return;

        _angle += _angularSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime;

        float x = Mathf.Cos(_angle) * _radius;
        float z = Mathf.Sin(_angle) * _radius;
        transform.position = new Vector3(
            _target.transform.position.x + x,
            _initialY,
            _target.transform.position.z + z
        );
    }

    public override Vector3 GetSpeedVector()
    {
        float vx = -Mathf.Sin(_angle) * _radius * _angularSpeed * Mathf.Deg2Rad;
        float vz = Mathf.Cos(_angle) * _radius * _angularSpeed * Mathf.Deg2Rad;

        return new Vector3(vx, 0, vz);
    }

    public override bool CalculateIntercept(Vector3 shooterPos, float projectileSpeed,
        out Vector3 direction, out Vector3 interceptPoint)
    {
        direction = Vector3.zero;
        interceptPoint = Vector3.zero;

        return false;
    }
}