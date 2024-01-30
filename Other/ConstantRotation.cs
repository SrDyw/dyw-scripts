using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
	[SerializeField]
    private Vector3 rotationVector;
	//< Properties

	//>

    void Update()
    {
        transform.Rotate(rotationVector * Time.deltaTime);
    }
}
