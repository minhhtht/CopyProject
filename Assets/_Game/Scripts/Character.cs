using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private float hp;
    private string currentAnimName;

    private bool IsDead => hp <= 0;

    void Start()
    {
        OnInit();
    }
    public virtual void OnInit()
    {
        hp = 100;
    }
    public virtual void OnDespawn()
    {

    }

    protected virtual void OnDeath()
    {

    }

    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            //anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }

    public virtual void OnHit(float damage)
    {
        if (!IsDead)
        {
            hp -= damage;
            if (IsDead)
            {
                OnDeath();
            }
        }
        
    }

    
}
