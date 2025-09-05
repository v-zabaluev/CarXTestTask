using UnityEngine;

namespace Gameplay.Monsters.Movement.MovementVariants
{
    public class MonsterMovementLinear : MonsterMovementBase
    {
        [SerializeField] private float _speed = 10f;
        private Vector3 _targetPoint;
        private bool _isMoveToPoint;

        public override Vector3 GetSpeedVector()
        {
            var target = _isMoveToPoint  ? _targetPoint : _target.position;

            return (target - transform.position).normalized * _speed;
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

        protected override void Move()
        {
            var translation = (_target.transform.position - transform.position).normalized;
            transform.Translate(translation * _speed * Time.fixedDeltaTime);
        }

        private void MoveToPoint()
        {
            var translation = (_targetPoint - transform.position).normalized;
            transform.Translate(translation * _speed * Time.fixedDeltaTime);
        }

        public void StartMoveToPoint(Vector3 targetPosition)
        {
            _targetPoint = targetPosition;
            _target = null;
            _isMoveToPoint = true;
        }

        public void StopMoveToPoint()
        {
            _isMoveToPoint = false;
        }

        protected override void FixedUpdate()
        {
            if (_isMoveToPoint)
            {
                MoveToPoint();
            }

            base.FixedUpdate();
        }
    }
}