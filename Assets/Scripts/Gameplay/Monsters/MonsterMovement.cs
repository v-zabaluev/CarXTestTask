using UnityEngine;
using UnityEngine.Serialization;

public class MonsterMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _reachDistance = 0.3f;

    public float Speed => _speed;
    private GameObject _moveTarget;

    public void SetTarget(GameObject target)
    {
        _moveTarget = target;
    }
    private void Update()
    {
        if (_moveTarget == null)
            return;

        if (Vector3.Distance(transform.position, _moveTarget.transform.position) <= _reachDistance)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (_moveTarget == null)
            return;

        var translation = _moveTarget.transform.position - transform.position;

        if (translation.magnitude > _speed)
        {
            translation = translation.normalized * _speed;
        }

        transform.Translate(translation * Time.fixedDeltaTime);
    }

    public Vector3 GetSpeedVector()
    {
        return (_moveTarget.transform.position - transform.position).normalized * _speed;
    }
}