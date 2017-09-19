using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsEntity : MonoBehaviour {

    public float gravityModifier = 1.0f;
	public float minGroundNormalY = .65f;

	protected Vector2 _targetVelocity;
    protected Rigidbody2D _rigidbody;
    protected Vector2 _velocity;
    protected Vector2 _deltaPosition;
    protected ContactFilter2D _contactFilter;
    protected RaycastHit2D[] _hitBuffer;
    protected List<RaycastHit2D> _hitBufferList;
	protected bool _isGrounded;
	protected bool _yMovement;
	protected Vector2 _groundNormal;

    protected const float _minMoveDistance = 0.001f;
    protected const float _shellRadius = 0.01f;

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _hitBufferList = new List<RaycastHit2D>(16);
		_hitBuffer = new RaycastHit2D[16];
    }

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay (transform.position, _velocity);
	}

    private void Start()
    {
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        _contactFilter.useLayerMask = true;
		_yMovement = true;
    }

    private void FixedUpdate()
    {
		_isGrounded = false;

        _velocity += gravityModifier * Physics2D.gravity * Time.fixedDeltaTime;
		_velocity.x = _targetVelocity.x;

		Vector2 moveAlongGround = new Vector2 (_groundNormal.y, -_groundNormal.x);

        _deltaPosition = _velocity * Time.fixedDeltaTime;
        Vector2 movement = Vector2.up * _deltaPosition.y;
		movement = 

		Move(movement, _yMovement);
    }

	private void Move(Vector2 movement, bool yMovement)
    {
        float distance = movement.magnitude;

        if (distance > _minMoveDistance)
        {
            int count = _rigidbody.Cast(movement, _contactFilter, _hitBuffer, distance + _shellRadius);

			_hitBufferList.Clear ();

			for (int i = 0; i < count; i++)
			{
				_hitBufferList.Add (_hitBuffer [i]);
			}

			for (int i = 0; i < _hitBufferList.Count; i++) 
			{
				Vector2 currentNormal = _hitBufferList [i].normal;
				Debug.DrawLine (currentNormal, currentNormal * 3); 
				if (currentNormal.y > minGroundNormalY)
				{
					_isGrounded = true;

					if (yMovement) 
					{
						_groundNormal = currentNormal;
						currentNormal.x = 0;
					}
				}

				float projection = Vector2.Dot (_velocity, currentNormal);

				Debug.Log (projection.ToString ());


				if (projection < 0) {
					_velocity = _velocity - projection * currentNormal;
				}

				float modifiedDistance = _hitBufferList [i].distance - _shellRadius;
				distance = modifiedDistance < distance ? modifiedDistance : distance;
			}
        }

		_rigidbody.position = _rigidbody.position +  movement.normalized * distance;
    }
}