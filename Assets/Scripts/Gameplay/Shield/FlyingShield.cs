using Gameplay.Towers.Cannon;
using Gameplay.Towers.SimpleTower;
using UnityEngine;

namespace Gameplay
{
    public class FlyingShield : MonoBehaviour
    {
        [SerializeField] private float _angularSpeed = 250f;
        [SerializeField] private float _radius = 1.25f;
        [SerializeField] private Vector3 _localAxis = Vector3.up;
        [SerializeField] private Vector3 _lookDirection = Vector3.forward;
        [SerializeField] private Vector3 _offset = new Vector3(0, 1f, 0);
        [SerializeField] private ParticleSystem _sparklesVfx;

        private Transform _parent;
        private Vector3 _axis;
        private Vector3 _initialDirection;
        private float _angle = 0f;

        private void Start()
        {
            _parent = transform.parent;

            if (_parent == null)
            {
                Debug.LogError("FlyingShield: Родительский объект не назначен!");
                enabled = false;

                return;
            }

            if (_lookDirection.sqrMagnitude < 1e-6f)
            {
                Debug.LogError("FlyingShield: Направление поворота не может быть нулевым вектором!");
                enabled = false;

                return;
            }

            _lookDirection.Normalize();

            _sparklesVfx.Stop();

            _axis = _parent.TransformDirection(_localAxis);

            Vector3 perpendicular = Vector3.Cross(_axis, Vector3.up);

            if (perpendicular.sqrMagnitude < 1e-6f)
            {
                perpendicular = Vector3.Cross(_axis, Vector3.forward);
            }

            perpendicular.Normalize();
            _initialDirection = perpendicular;

            transform.position = _parent.position + _offset + _initialDirection * _radius;

            UpdateRotation();
        }

        private void Update()
        {
            _angle += _angularSpeed * Time.deltaTime;

            if (_angle >= 360f)
            {
                _angle -= 360f;
            }

            Quaternion rotation = Quaternion.AngleAxis(_angle, _axis);
            Vector3 currentDirection = rotation * _initialDirection;
            transform.position = _parent.position + _offset + currentDirection * _radius;

            UpdateRotation();
        }

        private void UpdateRotation()
        {
            Vector3 directionToParent = (_parent.position - transform.position).normalized;
            Quaternion baseRotation = Quaternion.LookRotation(directionToParent, _axis.normalized);
            Quaternion adjustment = Quaternion.FromToRotation(Vector3.forward, _lookDirection);
            transform.rotation = baseRotation * adjustment;
        }

        private void OnTriggerEnter(Collider other)
        {
            var guidedProjectile = other.gameObject.GetComponent<GuidedProjectile>();

            if (guidedProjectile != null)
            {
                Destroy(guidedProjectile.gameObject);
                _sparklesVfx.Play();

                return;
            }

            var cannonProjectile = other.gameObject.GetComponent<CannonProjectile>();

            if (cannonProjectile != null)
            {
                Destroy(cannonProjectile.gameObject);
                _sparklesVfx.Play();

                return;
            }
        }
    }
}