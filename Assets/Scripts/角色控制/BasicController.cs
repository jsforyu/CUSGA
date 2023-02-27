using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class BasicController : MonoBehaviour
{
    public Vector3 startPos;
    public Animator ani;
    private AnimatorController animController;

    protected virtual void Awake()
    {
        startPos = transform.position;
        ani = GetComponent<Animator>();
        animController = ani.runtimeAnimatorController as AnimatorController;
    }
    public void ReturnToStartPos()
    {
        if(transform.position!=startPos)
        transform.position = startPos;
    }
    public void ReturnToAttackPos(Vector3 pos)
    {
        if(transform.position!=pos)
        transform.position = pos;
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
