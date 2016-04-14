using fow;
using rend.repaint;
using UnityEngine;

namespace unit
{
    [RequireComponent(typeof(BoxCollider))]
    class AutoMinimapMesh: MonoBehaviour
    {
        private const string SHADER_NAME = "Unlit/Color";
        private const string NEW_OBJECT_NAME = "AutoMinimapMesh";

        private void Awake()
        {
            CreateMinimapMesh();
        }

        private void CreateMinimapMesh()
        {
            var bbox = GetComponent<BoxCollider>();
            var newObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            InitTransform(newObj.transform, bbox);
            newObj.GetComponent<Renderer>().material = new Material(Shader.Find(SHADER_NAME));
            newObj.AddComponent<RepaintColor>();
            newObj.AddComponent<ApplyVisibilitySelf>();
            newObj.name = NEW_OBJECT_NAME;
            newObj.layer = LayersInt.Minimap;
        }

        private void InitTransform(Transform newTransform, BoxCollider bbox)
        {
            newTransform.SetParent(gameObject.transform, false);
            newTransform.localPosition = bbox.center;
            newTransform.localScale = bbox.size;
        }        
    }
}
