using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroytAtTime : MonoBehaviour
{
    public float timeToDestroy = 1;
    
    void Awake()
    {
        StartCoroutine(DestroyGameObject());
    }

    IEnumerator DestroyGameObject() {

        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
        yield return null;
    }
}
