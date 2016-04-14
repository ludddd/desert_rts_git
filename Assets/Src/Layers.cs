
using UnityEngine;

public static class Layers
{
    public const string Terrain = "Terrain";
    public const string Minimap = "Minimap";
    public const string Default = "Default";
}

public static class LayersInt
{
    public static readonly int Terrain = LayerMask.NameToLayer(Layers.Terrain);
    public static readonly int Minimap = LayerMask.NameToLayer(Layers.Minimap);
    public static readonly int Default = LayerMask.NameToLayer(Layers.Default);
}


