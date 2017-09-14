using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsEntity : MonoBehaviour {

    public float gravityModifier = 1.0f;

    protected Rigidbody2D _rigidbody;
    protected Vector2 _velocity;
    protected Vector2 _deltaPosition;
    protected ContactFilter2D _contactFilter;
    protected RaycastHit2D[] _hitBuffer;
    protected List<RaycastHit2D> _hitBufferList;

    protected const float _minMoveDistance = 0.001f;
    protected const float _shellRadius = 0.01f;

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _hitBufferList = new List<RaycastHit2D>(16);
    }

    private void Start()
    {
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        _contactFilter.useLayerMask = true;
    }

    private void FixedUpdate()
    {
        _velocity += gravityModifier * Physics2D.gravity * Time.fixedDeltaTime;
        _deltaPosition = _velocity * Time.fixedDeltaTime;
        Vector2 movement = Vector2.up * _deltaPosition.y;

        Move(movement);
    }

    private void Move(Vector2 movement)
    {
        float distance = movement.magnitude;

        if (distance > _minMoveDistance)
        {
            int count = _rigidbody.Cast(movement, _contactFilter, _hitBuffer, distance + _shellRadius);
        }
        _rigidbody.position += movement;
    }
}