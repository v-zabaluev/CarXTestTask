using UnityEngine;

public abstract class MonsterMovementBase : MonoBehaviour
{
    protected Transform _target;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    protected virtual void Update()
    {
        if (_target == null)
            return;
    }

    protected abstract void Move();

    private void FixedUpdate()
    {
        if (_target == null)
            return;

        Move();
    }
    
    public abstract Vector3 GetSpeedVector();
    public abstract bool CalculateIntercept(Vector3 shooterPos, float projectileSpeed, 
        out Vector3 direction, out Vector3 interceptPoint);
}