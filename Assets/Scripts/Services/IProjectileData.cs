using System;

namespace Services
{
    public interface IProjectileData<TType>
        where TType : Enum
    {
        TType Type { get; }
    }
}