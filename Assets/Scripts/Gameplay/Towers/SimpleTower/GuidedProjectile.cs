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
            var monster = other.gameObject.GetComponent<Monster>();

            if (monster == null)
                return;

            monster.m_hp -= _damage;

            if (monster.m_hp <= 0)
            {
                Destroy(monster.gameObject);
            }

            Destroy(gameObject);
        }

        public void SetTarget(GameObject target)
        {
            _target = target;
        }
    }
}