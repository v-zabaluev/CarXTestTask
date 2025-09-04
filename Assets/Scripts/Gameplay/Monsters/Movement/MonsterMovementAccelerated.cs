using DefaultNamespace;
using UnityEngine;

namespace Gameplay.Monsters.Movement
{
    public class MonsterMovementAccelerated : MonsterMovementBase
    {
        private const float CalculateInterceptTimeStep = 0.01f;

        [SerializeField] private float _reachDistance = 0.3f;
        [SerializeField] private float _initialSpeed = 5f;
        [SerializeField] private float _acceleration = 2f;

        private float _currentSpeed;

        public override Vector3 GetSpeedVector()
        {
            return (_target.transform.position - transform.position).normalized * _currentSpeed;
        }

        public Vector3 GetAccelerationVector()
        {
            return (_target.transform.position - transform.position).normalized * _acceleration;
        }

        public override bool CalculateIntercept(Vector3 shooterPos, float projectileSpeed,
            out Vector3 direction, out Vector3 interceptPoint)
        {
            for (float t = CalculateInterceptTimeStep; t < Constants.MaxPredictionInterceptTime; t += CalculateInterceptTimeStep)
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

        private void Start()
        {
            _currentSpeed = _initialSpeed;
        }

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
            _currentSpeed += _acceleration * Time.fixedDeltaTime;
            var translation = (_target.transform.position - transform.position).normalized;
            transform.Translate(translation * _currentSpeed * Time.fixedDeltaTime);
        }
    }
}