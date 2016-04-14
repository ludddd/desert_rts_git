using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Image))]
public class FilledImageColorChange : MonoBehaviour {

	public Gradient color;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.GetComponent<Image> ().color = color.Evaluate (gameObject.GetComponent<Image> ().fillAmount);
	}
}
