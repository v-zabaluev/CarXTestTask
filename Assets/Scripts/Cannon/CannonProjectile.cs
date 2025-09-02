using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class CannonProjectile : MonoBehaviour
{
    [SerializeField] private float _speed = 0.2f;
    [SerializeField] private int _damage = 10;

    [Header("Mortar settings")]
    [SerializeField] private float _maxHeight = 5f;

    private float _gravity;

    private CannonType _type;
    private Vector3 _velocity;
    private bool _initialized = false;

    public float Speed => _speed;

    public void Initialize(CannonType type, Vector3 targetPosition)
    {
        _type = type;

        if (_type == CannonType.Cannon)
        {
            _initialized = true;
            _velocity = transform.forward * _speed;
        }
        else if (_type == CannonType.Mortar)
        {
            Vector3 displacement = targetPosition - transform.position;

            Vector3 horizontalDisplacement = new Vector3(displacement.x, 0, displacement.z);

            float time = horizontalDisplacement.magnitude / _speed;

            float startVy = 2 * (_maxHeight - transform.position.y) / (time / 2); 
            _gravity = 2 * (startVy * time - displacement.y) / (time * time);

            Vector3 horizontalVelocity = horizontalDisplacement / time;

            _velocity = horizontalVelocity + Vector3.up * startVy;

            _initialized = true;
        }
    }

    private void FixedUpdate()
    {
        if (!_initialized)
            return;

        switch (_type)
        {
            case CannonType.Cannon:
                transform.position += transform.forward * _speed * Time.fixedDeltaTime;;
                break;

            case CannonType.Mortar:
                _velocity += Vector3.down * _gravity * Time.fixedDeltaTime;
                transform.position += _velocity * Time.fixedDeltaTime;
                transform.rotation = Quaternion.LookRotation(_velocity);
                break;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        var monster = other.gameObject.GetComponent<Monster>();

        if (monster == null)
            return;

        Debug.Log("Hit!");
        monster.m_hp -= _damage;

        if (monster.m_hp <= 0)
        {
            Destroy(monster.gameObject);
        }

        Destroy(gameObject);
    }
}