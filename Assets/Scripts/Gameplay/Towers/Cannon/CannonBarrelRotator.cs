using UnityEngine;

namespace Gameplay.Towers.Cannon
{
    public class CannonBarrelRotator : MonoBehaviour
    {
        [SerializeField] private Transform _barrel;
        [SerializeField] private CannonTower _tower;

        [SerializeField] private float _turnSpeed = 5f;

        void Update()
        {
            if (_tower == null || _barrel == null) return;

            Vector3 targetDir = _tower.ProjectileDirection;

            if (targetDir != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDir);

                _barrel.rotation = Quaternion.RotateTowards(_barrel.rotation, targetRotation,
                    _turnSpeed * Time.deltaTime * 100f);
            }
        }
    }
}