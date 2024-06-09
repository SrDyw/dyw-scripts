using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DywFunctions.Pool;

public class SimplePoolerObject : PoolerObject
{
    public float lifeTime = 5f;
    [SerializeField]
    private Animator animator;
    public Animator Animator { get => animator; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (!Pooler) OnPoolSpawn();
    }
    public override void OnPoolSpawn()
    {
        if (animator) animator.enabled = true;
        StartCoroutine(Timer(lifeTime));
    }

    public override void ReturnToPool()
    {
        if (animator) animator.enabled = false;
        base.ReturnToPool();
    }

    public IEnumerator Timer(float time)
    {   
        yield return new WaitForSeconds(time);
        ReturnToPool();
    }
}
