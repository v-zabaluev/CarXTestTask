using UnityEngine;

namespace Gameplay.Towers.Cannon
{
    public abstract class BarrelRotatorBase : MonoBehaviour
    {
        [SerializeField] protected Transform _barrel;
        [SerializeField] protected Transform _barrelStand;
        [SerializeField] protected float _turnSpeed = 5f;

        public Transform BarrelStand => _barrelStand;
        public Transform Barrel => _barrel;

        public abstract void RotateTowards(Vector3 targetPoint, float projectileSpeed, float maxHeight);
    }
}