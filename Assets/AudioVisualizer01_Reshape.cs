using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualizer01_Reshape : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = new Vector3 (MicInput.MicLoudness, MicInput.MicLoudness, MicInput.MicLoudness);
	}
}
