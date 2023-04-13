//using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Animations;
using UnityEngine;

public class CharacterController<T> : Singleton<T> where T: CharacterController<T>
{
    public Vector3 startPos;
    public Vector3 attackPos;
    private Animator ani;

    protected override void Awake()
    {
        base.Awake();
        TurnToStartPos();
        ani = GetComponent<Animator>();
    }

    public void TurnToStartPos()
    {
        transform.position = startPos;
    }

    public void TurnToAttackPos()
    {
        transform.position = attackPos;
    }

    public void Attack(int dir, bool toAttackPos)
    {
        if (toAttackPos) { TurnToAttackPos(); }
        ani.SetInteger("AttackDir", dir);
        ani.SetTrigger("Attack");
    }

    public void Block(bool isSuccess)
    {
        ani.SetBool("BlockResult", isSuccess);
        ani.SetTrigger("Block");
    }

    public void Death()
    {
        ani.SetTrigger("Death");
    }

    public void SetAnimatorSpeed(float _speed)
    {
        ani.speed = _speed;
    }
}
