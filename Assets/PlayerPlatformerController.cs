using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsEntity
{
    public float jumpSpeed;
    public float maxSpeed;
    public float jumpDecreaseRate;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private bool isHurt;

    public bool IsHurt
    {
        get
        {
            return isHurt;
        }

        set
        {
            isHurt = value;
        }
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
        Vector2 movement = Vector2.zero;

        movement.x = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && _isGrounded) 
        {
            _velocity.y = jumpSpeed;
			_inAir = true;
        }
        else if (Input.GetButtonDown("Jump"))
        {
            if (_velocity.y > 0)
            {
				_velocity.y *= jumpDecreaseRate; 
            }
        }

        bool flipSprite = (_spriteRenderer.flipX ? (movement.x > 0.001f) : (movement.x < 0.001f));

        if (flipSprite)
        {
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
        }

        _animator.SetBool("grounded", _isGrounded);
        _animator.SetFloat("velocityX", Mathf.Abs(_velocity.x)/maxSpeed);
        _animator.SetBool("hurt", isHurt);

        _targetVelocity = movement * maxSpeed;
    }
}
