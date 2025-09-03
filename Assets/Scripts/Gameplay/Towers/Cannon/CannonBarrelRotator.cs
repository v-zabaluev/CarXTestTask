using UnityEngine;

namespace Gameplay.Towers.Cannon
{
    public class CannonBarrelRotator : MonoBehaviour
    {
        [SerializeField] private Transform _barrel;
        [SerializeField] private Transform _barrelStand;
        [SerializeField] private CannonTower _tower;

        [SerializeField] private float _turnSpeed = 5f;

        void Update()
        {
            if (_tower == null || _barrel == null || _barrelStand == null ||
                _tower.CannonType != CannonType.Cannon) return;

            Vector3 targetDir = _tower.ProjectileDirection;
           

            if (targetDir != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDir);

                _barrelStand.rotation = Quaternion.RotateTowards(_barrelStand.rotation, targetRotation,
                    _turnSpeed * Time.deltaTime * 100f);
                _barrel.localRotation = Quaternion.identity;
            }
        }
    }
}