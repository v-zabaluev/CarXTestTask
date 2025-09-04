using System.Collections.Generic;
using System.Linq;
using Gameplay.Monsters.Movement.MovementVariants;
using UnityEngine;

namespace Gameplay.Monsters.Movement
{
    public class MonsterMovementManager
    {
        private readonly Transform _moveTarget;
        private readonly Transform _orbitTarget;
        private readonly List<Transform> _pathCheckpoints;

        public MonsterMovementManager(Transform moveTarget, Transform orbitTarget, List<Transform> pathCheckpoints)
        {
            _moveTarget = moveTarget;
            _orbitTarget = orbitTarget;
            _pathCheckpoints = pathCheckpoints;
        }

        public void SwitchMovement(List<GameObject> monsters, MonsterMovementType type)
        {
            foreach (var monster in monsters)
            {
                if (monster == null) continue;

                var movementController = monster.GetComponent<MonsterMovementController>();
                movementController.SetTargetPoints(_moveTarget, _orbitTarget);

                if (type == MonsterMovementType.PathFollow)
                {
                    var pathFollow = monster.GetComponent<MonsterMovementPathFollow>();
                    if (pathFollow != null)
                    {
                        pathFollow.SetPath(_pathCheckpoints);
                    }
                }

                movementController.SwitchMovementType(type);
            }
        }
    }
}