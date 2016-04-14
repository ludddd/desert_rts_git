using UnityEngine;

namespace ui.input.visual
{
    public class SelectionRect : MonoBehaviour
    {

        public void setRect(Vector3 from, Vector3 to)
        {
            gameObject.SetActive(true);
            transform.position = (from + to) * 0.5f;
            transform.localScale = new Vector3(
                                                2.0f * Mathf.Abs(transform.position.x - from.x),
                                                transform.localScale.y,
                                                2.0f * Mathf.Abs(transform.position.z - from.z));

        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}