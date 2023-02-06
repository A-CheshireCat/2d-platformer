using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaterController : PlayerController
{
    [Header("Water Mechanics")]
    [SerializeField] protected LayerMask waterLayerMask;
    [SerializeField] private float isWetTimer = 4.0f;
    protected AudioClip hitWater;

    //private
    public bool isInWater = false;
    public bool IsInWater {
        get { return isInWater; }
        set {
            if (isInWater != value) {
                isInWater = value;
                if (isInWater) {
                    playerAudioSource.PlayOneShot(hitWater);
                    SetWaterParameters();
                }
                else {
                    SetDefaultParameters();
                }
            }
        }
    }
    //private
    public bool canLaunchFromWater;
    public bool isWet;
    //private
    public float isWetCountDown;

    protected float defaultMoveSpeed;
    protected float defaultJumpForce;
    protected float defaultMinGravityScale;
    protected float defaultMaxGravityScale;
    protected float defaultPlayerMass;

    protected override void Awake() {
        base.Awake();

        hitWater = Resources.Load<AudioClip>("Audio/CasualGameSounds/" + "water_surface");
    }

    protected override void Start() {
        base.Start();
        // Save defaults
        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultMinGravityScale = minGravityScale;
        defaultMaxGravityScale = maxGravityScale;
        defaultPlayerMass = playerMass;
    }

    protected override void Update() {
        base.Update();

        //Make Jump a constant swim upward until button release
        if (Input.GetButton("Jump") && isInWater && canLaunchFromWater) {
            if (!playerAudioSource.isPlaying) { playerAudioSource.PlayOneShot(jump); }
            Jump();
        }
        if (Input.GetButtonUp("Jump") && isInWater && canLaunchFromWater) {
            canLaunchFromWater = false;
        }
    }
    
    protected override void FixedUpdate() {
        base.FixedUpdate();

        if (isGrounded && isInWater) {
            canLaunchFromWater = true;
        }

        if (isInWater) {
            isWet = true;
        }

        if (isWet) {
            GetComponent<SpriteRenderer>().color = Color.cyan;
        }

        if (!isInWater && isWet) {
            // Start wetness countdown
            if (isWetCountDown > 0) {
                isWetCountDown -= Time.deltaTime;
            } else {
                isWet = false;
                GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    protected void SetWaterParameters() {
        moveSpeed -= 3.0f;
        jumpForce -= 2.0f;
        minGravityScale = 2.0f;
        maxGravityScale = 3.0f;
        playerMass = 3.0f;
        Debug.Log("Water Params");
    }

    protected void SetDefaultParameters() {
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        minGravityScale = defaultMinGravityScale;
        maxGravityScale = defaultMaxGravityScale;
        playerMass = defaultPlayerMass;
        Debug.Log("Default Params");
    }

    protected override void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);
        if (other.gameObject.layer == LayerMask.NameToLayer("Water")) {
            IsInWater = true;
        }
    }

    protected void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water")) {
            // Get player's bottom center point
            Vector2 playerPoint = new Vector2(playerRb2d.position.x, playerRb2d.position.y - GetComponent<BoxCollider2D>().size.y * 0.5f);
            // Check if it is still in water(has trigger above) after it stops touching the trigger bounds
            bool hasWaterTriggerAbove = false;
            RaycastHit2D[] raycastHits = Physics2D.RaycastAll(playerPoint, Vector2.up);
           
            foreach (RaycastHit2D hit in raycastHits) {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Water")) {
                    hasWaterTriggerAbove = true;
                }
            }
            
            if (hasWaterTriggerAbove) {
                IsInWater = true;
            } else {
                IsInWater = false;
                canLaunchFromWater = false;
                isWetCountDown = isWetTimer;
            }
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision) {
        // When you bump your head on something, drop back down
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerCollider2d.bounds.center, playerCollider2d.bounds.size, 0f, Vector2.up);
        if(raycastHit.collider != null) {
            canLaunchFromWater = false;
        }
    }
}
