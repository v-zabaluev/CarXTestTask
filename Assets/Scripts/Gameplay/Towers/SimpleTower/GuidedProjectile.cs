using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Towers.SimpleTower
{
    public class GuidedProjectile : MonoBehaviour
    {
        [SerializeField] private GameObject _target;
        [SerializeField] private float _speed = 0.2f;
        [SerializeField] private int _damage = 10;

        private void Update()
        {
            if (_target == null)
            {
                Destroy(gameObject);

                return;
            }

            var translation = _target.transform.position - transform.position;

            if (translation.magnitude > _speed)
            {
                translation = translation.normalized * _speed;
            }

            transform.Translate(translation);
        }

        private void OnTriggerEnter(Collider other)
        {
            var monsterHealth = other.gameObject.GetComponent<MonsterHealth>();

            if (monsterHealth == null)
                return;

            Debug.Log("Hit!");

            monsterHealth.TakeDamage(_damage);

            Destroy(gameObject);
        }

        public void SetTarget(GameObject target)
        {
            _target = target;
        }
    }
}