using faction;
using rend.repaint;
using UnityEngine;
using utils;

namespace unit
{
    [ExecuteInEditMode]
    public class UnitRepaint : MonoBehaviour
    {
        void Start()
        {
            RepaintToFaction();
        }

        public void RepaintToFaction()
        {
            var data = Faction.GetFactionDataIfAny(gameObject);
            if (data.HasValue) Repaint(gameObject, data.Value.AsPaintData());
        }

        public static void Repaint(GameObject gameObject, IPaintData paintData)
        {
            foreach (var obj in gameObject.GetComponentsWithInterfaceInChildren<IRepaintable>(true))
            {
                obj.Repaint(paintData);
            }

            //TODO: is there a way to move this to UnitIcon?
            if (gameObject.GetComponentInChildren<UnitIcon>())
            {
                gameObject.GetComponentInChildren<UnitIcon>().UpdateIcon();
            }
        }        
    }
}