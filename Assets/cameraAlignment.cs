using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraAlignment : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.forward = Camera.main.transform.forward;
	}
}
