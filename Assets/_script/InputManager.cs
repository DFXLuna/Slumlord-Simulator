﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
	public GameObject c;
	// Allows different modes for input
	private enum context{ build, select};
	private context currentContext;
	// Hold camera's initial position
	private Vector3 cameraPos;

	// Use this for initialization
	void Start () {
		//cameraPos = 
		currentContext = context.select;
	}
	
	// Update is called once per frame
	void Update () {
		// Not sure if this is the best way to write this
		// Context Switcher/////////////////////////////////////////////////
		if(Input.GetKeyDown(KeyCode.S)){ currentContext = context.select; }
		else if(Input.GetKeyDown(KeyCode.B)){ currentContext = context.build; }
		// Build mode //////////////////////////////////////////////////////
		if(currentContext == context.build){
			// Build house
			if(Input.GetMouseButtonDown(0)){
				Ray mRay = 
					c.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
				RaycastHit info;
				if(Physics.Raycast(mRay, out info, 100)){
					if(info.collider.tag == "Grid"){
						GetComponent<HomeManager>().SpawnHome(info.collider.gameObject);
					}
				}
			}
		}
		// Select Mode /////////////////////////////////////////////////////
		else if(currentContext == context.select){
			// Focus/Unfocus
			if(Input.GetMouseButtonDown(0)){
				if(isCameraFocused()){ unfocus(); }
				else{
					Ray mRay = 
						c.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
					RaycastHit info;
					if(Physics.Raycast(mRay, out info, 100)){
						Transform child;
						if(info.collider.gameObject.FindChildwithTag("Home", out child)){
							focusOn(child);
						}
					}
				}
			}
		}
	}

	// Helper functions to simplify camera focus
	private bool isCameraFocused(){
		return c.GetComponent<CameraController>().isFocused();
	}

	private void focusOn(Transform child){
		c.GetComponent<CameraController>().focusOn(child);
	}

	private void unfocus(){
		c.GetComponent<CameraController>().unfocus();
	}

	void OnGUI(){
		// Context indicator
		string text = "";
		if(currentContext == context.build){ text = "Build"; }
		else if(currentContext == context.select){text = "Select"; }
		GUI.Box(new Rect(0, 0, 100, 25), text);
	}
}
