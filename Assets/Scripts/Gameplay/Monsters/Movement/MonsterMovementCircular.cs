using DefaultNamespace;
using UnityEngine;

namespace Gameplay.Monsters.Movement
{
    public class MonsterMovementCircular : MonsterMovementBase
    {
        private const float CalculateInterceptTimeStep = 0.01f;

        [SerializeField] private float _angularSpeed = 90f;
        [SerializeField] private float _radius = 3f;
        [SerializeField] private float _tolerance = 0.2f;

        private float _angle;
        private float _initialY;
        private Vector3 _targetXZ;

        public void Construct(Transform target)
        {
            _target = target;
            _initialY = transform.position.y;

            if (_target != null)
            {
                _targetXZ = new Vector3(_target.position.x, 0f, _target.position.z);

                Vector3 offset = new Vector3(
                    transform.position.x - _target.position.x,
                    0f,
                    transform.position.z - _target.position.z
                );

                _angle = Mathf.Atan2(offset.z, offset.x);
            }
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
            for (float t = CalculateInterceptTimeStep; t < Constants.MaxPredictionInterceptTime; t += CalculateInterceptTimeStep)
            {
                float futureAngle = _angle + _angularSpeed * Mathf.Deg2Rad * t;

                float x = Mathf.Cos(futureAngle) * _radius;
                float z = Mathf.Sin(futureAngle) * _radius;

                Vector3 predictedPos = new Vector3(
                    _targetXZ.x + x,
                    _initialY,
                    _targetXZ.z + z
                );

                float dist = (predictedPos - shooterPos).magnitude;
                float projectileTime = dist / projectileSpeed;

                if (Mathf.Abs(projectileTime - t) < CalculateInterceptTimeStep * 2)
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

        protected override void Move()
        {
            if (_target == null) return;

            _targetXZ.x = _target.position.x;
            _targetXZ.z = _target.position.z;

            _angle += _angularSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime;

            float x = Mathf.Cos(_angle) * _radius;
            float z = Mathf.Sin(_angle) * _radius;

            transform.position = new Vector3(
                _targetXZ.x + x,
                _initialY,
                _targetXZ.z + z
            );
        }
    }
}