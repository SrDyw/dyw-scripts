using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DywFunctions.Pool;

public class AniamtedPooler : PoolerObject
{
    [SerializeField]
    private Animator animator;

    public Animator Animator { get => animator; set => animator = value; }

    public override void OnPoolSpawn()
    {
        animator.enabled = true;
    }

    public override void ReturnToPool()
    {
        animator.enabled = false;
        base.ReturnToPool();
    }
}
