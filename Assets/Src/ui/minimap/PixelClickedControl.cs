using utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ui.minimap
{
    class PixelClickedControl : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private UnityEventWithVector2 pixelClickedEvent;

        public void OnPointerDown(PointerEventData eventData)
        {
            Vector2 localPos;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform,
                eventData.pressPosition,
                eventData.pressEventCamera, out localPos)) return;
            var imagePos = LocalPosToImagePixel(localPos, GetComponent<RawImage>());
            pixelClickedEvent.Invoke(imagePos);
        }

        private Vector2 LocalPosToImagePixel(Vector2 localPos, RawImage rawImage)
        {
            var rect = (RectTransform)transform;
            var pixelCoord = new Vector2((localPos.x - rect.rect.x) / rect.rect.width, (localPos.y - rect.rect.y) / rect.rect.height);
            pixelCoord = Vector2.Scale(pixelCoord, new Vector3(rawImage.texture.width, rawImage.texture.height));
            return pixelCoord;
        }
    }
}
