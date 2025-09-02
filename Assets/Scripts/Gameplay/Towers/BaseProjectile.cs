using UnityEngine;

namespace Gameplay.Towers
{
    public abstract class BaseProjectile : MonoBehaviour
    {
        public float Speed { get; protected set; }
        public int Damage { get; protected set; }

        public abstract void Initialize(Vector3 targetPosition, float speed, int damage, float maxHeight = 0f);
    }
}