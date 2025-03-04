using System;
using UnityEngine;

public class Player : Character
{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float checkLength;


    [SerializeField] private Transform footer;

    private int coin = 0;

    private float horizontal;
    private float vertical;
    public bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack = false;
    private bool isDead = false;

    [SerializeField] private float jumpForce;

    private Vector3 savePoint;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SavePoint();
        //OnInit();
    }

    public override void OnInit()
    {
        base.OnInit();
        isDead = false;
        isAttack = false;
        isJumping = false;
        transform.position = savePoint;
        ChangeAnim("idle");
    }
    protected override void OnDeath()
    {
        base.OnDeath();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(isDead) return;
        isGrounded = CheckGrounded();
        horizontal = Input.GetAxisRaw("Horizontal");
        //vertical = Input.GetAxisRaw("Vertical");

        if (isGrounded)
        {
            if (isJumping) return;
            if (isAttack)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }
            //Jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }
            //change anim run
            if(Mathf.Abs(horizontal) > 0.1f && !isAttack)
            {
                ChangeAnim("run");
            }

            //attack
            if (Input.GetKeyDown(KeyCode.C) && isGrounded) { Attack(); }
            //throw
            if (Input.GetKeyDown(KeyCode.V) && isGrounded) { Throw(); }

        }
        //check fall
        if (!isGrounded && rb.linearVelocityY < 0f)
        {
            ChangeAnim("fall");
            isJumping = false;
        }


        //Moving
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.linearVelocity = new Vector2 (horizontal * speed * Time.fixedDeltaTime,  rb.linearVelocityY);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180 , 0));
            //transform.localScale = new Vector3(horizontal, 1, 1);
        }
        // idle
        else if(isGrounded)
        {
            ChangeAnim("idle");
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void ResetVelocity()
    {
    }

    private bool CheckGrounded()
    {
        Debug.DrawLine(footer.position, footer.position + Vector3.down *1.1f,  Color.red);
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, ground);
        RaycastHit2D hit = Physics2D.BoxCast(footer.position, new Vector2(0.9f, 0.1f), 0f, Vector2.down, checkLength , ground);

        return hit.collider != null;
    }

    private void Attack()
    {
        isAttack = true;
        ChangeAnim("attack");
        Invoke(nameof(ResetAttack), 0.5f);
    }

    private void Throw()
    {
        isAttack = true;
        ChangeAnim("throw");
        Invoke(nameof(ResetAttack), 0.5f);
    }
    private void ResetAttack()
    {
        ChangeAnim("/idle");
        isAttack = false;


    }

    private void Jump()
    {
        isJumping = true;
        rb.AddForce(jumpForce * Vector2.up);
        ChangeAnim("jump");
    }

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            coin++;
            Destroy(collision.gameObject);
            //Debug.Log("Coin");
        }
        if (collision.CompareTag("DeathZone"))
        {
            //Destroy(rb);
            isDead = true;
            ChangeAnim("die");
            Debug.Log("DIEEEEE");
            Invoke(nameof(OnInit), 2f);
        }
    }

    internal void SavePoint()
    {
        savePoint = transform.position;
    }
}
