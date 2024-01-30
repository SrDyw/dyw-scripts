using UnityEngine;

public class ConstantSpeed : MonoBehaviour
{
	public Vector3 constantSpeed;
    public float timeToDestroy = 10f;
	//< Properties

	//>

    private void Start() {
        if (timeToDestroy != -1) {
            Invoke("DestroyGameObject", timeToDestroy);
        }
    }
    void Update()
    {
        transform.position += constantSpeed * Time.fixedDeltaTime;
    }

    public void DestroyGameObject() {
        Destroy(gameObject);
    }


}
