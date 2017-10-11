using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour {

	private struct RaycastOrigins
	{
		public Vector2 bottomLeft,bottomRight;
		public Vector2 topLeft, topRight;
	}

	public struct CollisionInfo
	{
		public bool above, down;
		public bool right, left;

		public void Reset()
		{
			above = down = false;
			right = left = false;
		}
	}

	public float skinWidth = 0.015f;
	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;
	public LayerMask collisionMask;
	public CollisionInfo collisionInfo;

	private RaycastOrigins m_raycastOrigins;
	private BoxCollider2D m_collider;
	private float m_horizontalRaySpacing;
	private float m_verticalRaySpacing;


	// Use this for initialization
	void Start ()
	{
		m_collider = GetComponent<BoxCollider2D> ();
		CalculateRaySpacing ();
	}

	public void Move(Vector3 velocity)
	{
		UpdateRayPosition ();
		collisionInfo.Reset ();

		if (velocity.x != 0) {
			HorizontalCollisions (ref velocity);
		}

		if (velocity.y != 0) {
			VerticalCollisions (ref velocity);
		}

		transform.Translate (velocity);
	}


	void HorizontalCollisions(ref Vector3 velocity)
	{
		//get the direction of the velocity vector and its length
		float directionX = Mathf.Sign (velocity.x);
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;

		for (int i = 0; i < horizontalRayCount; i++) 
		{
			//find out if he cast the ray from the left or from the right
			Vector2 rayOrigin = (directionX == -1) ? m_raycastOrigins.bottomLeft : m_raycastOrigins.bottomRight;

			//find out at which height to shoot the ray
			rayOrigin += Vector2.up * (m_horizontalRaySpacing * i);

			RaycastHit2D hit = Physics2D.Raycast (rayOrigin,Vector2.right * directionX, rayLength, collisionMask);

			Debug.DrawRay (rayOrigin, Vector2.right * directionX * rayLength, Color.red);

			if (hit) {
				velocity.x = (hit.distance - skinWidth) * directionX;
				rayLength = hit.distance;

				collisionInfo.right = directionX == 1;
				collisionInfo.left = directionX == -1;
			}
		}

	}

	void VerticalCollisions(ref Vector3 velocity)
	{
		//get the direction of the velocity vector and its length
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i++) 
		{
			//find out if he cast the ray from the left or from the right
			Vector2 rayOrigin = (directionY == -1) ? m_raycastOrigins.bottomLeft : m_raycastOrigins.topLeft;

			//find out at which height to shoot the ray
			rayOrigin += Vector2.right * (m_verticalRaySpacing * i + velocity.x);

			RaycastHit2D hit = Physics2D.Raycast (rayOrigin,Vector2.up * directionY, rayLength, collisionMask);

			Debug.DrawRay (rayOrigin, Vector2.up * directionY * rayLength, Color.blue);

			if (hit) {
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				collisionInfo.above = directionY == 1;
				collisionInfo.down = directionY == -1;
			}
		}

	}

	void UpdateRayPosition ()
	{
		//get the bounds
		Bounds bounds = m_collider.bounds;
		bounds.Expand (skinWidth * -2);

		//initialize the raycastorigin struct with the collider bounds
		m_raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		m_raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		m_raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		m_raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);

	}

	void CalculateRaySpacing()
	{
		Bounds bounds = m_collider.bounds;
		bounds.Expand (skinWidth * -2);

		horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);

		m_horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		m_verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);

	}

}
