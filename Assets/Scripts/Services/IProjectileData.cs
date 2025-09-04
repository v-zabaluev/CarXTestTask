using System;

public interface IProjectileData<TType>
    where TType : Enum
{
    TType Type { get; }
}