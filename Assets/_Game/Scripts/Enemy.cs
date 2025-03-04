using System;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float rangeAttack;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    private IState currentState;
    private bool isRight = true;

    private Character target;
    public Character Target => target;

    private void Update()
    {
        if(currentState != null)
        {
            currentState.OnExecute(this);   
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created 
    public override void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
    }
    protected override void OnDeath()
    {
        base.OnDeath();
    }
    public void ChangeState(IState newState) {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
    public void Moving()
    {
        ChangeAnim("run");
        rb.linearVelocity = transform.right * moveSpeed;
    }
    public void StopMoving()
    {
        ChangeAnim("idle");
        rb.linearVelocity = Vector2.zero;
    }
    public void Attack()
    {

    }
    public bool IsTargetRange()
    {
        if (target != null && Vector2.Distance(target.transform.position, transform.position) <= rangeAttack)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "EnemyWall")
        {
            ChangeDirection(!isRight);
        }
    }

    public void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;
        transform.rotation = Quaternion.Euler(0, isRight ? 0 : 180, 0);
    }

    internal void SetTarget(Player player)
    {
        this.target = player;
        if (IsTargetRange())
        {
            ChangeState(new AttackState());
        }
        else if (Target != null)
        {
            ChangeState(new PatrolState());
        }
        else
        {
            ChangeState(new IdleState());
        }
    }
}
