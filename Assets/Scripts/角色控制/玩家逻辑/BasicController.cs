using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class BasicController<T> : Singleton<T> where T: BasicController<T>
{
    public Vector3 startPos;
    public Vector3 attackPos;
    public Animator ani;
    private AnimatorController animController;

    protected override void Awake()
    {
        base.Awake();
        ReturnToStartPos();
        ani = GetComponent<Animator>();
        animController = ani.runtimeAnimatorController as AnimatorController;
    }

    public void ReturnToStartPos()
    {
        transform.position = startPos;
    }

    public void ReturnToAttackPos()
    {
        transform.position = attackPos;
    }

    public void Attack(int dir)
    {
        ani.SetInteger("AttackDir", dir);
        ani.SetTrigger("Attack");
    }

    public void Block(bool isSuccess)
    {
        ani.SetBool("BlockResult", isSuccess);
        ani.SetTrigger("Block");
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
