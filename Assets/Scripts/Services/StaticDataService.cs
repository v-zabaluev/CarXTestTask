using System.Collections.Generic;
using Gameplay.Towers.Cannon;
using Gameplay.Towers.SimpleTower;
using StaticData.Projectile;
using UnityEngine;

namespace Services
{
    public static class StaticDataService
    {
        private const string PathToMortar = "StaticData/Projectiles/Mortar";
        private const string PathToCannon = "StaticData/Projectiles/Cannon";
        private const string PathToGuided = "StaticData/Projectiles/Guided";

        private static Dictionary<CannonProjectileType, CannonProjectileData> _cannonProjectiles;
        private static Dictionary<MortarProjectileType, MortarProjectileData> _mortarProjectiles;
        private static Dictionary<GuidedProjectileType, GuidedProjectileData> _guidedProjectiles;

        public static void LoadAll()
        {
            _cannonProjectiles = new Dictionary<CannonProjectileType, CannonProjectileData>();
            _mortarProjectiles = new Dictionary<MortarProjectileType, MortarProjectileData>();
            _guidedProjectiles = new Dictionary<GuidedProjectileType, GuidedProjectileData>();

            CannonProjectileData[] cannons = Resources.LoadAll<CannonProjectileData>(PathToCannon);

            foreach (var cannon in cannons)
            {
                if (System.Enum.TryParse(cannon.name, out CannonProjectileType type))
                {
                    _cannonProjectiles[type] = cannon;
                }
            }

            MortarProjectileData[] mortars = Resources.LoadAll<MortarProjectileData>(PathToMortar);

            foreach (var mortar in mortars)
            {
                if (System.Enum.TryParse(mortar.name, out MortarProjectileType type))
                {
                    _mortarProjectiles[type] = mortar;
                }
            }

            GuidedProjectileData[] guideds = Resources.LoadAll<GuidedProjectileData>(PathToGuided);

            foreach (var guided in guideds)
            {
                if (System.Enum.TryParse(guided.name, out GuidedProjectileType type))
                {
                    _guidedProjectiles[type] = guided;
                }
            }
        }

        public static CannonProjectileData GetCannonProjectile(CannonProjectileType type)
        {
            return _cannonProjectiles.TryGetValue(type, out var data) ? data : null;
        }

        public static MortarProjectileData GetMortarProjectile(MortarProjectileType type)
        {
            return _mortarProjectiles.TryGetValue(type, out var data) ? data : null;
        }

        public static GuidedProjectileData GetGuidedProjectile(GuidedProjectileType type)
        {
            return _guidedProjectiles.TryGetValue(type, out var data) ? data : null;
        }
    }
}