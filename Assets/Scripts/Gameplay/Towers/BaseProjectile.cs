using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Towers
{
    public abstract class BaseProjectile<T> : MonoBehaviour, IProjectile where T : BaseProjectile<T>
    {
        protected bool _initialized = false;
        public float Speed { get; protected set; }
        public int Damage { get; protected set; }
        public Action<T> OnDespawn;

        public abstract void Initialize(float speed, int damage, float maxHeight = 0f);

        public void DespawnProjectile()
        {
            OnDespawn?.Invoke((T) this);
            OnDespawn = null;
            _initialized = false;
        }

        public IEnumerator StartDestroyProcess()
        {
            yield return new WaitForSeconds(Constants.MaxPredictionInterceptTime);
            DespawnProjectile();
        }
    }

    public interface IProjectile
    {
        public void DespawnProjectile();
    }
}