using UnityEngine;

namespace Gameplay
{
using UnityEngine;
using System;

namespace Gameplay
{
    public class FlyingShieldProtection : MonoBehaviour
    {
        [SerializeField] private Collider _shieldCollider;
        [SerializeField] private Transform _shielded;

        /// <summary>
        /// Проверяет, будет ли щит блокировать снаряд
        /// </summary>
        /// <param name="projectileCollider">коллайдер снаряда</param>
        /// <param name="shooterPos">позиция башни</param>
        /// <param name="projectileDir">направление движения снаряда</param>
        /// <param name="projectileSpeed">скорость снаряда</param>
        /// <param name="currentAngle">текущий угол щита вокруг объекта</param>
        /// <param name="angularSpeed">скорость вращения щита (deg/s)</param>
        /// <param name="radius">радиус вращения щита</param>
        /// <param name="shieldOffsetPosition">смещение щита относительно родителя</param>
        /// <param name="maxTime">макс. время полета снаряда для проверки</param>
       
        public bool WillBlockIntercept(Collider projectileCollider, Vector3 shooterPos, Vector3 projectileDir,
            float projectileSpeed, float currentAngle, float angularSpeed, float radius, Vector3 shieldOffsetPosition,
            float maxTime = 6f)
        {
            if (_shieldCollider == null || projectileCollider == null)
                return false;

            var predictor = new TrajectoryCollisionPredictor(projectileCollider, _shieldCollider, transform);

            return predictor.WillCollide(shooterPos, projectileDir, projectileSpeed, maxTime, t =>
            {
                // положение щита в момент времени t
                float angle = currentAngle + angularSpeed * Mathf.Deg2Rad * t;
                Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

                return _shielded.position + shieldOffsetPosition + offset;
            });
        }
    }
}
}