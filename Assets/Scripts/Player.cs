using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float jumpForce;
    public float speed;
    private Rigidbody2D body;
    public BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer;
    public bool wateringCan;
    public bool canJump;
    public float jumpTime;
    public float jumpTimeCounter;
    private bool isJumping;

    public enum State
    { 
        Standard,
        HitStun,
        NoMove,
        Refill,
        Pour,
        Float,
    
    }
    public State state;

    public Animator anim;
    private float hitStunTime;
    private float iFrameTimer;
    public bool iFrames;
    public float water;
    public Sink sink;
    public Plant plant;
    public SpriteRenderer shadow;
    public SpriteRenderer waterBubble;
    private LevelLoader loader;
    private bool sceneTransition = false;

    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip hurtSound;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        waterBubble.enabled = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loader = LevelLoader.FindAnyObjectByType<LevelLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Standard:
                Movement();
                break;
            case State.NoMove:
                break;
            case State.Refill:
                Refill();
                break;
            case State.Pour:
                Pour();
                break;
            case State.HitStun:
                HitStun();
                break;
            case State.Float:
                Float();
                break;
        }
    }

    private void Movement()
    {
        anim.SetInteger("react", 0);
        anim.SetBool("water", wateringCan);
        anim.SetBool("refill", false);
        anim.SetBool("pour", false);
        hitStunTime = 0;

        float horizontalInput = Input.GetAxis("Horizontal");
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        //Jumping Code
        if (canJump)
        {
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                SoundManager.instance.PlaySound(jumpSound);
                isJumping = true;
                jumpTimeCounter = jumpTime;
                JumpForceMethod();
            }

            if (Input.GetKey(KeyCode.Space) && isJumping == true)
            {
                if (jumpTimeCounter > 0)
                {
                    JumpForceMethod();
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                isJumping = false;
            }
        }


        if (IsGrounded() && body.linearVelocity.y == 0)
        {
            body.gravityScale = 1.5f;
            shadow.enabled = true;
        }
        if (!IsGrounded())
        {
            shadow.enabled = false;
        }
        if (!IsGrounded() && body.linearVelocity.y <= 0)
        {
            body.gravityScale = 2;
        }

        iFrameTimer -= Time.deltaTime;
        if (iFrameTimer < 0)
        {
            iFrames = false;
        }

        if (iFrames)
        {
            GetComponent<SpriteRenderer>().color = Color.gray;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            Physics2D.IgnoreLayerCollision(6, 7, false);
        }

        //Flip Sprite
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }

        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        anim.SetBool("move", horizontalInput != 0);
        anim.SetBool("grounded", IsGrounded());
        anim.SetBool("falling", IsFalling());
    }

    private void Refill()
    {
        body.linearVelocity = new Vector2(0, 0);
        anim.SetBool("refill", true);
        sink.state = Sink.State.On;

        if (Input.GetKeyUp(KeyCode.Space) || water >= 100)
        {
            state = State.Standard;
            sink.state = Sink.State.Off;
            waterBubble.enabled = false;
        }
    }

    private void Pour()
    {
        body.linearVelocity = new Vector2(0, 0);
        anim.SetBool("pour", true);
        plant.state = Plant.State.Growing;

        if (Input.GetKeyUp(KeyCode.Space) || water <= 0)
        {
            state = State.Standard;
            plant.state = Plant.State.NotGrowing;
            waterBubble.enabled = false;
        }
    }

    private void HitStun()
    {
        hitStunTime += Time.deltaTime;
        anim.SetBool("hurt", true);
        if (IsGrounded() && hitStunTime >= 0.2f)
        {
            anim.SetBool("hurt", false);
            state = State.Standard;
            iFrames = true;
            iFrameTimer = 2;
        }
    }

    private void KnockBack()
    {
        Physics2D.IgnoreLayerCollision(6, 7);
        SoundManager.instance.PlaySound(hurtSound);
        shadow.enabled = false;
        water = Mathf.Floor(water / 2);

        if (IsFacingRight())
        {
            body.linearVelocity = new Vector2(-3f, 5f);
        }
        else
        {
            body.linearVelocity = new Vector2(3f, 5f);
        }
    }

    private void JumpForceMethod()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
    }

    public bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    public bool IsFalling()
    {
        if (body.linearVelocity.y < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsFacingRight()
    {
        if (transform.localScale.x == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void StopMoving(int react)
    {
        body.linearVelocity = new Vector2(0, 0);
        anim.SetInteger("react", react);
    }

    private void Float()
    {
        //transform.position = floatPosition;
        shadow.enabled = false;
        body.gravityScale = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (iFrames == false && state != State.HitStun && collision.gameObject.tag == "Apple")
        {
            if (collision.gameObject.GetComponent<Apple>().dead == false)
            {
                Debug.Log("Hit");
                state = State.HitStun;
                KnockBack();
            }

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Refill" && Input.GetKey(KeyCode.Space) && water < 100)
        {
            state = State.Refill;
            water += 0.5f;
        }

        if (collision.gameObject.tag == "Plant" && Input.GetKey(KeyCode.Space) && water > 0)
        {
            state = State.Pour;
            water -= 0.5f;
        }

        if (collision.gameObject.tag == "Refill" && water < 100)
        {
            waterBubble.enabled = true;
        }

        if (collision.gameObject.tag == "Plant" && water > 0)
        {
            waterBubble.enabled = true;
        }

        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    state = State.Standard;
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        waterBubble.enabled = false;
    }

    public IEnumerator Floating(Vector2 floatPosition, string sceneName)
    {
        float time = 0;
        while (time < 2)
        {
            time += Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, floatPosition, time / 2);
            if (time >= 1 && sceneTransition == false)
            {
                loader.LoadNextLevel(sceneName);
                sceneTransition = true;
            }
            yield return null;
        }

        //loader.LoadNextLevel(sceneName);
    }
}
