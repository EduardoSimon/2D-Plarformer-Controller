using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display : MonoBehaviour {

	private PlayerActor m_player;
	private Controller2D m_controller;
	private Text m_text;

	// Use this for initialization
	void Start () {
		m_player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerActor>();
		m_controller = GameObject.FindGameObjectWithTag ("Player").GetComponent<Controller2D> ();

		m_text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		m_text.text = m_player.velocity.ToString() +  ' ' + m_controller.collisionInfo.slopeAngle.ToString() + ' ' + 
			m_controller.collisionInfo.previousSlopeAngle.ToString() + ' ' + m_controller.collisionInfo.climbingSlope.ToString() + ' '+ 
			m_controller.collisionInfo.right.ToString() + m_controller.collisionInfo.left.ToString() + m_controller.collisionInfo.down.ToString()
			+ m_controller.collisionInfo.above.ToString();
	}
}
