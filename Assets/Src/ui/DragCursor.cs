using UnityEngine;

public class DragCursor : MonoBehaviour {

    void Start () {
        gameObject.SetActive(false);
	}
	
	void Update () {
        if (!Input.GetMouseButton(0)) { 
            return;
        }
        transform.position = Input.mousePosition;
	}

    void OnEnable()
    {
        Update();
    }
}
