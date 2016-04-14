using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//public delegate void ModeChangedHandler(input.IMode newMode);

public class MouseInput : MonoBehaviour {

	public static MouseInput instance { get; private set; }

	void Awake() {
		instance = this;
	}

	// Use this for initialization
	void Start () {
	}


	// Update is called once per frame
	void Update () {
	}


}
