using UnityEngine;

namespace Gameplay
{
    public class TrajectoryCollisionPredictor
    {
        private readonly Collider _projectileCollider;
        private readonly Collider _shieldCollider;
        private readonly Transform _shieldTransform;
        private readonly float _timeStep;

        public TrajectoryCollisionPredictor(Collider projectileCollider, Collider shieldCollider, Transform shieldTransform, float timeStep = 0.02f)
        {
            _projectileCollider = projectileCollider;
            _shieldCollider = shieldCollider;
            _shieldTransform = shieldTransform;
            _timeStep = timeStep;
        }

        public bool WillCollide(Vector3 shooterPos, Vector3 projectileDir, float projectileSpeed, float maxTime,
            System.Func<float, Vector3> shieldPositionAtTime)
        {
            for (float t = 0; t <= maxTime; t += _timeStep)
            {
                Vector3 projectilePos = shooterPos + projectileDir * (projectileSpeed * t);

                Vector3 shieldPos = shieldPositionAtTime(t);

                if (Physics.ComputePenetration(
                        _projectileCollider, projectilePos, Quaternion.identity,
                        _shieldCollider, shieldPos, _shieldTransform.rotation,
                        out _, out _))
                {
                    return true;
                }
            }

            return false;
        }
    }}