using utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ui
{
    public class Icon : MonoBehaviour, IPointerClickHandler {

        [SerializeField]
        private UnityEventWithGameObject eventClicked;
        private GameObject unit;

        private void Update () {
            if (!unit) {
                return;
            }
            var damagable = unit.GetComponent<Damagable>();
            if (!damagable) {
                return;
            }

            //TODO: move to initialization
            var healthbar = transform.FindChild("HealthBar").gameObject.GetComponent<Image>();
            healthbar.fillAmount = damagable.Health / damagable.MaxHealth;
        }

        public void Setup(GameObject parent, GameObject unit) {
            this.unit = unit;
		
            var icon = unit.GetComponentInChildren<UnitIcon>();
            if (icon && icon.Texture) { 
                var rawImage = GetComponent<RawImage>();
                rawImage.texture = icon.Texture;
            }
            transform.SetParent(parent.transform);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            eventClicked.Invoke(unit);
        }
    }
}