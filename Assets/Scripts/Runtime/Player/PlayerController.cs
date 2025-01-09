using System;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[Header("Camera")]
	[Space]

	public Camera cam;

	public CinemachineVirtualCamera virtualCamera;


	public float cameraSize;

	[Space]
	[Header("Movement")]
	[Space]

	public float moveSpeed;

	public float dragMultiplier;
	//public float groundDrag;
	//public float airDrag;

	public float jumpForce;
	public float jumpCooldown;
	public float airMultiplier;

	public AudioClip jumpAudioClip;
	[Range(0f, 1f)] public float jumpAudioClipVolume;

	private bool readyToJump;

	private float horizontalInput;

	private Vector3 moveDirection;

	[Space]
	[Header("Ground Collision Detection")]
	[Space]

	public float playerWidth;
	public float playerHeight;
	public LayerMask groundCheckLayerMask;
	public bool grounded;

	[Space]
	[Header("Projectile Throw")]
	[Space]

	public GameObject projectilePrefab;

	public float throwCooldown;

	public bool canThrow = true;

	[SerializeField] private int projectileCount;

	public AudioClip throwAudioClip;
	[Range(0f, 1f)] public float throwAudioClipVolume;

	private Vector2 mousePos;

	private SoundPlayer soundPlayer = new SoundPlayer();

	private InputActionMap gameplayInputActionMap;

	private PlayerInput playerInput;

	public int ProjectileCount
	{
		get
		{
			return projectileCount;
		}

		set
		{
			projectileCount = value;
			OnProjectileCountChanged();
		}
	}

	public TMP_Text projectileCountText;

	private Rigidbody2D rb;
	private Animator animator;

	private int direction = 1;

	private void OnEnable()
	{

		gameplayInputActionMap = GetComponent<PlayerInput>().actions.FindActionMap("Gameplay");
		gameplayInputActionMap.Enable();
	}

	private void OnDisable()
	{
		gameplayInputActionMap.Disable();
	}

	private void Start()
	{
		playerInput = GetComponent<PlayerInput>();

		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

		readyToJump = true;

		OnProjectileCountChanged();
		UpdateCamera();
	}

	private void Update()
	{
		GroundCheck();
		Inputs();
		SpeedControl();
		Visuals();

		//if (grounded)
		//{
		//    rb.drag = groundDrag;
		//}
		//else
		//{
		//    rb.drag = airDrag;
		//}
	}

	private void FixedUpdate()
	{
		Move();

		Vector2 dragVel = new Vector2(-rb.velocity.x, 0);
		rb.AddForce(dragVel * dragMultiplier, ForceMode2D.Impulse);
	}

	private void GroundCheck()
	{
		if (Physics2D.Raycast(transform.position, Vector2.down, playerHeight * 0.5f + 0.2f, groundCheckLayerMask) || Physics2D.Raycast(transform.position + new Vector3(playerWidth / 2, 0, 0), Vector2.down, playerHeight * 0.5f + 0.2f, groundCheckLayerMask) || Physics2D.Raycast(transform.position - new Vector3(playerWidth / 2, 0, 0), Vector2.down, playerHeight * 0.5f + 0.2f, groundCheckLayerMask))
		{
			grounded = true;
		}
		else
		{
			grounded = false;
		}
	}

	private void Inputs()
	{
		//horizontalInput = Input.GetAxisRaw("Horizontal");
		horizontalInput = gameplayInputActionMap.FindAction("Move").ReadValue<Vector2>().x;

		if (gameplayInputActionMap.FindAction("Jump").IsPressed() && readyToJump && grounded)
		{
			readyToJump = false;

			Jump();

			Invoke(nameof(ResetJump), jumpCooldown);
		}

		if (gameplayInputActionMap.FindAction("Fire").triggered && canThrow && ProjectileCount > 0)
		{
			ThrowProjectile();
		}

		if (playerInput.currentControlScheme == "Keyboard")
		{
			mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
		}
		else if (playerInput.currentControlScheme == "Gamepad")
		{
			Vector2 mouseOffset = gameplayInputActionMap.FindAction("Aim").ReadValue<Vector2>();
			mousePos = new Vector2(transform.position.x, transform.position.y) + mouseOffset;
		}
	}

	private void Move()
	{
		moveDirection = Vector3.right * horizontalInput;

		if (grounded)
			rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode2D.Force);
		else
			rb.AddForce(moveDirection * moveSpeed * 10f * airMultiplier, ForceMode2D.Force);
	}

	private void SpeedControl()
	{
		Vector2 flatVel = new Vector2(rb.velocity.x, 0f);

		if (flatVel.magnitude > moveSpeed)
		{
			Vector2 limitedVel = flatVel.normalized * moveSpeed;
			rb.velocity = new Vector2(limitedVel.x, rb.velocity.y);
		}
	}

	private void Jump()
	{
		rb.velocity = new Vector2(rb.velocity.x, 0f);

		rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

		soundPlayer.PlaySoundOnGameObject(gameObject, jumpAudioClip, jumpAudioClipVolume);
	}

	private void ThrowProjectile()
	{
		canThrow = false;

		ProjectileCount--;

		GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);

		Vector2 lookDir = mousePos - new Vector2(transform.position.x, transform.position.y);
		float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

		projectile.transform.rotation = Quaternion.Euler(0, 0, angle);

		soundPlayer.PlaySoundOnGameObject(gameObject, throwAudioClip, throwAudioClipVolume);

		Invoke(nameof(ResetThrow), throwCooldown);
	}

	private void ResetThrow()
	{
		canThrow = true;
	}

	private void Visuals()
	{
		if (grounded)
		{
			if (horizontalInput != 0)
			{
				animator.SetBool("Moving", true);
				animator.SetBool("Jumping", false);
				animator.SetBool("Falling", false);
			}
			else
			{
				animator.SetBool("Moving", false);
				animator.SetBool("Jumping", false);
				animator.SetBool("Falling", false);
			}
		}
		else
		{
			animator.SetBool("Moving", false);

			if (rb.velocity.y > 0)
			{
				animator.SetBool("Jumping", true);
				animator.SetBool("Falling", false);
			}
			else
			{
				animator.SetBool("Jumping", false);
				animator.SetBool("Falling", true);
			}
		}

		if (horizontalInput < 0)
		{
			direction = -1;
			GetComponent<SpriteRenderer>().flipX = true;
		}
		else if (horizontalInput > 0)
		{
			direction = 1;
			GetComponent<SpriteRenderer>().flipX = false;
		}
	}

	private void OnProjectileCountChanged()
	{
		projectileCountText.text = ProjectileCount.ToString();
	}

	private void ResetJump()
	{
		readyToJump = true;
	}

	private void UpdateCamera()
	{
		virtualCamera.m_Lens.OrthographicSize = cameraSize;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -(playerHeight / 2 + 0.2f), 0));
		Gizmos.DrawLine(transform.position + new Vector3(playerWidth / -2, 0, 0), transform.position + new Vector3(playerWidth / -2, -(playerHeight / 2 + 0.2f), 0));
		Gizmos.DrawLine(transform.position + new Vector3(playerWidth / 2, 0, 0), transform.position + new Vector3(playerWidth / 2, -(playerHeight / 2 + 0.2f), 0));
	}
}
