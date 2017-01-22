using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

    Camera m_Camera;

	// Use this for initialization
	void Start () {
        m_Camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {

        this.transform.LookAt(Camera.main.transform.position);
	}
}
