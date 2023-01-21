using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// The RequireComponent attribute automatically adds required components as dependencies.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
        [SerializeField] protected float moveSpeed = 7.0f;
        [SerializeField] protected ParticleSystem landEffect;
        protected ParticleSystem.EmissionModule landEffectEmission;
        [SerializeField] protected ParticleSystem moveEffect;
        protected ParticleSystem.EmissionModule moveEffectEmission;
        protected float directionH;
    [Header("Jumping")]
        [SerializeField] protected float jumpForce = 10.0f;
        [SerializeField] protected float jumpPauseTime = 0.1f;
        protected float jumpCountdown; // To fix double jump from coyote/hang time
        [SerializeField] protected LayerMask groundLayerMask;
        [SerializeField] protected float groundCheckHeight = 0.4f;
        public bool isGrounded;
    [Header("Feel")]
        [SerializeField] protected float minGravityScale = 2.0f;
        [SerializeField] protected float maxGravityScale = 5.0f;
        [SerializeField] protected float playerMass = 4.0f;
        [SerializeField] protected float hangTime = 0.2f;
        public float hangCounter;

    protected AudioSource playerAudioSource;
    protected AudioClip damage;
    protected AudioClip jump;
    protected AudioClip appearing;

    protected Rigidbody2D playerRb2d;
    protected BoxCollider2D playerCollider2d;
    protected Animator playerAnimator;
    protected GameManager gameManager;
    protected bool facingRight;
    protected bool executeJump;
    protected bool executeCutJump;
    protected float timeInAir = 0.0f;
    protected float landRateOverTime = 200f;
    public bool isDead;

    protected virtual void Awake() {
        playerRb2d = GetComponent<Rigidbody2D>();
        playerCollider2d = GetComponent<BoxCollider2D>();
        playerAnimator = GetComponent<Animator>();
        playerAudioSource = GetComponent<AudioSource>();

        damage = Resources.Load<AudioClip>("Audio/CasualGameSounds/" + "damage");
        jump = Resources.Load<AudioClip>("Audio/CasualGameSounds/" + "jump");
        appearing = Resources.Load<AudioClip>("Audio/CasualGameSounds/" + "appearing");

        isDead = false;
    }

    // Start is called before the first frame update;
    protected virtual void Start() {
        gameManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<GameManager>();
        if(gameManager.userData.currentCheckpointLocation != null) {
            gameManager.LoadLevelProgress();
            transform.position = gameManager.userData.currentCheckpointLocation;
        }

        playerRb2d.mass = playerMass;
        moveEffectEmission = moveEffect.emission;
        landEffectEmission = landEffect.emission;
        facingRight = true;

        playerAudioSource.PlayOneShot(appearing);
    }

    // Update is called once per frame; Handle Input here
    protected virtual void Update() {
        // Hang Time
        if (isGrounded) {
            hangCounter = hangTime;
        } else {
            hangCounter -= Time.deltaTime;
        }

        if (!isDead) {        
            // Horizontal movement Input
            directionH = Input.GetAxisRaw("Horizontal");
            // Jumping Input
            if (Input.GetButtonDown("Jump") && hangCounter > 0) {
                executeJump = true;
            }
            // Stop jump on button release
            if (Input.GetButtonUp("Jump") && !isGrounded && playerRb2d.velocity.y > 0) {
                executeCutJump = true;
            }
        }

    }

    // Handle physics here;
    protected virtual void FixedUpdate() {
        if (jumpCountdown <= 0) {
            isGrounded = IsGrounded();
        } else {
            jumpCountdown -= Time.deltaTime;
        }

        if (!isDead) {
            // Do movement physics
            MoveHorizontal();
            if (executeJump) {
                playerAudioSource.PlayOneShot(jump);
                Jump();
                executeJump = false;
            }
            if (executeCutJump) {
                CutJump();
                executeCutJump = false;
            }
            SpeedUpFalling();

            // Show move effects & animation
            FixedUpdateEffects();
            FixedUpdateAnimations();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Play Landing effect
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            landEffect.Play();
        }
        if (collision.gameObject.CompareTag("DamageDealer")) {
            playerAudioSource.PlayOneShot(damage);
            Die();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Collectible")) {
            Collectible collectible = other.gameObject.GetComponent<Collectible>();

            collectible.StartAnimation();
            gameManager.userData.score += collectible.collectibleScore;
            Debug.Log("player controller: new userdata.score = " + gameManager.userData.score);
            GameObject.FindGameObjectWithTag("GameUI").GetComponent<GameUI>().UpdateScoreText(gameManager.userData.score);
        }
    }

    #region Animation&Effects
    protected void TurnAround(float horizontal) {
        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight)) {
            facingRight = !facingRight;
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
    }

    protected void FixedUpdateAnimations() {
        if (isGrounded) {
            playerAnimator.SetBool("grounded", true);
        } else {
            playerAnimator.SetBool("grounded", false);
        }
        playerAnimator.SetFloat("velocityY", playerRb2d.velocity.y);
        playerAnimator.SetFloat("speed", Mathf.Abs(directionH));
    }

    protected void FixedUpdateEffects() {
        if (isGrounded) {
            timeInAir = 0;
        }
        // Set landing particles based on air time
        if (!isGrounded && playerRb2d.velocity.y != 0) {
            timeInAir += Time.deltaTime;
            landEffectEmission.rateOverTime = landRateOverTime * timeInAir;
        }
        // Show moving particles
        moveEffectEmission.enabled = (directionH != 0) && isGrounded;
    }
    #endregion

    #region Death
    protected void Die() {
        isDead = true;
        playerRb2d.bodyType = RigidbodyType2D.Static;
        // Play death animation
        playerAnimator.SetTrigger("death");
    }

    // This is called from the player death anim now
    protected void RestartLevel() {
        gameManager.LoadLevelProgress();
        // Player gets respawn
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion

    #region GroundMovement&Jump
    protected virtual void Jump() {
        isGrounded = false;
        hangCounter = 0;
        jumpCountdown = jumpPauseTime;

        playerRb2d.velocity = new Vector2(playerRb2d.velocity.x, jumpForce);
    }

    protected void CutJump() {
        playerRb2d.velocity = new Vector2(playerRb2d.velocity.x, playerRb2d.velocity.y * 0.5f);
    }

    protected void MoveHorizontal() {
        TurnAround(directionH);
        playerRb2d.velocity = new Vector2(directionH * moveSpeed, playerRb2d.velocity.y);
    }

    protected void SpeedUpFalling() {
        // If not grounded and Y velocity <= 0 you are falling -> fall faster
        if(!isGrounded && playerRb2d.velocity.y <= 0) {
            playerRb2d.gravityScale = maxGravityScale;
        } else {
            playerRb2d.gravityScale = minGravityScale;
        }
    }

    private bool IsGrounded() {
        // Raycast for collision with "Ground" layer
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerCollider2d.bounds.center, playerCollider2d.bounds.size, 0f, Vector2.down, groundCheckHeight, groundLayerMask);
        
        // Debug the raycast
        Color rayColor;
        if(raycastHit.collider != null) {
            rayColor = Color.green;
        } else {
            rayColor = Color.red;
        }
        Debug.DrawRay(playerCollider2d.bounds.center + new Vector3(playerCollider2d.bounds.extents.x, 0), Vector2.down * (playerCollider2d.bounds.extents.y + groundCheckHeight), rayColor);
        Debug.DrawRay(playerCollider2d.bounds.center - new Vector3(playerCollider2d.bounds.extents.x, 0), Vector2.down * (playerCollider2d.bounds.extents.y + groundCheckHeight), rayColor);
        Debug.DrawRay(playerCollider2d.bounds.center - new Vector3(playerCollider2d.bounds.extents.x, playerCollider2d.bounds.extents.y + groundCheckHeight), Vector2.right * (playerCollider2d.bounds.extents.x), rayColor);
        // Debug.Log(raycastHit.collider);

        return raycastHit.collider != null;
    }
    #endregion
}
