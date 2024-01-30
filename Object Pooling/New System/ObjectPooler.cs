using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



namespace DywFunctions
{
    namespace Pool
    {
        public class ObjectPooler : MonoBehaviour
        {
            [Serializable]
            public class Pooler
            {
                public string tag;
                public GameObject prefab;
                public int maxSize;

            }

            [Header("Pooler Settings")]
            [SerializeField]
            private bool resize = false;
            [SerializeField]
            private bool allowLogs = false;

            [SerializeField]
            List<Pooler> poolers;

            private Dictionary<string, Queue<GameObject>> poolList;
            private bool initalized = false;
            public Dictionary<string, Queue<GameObject>> PoolList { get => poolList; private set => poolList = value; }

            #region Singletton
            public static ObjectPooler instance;
            private void Awake()
            {
                instance = this;
            }
            #endregion Singletton

            private void Start()
            {
                // QualitySettings.vSyncCount = -1;
                if (!initalized) Init();
            }

            public void Init()
            {
                initalized = true;

                poolList = new Dictionary<string, Queue<GameObject>>();

            }

            void InitPoolerContainer(string tag)
            {
                if (tag == "all")
                {
                    for (int i = 0; i < poolers.Count; i++)
                    {
                        InitPoolerContainer(i);
                    }
                }
                else
                {
                    InitPoolerContainer(GetIndexOf(tag));
                }
            }

            int GetIndexOf(string tag)
            {
                for (int i = 0; i < poolers.Count; i++)
                {
                    if (poolers[i].tag == tag) return i;
                }
                return -1;
            }

            bool ExistsContainer(string tag) => poolList.ContainsKey(tag);

            void InitPoolerContainer(int id)
            {
                if (id == -1)
                {
                    Log("The references doesnt exists in Poolers list.", "error");
                    return;
                }

                var cointainer = new GameObject($"Container.{poolers[id].tag}");
                poolList.Add(poolers[id].tag, new Queue<GameObject>());

                cointainer.transform.SetParent(this.transform);
                AddPooler(poolers[id].tag, poolers[id].maxSize, cointainer.transform);
            }
            void AddPooler(string tag, int amount, Transform container, bool resizing = false)
            {
                var id = GetIndexOf(tag);
                for (int j = 0; j < amount; j++)
                {
                    var instance = Instantiate(poolers[id].prefab);
                    instance.transform.SetParent(container.transform);
                    var name = !resizing ? j + "" : "Extra";
                    instance.name = $"{tag}.{name}";
                    var poolerObject = instance.GetComponent<PoolerObject>();
                    if (poolerObject)
                    {
                        instance.GetComponent<PoolerObject>().Init(this, tag, container.transform);
                        poolList[tag].Enqueue(instance);
                        instance.gameObject.SetActive(false);
                    }
                    else
                    {
                        Log($"The object {tag}{j} isn't PoolerObject, will not return", "warning");
                    }
                }
            }

            Transform GetContainer(string tag)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (tag == transform.GetChild(i).name.Split('.')[1])
                    {
                        return transform.GetChild(i);
                    }
                }
                return null;
            }

            public GameObject Spawn(string tag, Vector3 position, Quaternion rotation)
            {
                if (!initalized) Init();
                if (!ExistsContainer(tag)) InitPoolerContainer(tag);

                if (!poolList.ContainsKey(tag))
                {
                    Log($"Pool don't contains the key {tag}", "error");
                    return null;
                }

                if (poolList[tag].Count == 0 && resize)
                {
                    AddPooler(tag, 5, GetContainer(tag), true);
                    Log($"Pool ${tag} was resizing");
                }

                if (poolList[tag].Count > 0)
                {
                    var spawned = poolList[tag].Dequeue();
                    spawned.gameObject.SetActive(true);
                    spawned.gameObject.transform.SetParent(null);

                    spawned.transform.position = position;
                    spawned.transform.rotation = rotation;

                    var pooler = spawned.GetComponent<PoolerObject>();
                    pooler?.OnPoolSpawn();

                    return spawned;
                }
                else
                {
                    Log($"Pool ${tag} is Empty, imposible spawn! You can allow resize for avoid this.", "warning");
                    return null;
                }
            }


            public void BackToPooler(PoolerObject poolerObject)
            {
                poolerObject.gameObject.SetActive(false);
                poolerObject.transform.SetParent(poolerObject.Container);

                poolList[poolerObject.PoolerTag].Enqueue(poolerObject.gameObject);
            }
            void Log(string message, string type = "log")
            {
                if (allowLogs)
                {
                    switch (type)
                    {
                        case "waring":
                            Debug.LogWarning(message);
                            break;
                        case "error":
                            Debug.LogError(message);
                            break;
                        default:
                            Debug.Log(message);
                            break;
                    }
                }
            }
        }


    }

}
