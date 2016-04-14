using UnityEngine;
using select;
using System.Collections.Generic;
using ui;

public class UpdateUnitIconList : MonoBehaviour
{

    public GameObject imageProto;
    private List<GameObject> _icons;

    void Start()
    {
        _icons = new List<GameObject>();
        Selection.instance.SelectionChanged += OnSelectionChanged;
        imageProto.SetActive(false);
    }

    public void OnDestroy()
    {
        Selection.instance.SelectionChanged -= OnSelectionChanged;
    }

    private void OnSelectionChanged(IEnumerable<ISelectable> objects)
    {
        _icons.ForEach(obj => Destroy(obj));
        _icons.Clear();
        foreach (GameObject unit in SelectionHelper.SelectablesToGameobjects(objects)) {
            GameObject icon = Instantiate(imageProto);
            icon.SetActive(true);
            icon.GetComponent<Icon>().Setup(gameObject, unit);
            icon.transform.localScale = Vector3.one;    //Seems that transform is copied from proto and saved
            _icons.Add(icon);
        }
    }
}
