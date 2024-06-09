using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroytAtTime : MonoBehaviour
{
    public float timeToDestroy = 1;
    public bool autoInit = true;

    void Awake()
    {
        if (autoInit) Init(timeToDestroy);

    }
    public void Init(float time)
    {
        StopAllCoroutines();
        StartCoroutine(DestroyGameObject(time));
    }

    IEnumerator DestroyGameObject(float time)
    {

        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
    }
}
