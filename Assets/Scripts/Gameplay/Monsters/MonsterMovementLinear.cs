using UnityEngine;

public class MonsterMovementLinear : MonsterMovementBase
{
    [SerializeField] private float _speed = 10f;
    public float Speed => _speed;

    protected override void Move()
    {
        var translation = (_moveTarget.transform.position - transform.position).normalized;
        transform.Translate(translation * _speed * Time.fixedDeltaTime);
    }

    public override Vector3 GetSpeedVector()
    {
        return (_moveTarget.transform.position - transform.position).normalized * _speed;
    }
}