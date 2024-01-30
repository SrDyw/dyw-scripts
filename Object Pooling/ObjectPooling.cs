using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [Space(10)]
    [Header("Pool stats")]

    [SerializeField]
    [Min(1)]
    private int poolSize = 1;

    [SerializeField]
    [Tooltip("Game object to create")]
    private GameObject objectToCreate;

    [SerializeField]
    [Tooltip("The parent of the object pooling")]
    private Transform parent;
    
    [HideInInspector]public GameObject[] objectsInPool;

    //<Properties
    public int PoolSize { get => poolSize; set => poolSize = value; }
    public GameObject ObjectToCreate { get => objectToCreate; set => objectToCreate = value; }

    //>

    void Start()
    {
        objectsInPool = new GameObject[poolSize];
        CreatePool();

        if (parent == null) parent = transform;
    }

    void CreatePool() {
        for(int i = 0; i < objectsInPool.Length; i++)
        {
            objectsInPool[i] = Instantiate(objectToCreate, Vector3.zero, Quaternion.identity);
            objectsInPool[i].transform.SetParent(transform);
            objectsInPool[i].AddComponent<ObjectInPool>().parent = transform;
            objectsInPool[i].SetActive(false);
            objectsInPool[i].transform.localPosition = Vector3.zero;
        }
    }

    /// <summary>
    /// Return pool index of object unused yet
    /// -> If there are not free index return -1
    /// </summary>
    public int IndexFreeObject() {
        int objectToUse = -1;

        for(int i = 0; i < objectsInPool.Length; i++) {
            if (!objectsInPool[i]) continue;
            
            if (!objectsInPool[i].activeSelf) {
                objectToUse = i;
            }
        }

        return objectToUse;
    }

    public void ActivateObject(int index) {
        objectsInPool[index].SetActive(true);
        objectsInPool[index].transform.SetParent(null);
    }
    public void ActivateObject(int index, Vector3 position) {
        objectsInPool[index].SetActive(true);
        objectsInPool[index].transform.SetParent(null);
        objectsInPool[index].transform.position = position;
    }

    public GameObject ActivateObject(Vector3 position) {
        int index = IndexFreeObject();
        GameObject poolObject = null;
        if (index != -1) {
            objectsInPool[index].SetActive(true);
            objectsInPool[index].transform.SetParent(null);
            objectsInPool[index].transform.position = position;
            poolObject = objectsInPool[index];
        }
        

        return poolObject;
    }

    public void ResetPool() {
        foreach (var item in objectsInPool)
        {
            if (item.activeSelf) {
                item.transform.position = Vector3.zero;
                item.SetActive(false);
                item.transform.SetParent(parent);
            }
        }
    }

}
