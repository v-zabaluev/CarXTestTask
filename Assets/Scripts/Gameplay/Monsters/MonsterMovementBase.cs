using UnityEngine;

public abstract class MonsterMovementBase : MonoBehaviour
{
    [SerializeField] protected float _reachDistance = 0.3f;
    protected GameObject _moveTarget;

    public void SetTarget(GameObject target)
    {
        _moveTarget = target;
    }

    protected virtual void Update()
    {
        if (_moveTarget == null)
            return;

        if (Vector3.Distance(transform.position, _moveTarget.transform.position) <= _reachDistance)
        {
            Destroy(gameObject);
        }
    }

    protected abstract void Move();

    private void FixedUpdate()
    {
        if (_moveTarget == null)
            return;

        Move();
    }
    
    public abstract Vector3 GetSpeedVector();
}