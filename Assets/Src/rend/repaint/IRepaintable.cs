using UnityEngine;

namespace rend.repaint
{
    public interface IPaintData
    {
        Color Color { get; }
        Texture Texture { get; }
    }

    public interface IRepaintable
    {
        void Repaint(IPaintData paintData);
    }
}
