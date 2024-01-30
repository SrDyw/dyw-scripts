using UnityEngine;

[ExecuteInEditMode]
public class Join : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private bool ajustOffset = false;

    private Vector3 offset;

    public Transform Target { get => target; set => target = value; }
    public Vector3 Offset { get => offset; set => offset = value; }

    void Update()
    {
        if (target != null) {
            if (!ajustOffset) transform.position = target.position + offset;
            else {
                UpdateOffset();
            }
        }
    }

    void UpdateOffset() {
        float x = transform.position.x - target.position.x;
        float y = transform.position.y - target.position.y;
        float z = transform.position.z - target.position.z;

        offset = new Vector3(x, y, z);
    }
}
