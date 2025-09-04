using Gameplay.Towers;
using UnityEngine;

namespace Gameplay.Shield
{
    public class FlyingShieldCollision : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _sparklesVfx;

        private void Start()
        {
            if (_sparklesVfx != null)
                _sparklesVfx.Stop();
        }

        private void OnTriggerEnter(Collider other)
        {
            var projectile = other.GetComponent<IProjectile>();
            
            if (projectile != null)
            {
                projectile.DespawnProjectile();

                if (_sparklesVfx != null)
                    _sparklesVfx.Play();
            }
        }
    }
}