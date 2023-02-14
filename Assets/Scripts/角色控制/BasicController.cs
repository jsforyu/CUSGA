using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicController : MonoBehaviour
{
    private Vector3 startPos;
    private Animator ani;
    protected virtual void Awake()
    {
        startPos = transform.position;
        ani = GetComponent<Animator>();
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
}
