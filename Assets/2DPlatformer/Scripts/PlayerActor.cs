using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActor : MonoBehaviour {

	public float movSpeed = 6.0f;
	public float jumpHeight = 4.0f;
	public float timeToGetToApex = 0.4f;

	public float m_gravity;
	public float m_jumpSpeed;
	private float m_accelerationTimeAirborne = .2f;
	private float m_accelerationTimeGrounded = .1f;
	private float m_velocityXSmoothing;
	private float m_targetXVelocity;
	private Vector3 m_velocity;
	private Controller2D m_controller;

	// Use this for initialization
	void Start () {
		m_controller = GetComponent<Controller2D> ();

		m_gravity = -(2 * jumpHeight) / Mathf.Pow (timeToGetToApex, 2);
		m_jumpSpeed = Mathf.Abs (m_gravity) * timeToGetToApex;
	}
	
	// Update is called once per frame
	void Update () {

		//if theres a ground dont add vertical velocity
		if (m_controller.collisionInfo.above || m_controller.collisionInfo.down) {
			m_velocity.y = 0;
		}

		//Get the 2D input
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		#if UNITY_EDITOR
		if ((Input.GetKeyDown(KeyCode.Space)) && m_controller.collisionInfo.down)
		{
			m_velocity.y = m_jumpSpeed;
		}

		#elif UNITY_ANDROID
		if (Input.touchCount > 0) {
			if (Input.GetTouch(0).phase == TouchPhase.Began) {
				m_velocity.y = jumpSpeed;
			}
		}
		#endif

		//m_velocity.x = input.x * movSpeed;
		m_targetXVelocity = input.x * movSpeed;
		m_velocity.x = Mathf.SmoothDamp (m_velocity.x, m_targetXVelocity,ref m_velocityXSmoothing, m_controller.collisionInfo.above ? m_accelerationTimeGrounded : m_accelerationTimeAirborne);
		m_velocity.y += m_gravity * Time.deltaTime;
		m_controller.Move(m_velocity * Time.deltaTime);
	}
}
