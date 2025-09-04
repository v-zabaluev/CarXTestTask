using UnityEngine;

namespace Gameplay.Monsters.Movement
{
    public class MonsterMovementLinear : MonsterMovementBase
    {
        [SerializeField] private float _reachDistance = 0.3f;
        [SerializeField] private float _speed = 10f;
        public float Speed => _speed;
        protected override void Update()
        {
            base.Update();
            if (Vector3.Distance(transform.position, _target.transform.position) <= _reachDistance)
            {
                Destroy(gameObject);
            }
        }
        protected override void Move()
        {
            var translation = (_target.transform.position - transform.position).normalized;
            transform.Translate(translation * _speed * Time.fixedDeltaTime);
        }

        public override Vector3 GetSpeedVector()
        {
            return (_target.transform.position - transform.position).normalized * _speed;
        }
    
        public override bool CalculateIntercept(Vector3 shooterPos, float projectileSpeed,
            out Vector3 direction, out Vector3 interceptPoint)
        {
            Vector3 displacement = transform.position - shooterPos;
            Vector3 velocity = GetSpeedVector();

            float a = Vector3.Dot(velocity, velocity) - projectileSpeed * projectileSpeed;
            float b = 2f * Vector3.Dot(velocity, displacement);
            float c = Vector3.Dot(displacement, displacement);

            float discriminant = b * b - 4 * a * c;

            if (discriminant < 0f)
            {
                direction = Vector3.zero;
                interceptPoint = Vector3.zero;
                return false;
            }

            float sqrtD = Mathf.Sqrt(discriminant);
            float t1 = (-b - sqrtD) / (2 * a);
            float t2 = (-b + sqrtD) / (2 * a);

            float t = Mathf.Min(t1, t2);
            if (t < 0f) t = Mathf.Max(t1, t2);
            if (t < 0f)
            {
                direction = Vector3.zero;
                interceptPoint = Vector3.zero;
                return false;
            }

            interceptPoint = transform.position + velocity * t;
            direction = (interceptPoint - shooterPos).normalized;
            return true;
        }
    }
}