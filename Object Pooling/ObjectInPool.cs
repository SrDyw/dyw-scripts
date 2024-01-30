using UnityEngine;

public class ObjectInPool : MonoBehaviour
{
    [HideInInspector] public Transform parent;
    public void RestartObjectInPool() {
        gameObject.SetActive(false);
        transform.SetParent(parent);
        transform.localScale = Vector3.one;
        transform.position = Vector3.zero;
    }
}
