using UnityEngine;
using System;
using System.Diagnostics;
using System.Reflection;
using QxFramework.Core;
using System.Collections.Generic;

public abstract class State : ScriptableObject
{
    protected StateManager manager;
    protected Animator animator;

    public void SetManager(StateManager m)
    {
        manager = m;
    }

    public virtual void Init()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void OnEnterState(State LastState)
    {

    }
    public virtual void OnExitState()
    {

    }

    public virtual void PlayAnimation(string name)
    {
        animator.Play(name, -1, 0);
        animator.Update(0);
    }

    public virtual bool IsAnimation(string name)
    {
        return animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex("Base Layer")).IsName(name);
    }
}