using UnityEngine;

public class UnitIcon : MonoBehaviour
{
    private const string SINGLE_OBJECT_LAYER_NAME = "SingleObject";

    private Texture2D texture;

    private Camera MyCamera
    {
        get
        {
            return gameObject.GetComponent<Camera>();
        }
    }

    public Texture Texture
    {
        get
        {
            return texture;
        }
    }

    public void UpdateIcon()
    {
        RenderOnCamera();
        UpdateTextureFromCamera();
    }

    private void RenderOnCamera()
    {
        int backUpLayer = GetUnitGameObject().layer;
        ChangeChildLayerFromTo(backUpLayer, LayerMask.NameToLayer(SINGLE_OBJECT_LAYER_NAME));
        MyCamera.enabled = true;

        MyCamera.Render();

        MyCamera.enabled = false;
        ChangeChildLayerFromTo(LayerMask.NameToLayer(SINGLE_OBJECT_LAYER_NAME), backUpLayer);
    }

    private void UpdateTextureFromCamera()
    {
        texture = utils.RenderTextureUtil.CreateTextureFromRT(MyCamera.targetTexture);
    }

    // Use this for initialization
    void Start()
    {
        MyCamera.enabled = false;
    }

    private void ChangeChildLayerFromTo(int fromLayer, int toLayer)
    {
        GameObject parentUnit = GetUnitGameObject();
        foreach (var renderer in parentUnit.GetComponentsInChildren<Renderer>()) {
            if (renderer.gameObject.layer == fromLayer) {
                renderer.gameObject.layer = toLayer;
            }
        }
    }

    private GameObject GetUnitGameObject()
    {
        return transform.parent.gameObject; //TODO: not a best way...
    }
}
