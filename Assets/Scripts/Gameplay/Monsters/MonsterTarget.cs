using System;
using UnityEngine;

namespace Gameplay.Monsters
{
    public class MonsterTarget : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out MonsterHealth monsterHealth))
            {
                Destroy(other.gameObject);
            }
        }
    }
}