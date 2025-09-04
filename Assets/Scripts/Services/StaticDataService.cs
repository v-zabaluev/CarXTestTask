using System;
using System.Collections;
using System.Collections.Generic;
using StaticData.Projectile;
using UnityEngine;

namespace Services
{
    public static class StaticDataService
    {
        private static readonly Dictionary<Type, IDictionary> _dataCache = new();

        public static void LoadAll()
        {
            LoadProjectileData<CannonProjectileData, CannonProjectileType>("StaticData/Projectiles/Cannon");
            LoadProjectileData<MortarProjectileData, MortarProjectileType>("StaticData/Projectiles/Mortar");
            LoadProjectileData<GuidedProjectileData, GuidedProjectileType>("StaticData/Projectiles/Guided");
        }

        private static void LoadProjectileData<TData, TType>(string path)
            where TData : ScriptableObject, IProjectileData<TType>
            where TType : Enum
        {
            TData[] dataArray = Resources.LoadAll<TData>(path);
            var dict = new Dictionary<TType, TData>();

            foreach (var data in dataArray)
            {
                dict[data.Type] = data;
            }

            _dataCache[typeof(TData)] = dict;
        }

        public static TData GetProjectile<TData, TType>(TType type)
            where TData : ScriptableObject, IProjectileData<TType>
            where TType : Enum
        {
            if (_dataCache.TryGetValue(typeof(TData), out var dictObj) && dictObj is Dictionary<TType, TData> dict)
            {
                return dict.TryGetValue(type, out var data) ? data : null;
            }

            return null;
        }
    }
}