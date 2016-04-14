using System;
using UnityEngine;

namespace wpn
{
    public interface ITarget: IEquatable<ITarget>
    {
        Vector3 Pos { get; }
        bool IsValid { get; }
    }
}
