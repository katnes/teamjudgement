﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErasePeasant : MonoBehaviour {

	//public GameObject peasant;
	public GameManager gameManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		//gameManager.peasant.SetActive (false);
	}
}
