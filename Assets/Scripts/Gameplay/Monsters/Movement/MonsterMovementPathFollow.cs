using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Monsters.Movement
{
    public class MonsterMovementPathFollow : MonsterMovementBase
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _reachDistance = 0.2f;

        private List<Transform> _checkpoints;
        private int _currentIndex = 0;

        public float Speed => _speed;

        public void SetPath(List<Transform> checkpoints)
        {
            if (checkpoints == null || checkpoints.Count == 0)
            {
                Debug.LogError("Path is empty! Monster cannot move.");

                return;
            }

            _checkpoints = checkpoints;
            _currentIndex = 0;

            transform.position = _checkpoints[_currentIndex].position;
        }

        public override Vector3 GetSpeedVector()
        {
            if (_checkpoints == null || _checkpoints.Count == 0 || _currentIndex >= _checkpoints.Count)
                return Vector3.zero;

            var targetPoint = _checkpoints[_currentIndex];

            return (targetPoint.position - transform.position).normalized * _speed;
        }

        public override bool CalculateIntercept(Vector3 shooterPos, float projectileSpeed,
            out Vector3 direction, out Vector3 interceptPoint)
        {
            direction = Vector3.zero;
            interceptPoint = Vector3.zero;

            if (_checkpoints == null || _checkpoints.Count == 0 || _currentIndex >= _checkpoints.Count)
                return false;

            Vector3 currentPos = transform.position;
            Vector3 velocity = GetSpeedVector();
            int index = _currentIndex;

            while (index < _checkpoints.Count)
            {
                Vector3 targetPoint = _checkpoints[index].position;
                Vector3 segment = targetPoint - currentPos;
                float segmentLength = segment.magnitude;
                Vector3 segmentDir = segment.normalized;

                velocity = segmentDir * _speed;

                Vector3 displacement = currentPos - shooterPos;
                float a = Vector3.Dot(velocity, velocity) - projectileSpeed * projectileSpeed;
                float b = 2f * Vector3.Dot(velocity, displacement);
                float c = Vector3.Dot(displacement, displacement);

                float discriminant = b * b - 4 * a * c;

                if (discriminant >= 0f)
                {
                    float sqrtD = Mathf.Sqrt(discriminant);
                    float t1 = (-b - sqrtD) / (2 * a);
                    float t2 = (-b + sqrtD) / (2 * a);

                    float t = Mathf.Min(t1, t2);
                    if (t < 0f) t = Mathf.Max(t1, t2);

                    if (t >= 0f)
                    {
                        Vector3 predictedPos = currentPos + velocity * t;

                        float traveled = (predictedPos - currentPos).magnitude;

                        if (traveled <= segmentLength + 0.01f)
                        {
                            interceptPoint = predictedPos;
                            direction = (interceptPoint - shooterPos).normalized;

                            return true;
                        }
                    }
                }

                currentPos = targetPoint;
                index++;
            }

            return false;
        }

        protected override void Update()
        {
            base.Update();

            if (IsPathNull())
                return;

            var targetPoint = _checkpoints[_currentIndex];
            float dist = Vector3.Distance(transform.position, targetPoint.position);

            if (dist <= _reachDistance)
            {
                _currentIndex++;
            }

            if (CheckPathCompleted())
            {
                Destroy(gameObject);

                return;
            }

            Move();
        }

        protected override void Move()
        {
            if (_currentIndex >= _checkpoints.Count) return;

            var targetPoint = _checkpoints[_currentIndex];
            var dir = (targetPoint.position - transform.position).normalized;

            transform.Translate(dir * _speed * Time.deltaTime, Space.World);
        }

        private bool CheckPathCompleted()
        {
            if (_currentIndex >= _checkpoints.Count)
            {
                Destroy(gameObject);

                return true;
            }

            return false;
        }

        private bool IsPathNull()
        {
            if (_checkpoints == null || _checkpoints.Count == 0)
                return true;

            return false;
        }
    }
}