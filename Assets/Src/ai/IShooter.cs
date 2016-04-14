using UnityEngine;

namespace ai
{
    public interface IShooter
    {
        wpn.ITarget Target { get; set; }
        bool CanHit(wpn.ITarget target);
        Vector3 Position { get; }
        float Range { get; }
    }
}
