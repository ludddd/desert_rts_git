using select;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class ActivateWhenObjectSelected : MonoBehaviour
{
	Selection.ChangedSelectionHandler handler;

	// Use this for initialization
	void Start ()
	{
		GetComponent<Selectable> ().interactable = false;
		handler = new Selection.ChangedSelectionHandler(OnChanged);
		Selection.instance.SelectionChanged += handler;
	}
	
	public void OnChanged(IEnumerable<ISelectable> selected) {
		GetComponent<Selectable> ().interactable = selected.Count() > 0;
	}

	void OnDestroy() {
		Selection.instance.SelectionChanged -= handler;
	}
}

