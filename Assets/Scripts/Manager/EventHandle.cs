using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandle : Singleton<EventHandle>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public Action<int> GetLevel;
}
