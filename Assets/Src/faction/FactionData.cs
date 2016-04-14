using System;
using rend.repaint;
using UnityEngine;

namespace faction
{
    public class FactionData : MonoBehaviour
    {
        [Serializable]
        public class FactionId
        {
            public static readonly FactionId NoFaction = new FactionId() { id = -1 };

            public int id;  //TODO: make readonly

            // override object.Equals
            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType()) {
                    return false;
                }
                return id == ((FactionId)obj).id;
            }

            // override object.GetHashCode
            public override int GetHashCode()
            {
                return id;
            }

            public bool IsSameFaction(FactionId other)
            {
                return id == other.id;
            }

            public bool HasFactionData()
            {
                return Instance != null;
            }

            public Data GetFactionData()
            {
                return Instance.factions[id];
            }
        }

        [Serializable]
        public struct Data
        {
            public string Name;
            public Color Color;
            public Texture UnitTexture;
            public Texture UnitDestroyedTexture;

            public IPaintData AsPaintData()
            {
                return new PaintDataAdapter(Color, UnitTexture);
            }

            public IPaintData AsDestroyedPaintData()
            {
                return new PaintDataAdapter(Color, UnitDestroyedTexture);
            }

            private class PaintDataAdapter : IPaintData
            {
                public PaintDataAdapter(Color color, Texture texture)
                {
                    Color = color;
                    Texture = texture;
                }

                public Color Color { get; private set; }
                public Texture Texture { get; private set; }
            }
        }

        [SerializeField]
        private Data[] factions = null;

        private static FactionData instance;

        public static FactionData Instance {
            get { return instance ?? (instance = FindObjectOfType<FactionData>()); }
        }

        private void OnDestroy()
        {
            instance = null;
        }

        public GUIContent[] FactionDataForGUI()
        {
            GUIContent[] rez = new GUIContent[factions.Length];
            for (int i = 0; i < factions.Length; i++) {
                rez[i] = new GUIContent(factions[i].Name, factions[i].UnitTexture);
            }
            return rez;
        }
    }
}
