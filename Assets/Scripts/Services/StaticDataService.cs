using System.Collections.Generic;
using StaticData.Projectile;
using UnityEngine;
using CannonProjectileData = Gameplay.Towers.Cannon.CannonProjectileData;

namespace Services
{
    public static class StaticDataService
    {
        private const string PathToMortar = "StaticData/Projectiles/Mortar";
        private const string PathToCannon = "StaticData/Projectiles/Cannon";
    
        private static Dictionary<CannonProjectileType, CannonProjectileData> _cannonProjectiles;
        private static Dictionary<MortarProjectileType, MortarProjectileData> _mortarProjectiles;
    
        public static void LoadAll()
        {
            _cannonProjectiles = new Dictionary<CannonProjectileType, CannonProjectileData>();
            _mortarProjectiles = new Dictionary<MortarProjectileType, MortarProjectileData>();

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
        }

        public static CannonProjectileData GetCannonProjectile(CannonProjectileType type)
        {
            return _cannonProjectiles.TryGetValue(type, out var data) ? data : null;
        }
    
        public static MortarProjectileData GetMortarProjectile(MortarProjectileType type)
        {
            return _mortarProjectiles.TryGetValue(type, out var data) ? data : null;
        }
    }
}