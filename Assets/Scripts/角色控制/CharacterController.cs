using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class CharacterController<T> : Singleton<T> where T: CharacterController<T>
{
    public Vector3 startPos;
    public Vector3 attackPos;
    private Animator ani;
    private AnimatorController animController;

    protected override void Awake()
    {
        base.Awake();
        TurnToStartPos();
        ani = GetComponent<Animator>();
        animController = ani.runtimeAnimatorController as AnimatorController;
    }

    public void TurnToStartPos()
    {
        transform.position = startPos;
    }

    public void TurnToAttackPos()
    {
        transform.position = attackPos;
    }

    public void Attack(int dir)
    {
        TurnToAttackPos();
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

    public void SetAnimatorSpeed(int _layer, string _stateName, float _speed)
    {
        for (int i = 0; i < animController.layers[_layer].stateMachine.states.Length; i++)
        {
            if (animController.layers[_layer].stateMachine.states[i].state.name == _stateName)
            {
                animController.layers[_layer].stateMachine.states[i].state.speed = _speed;
            }
        }
    }
}
