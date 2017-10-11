using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display : MonoBehaviour {

	private PlayerActor m_player;
	private Text m_text;

	// Use this for initialization
	void Start () {
		m_player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerActor>();
		m_text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		m_text.text = "Gravity: " + m_player.m_gravity + "    " + "Jumpspeed: " + m_player.m_jumpSpeed + ". ";
		
	}
}
