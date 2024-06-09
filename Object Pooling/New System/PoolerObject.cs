using UnityEngine;
using DywFunctions.Pool;

namespace DywFunctions
{
    namespace Pool
    {
        public abstract class PoolerObject : MonoBehaviour
        {
            private ObjectPooler pooler;
            private string poolerTag;
            private Transform container;

            public string PoolerTag { get => poolerTag; private set => poolerTag = value; }
            public ObjectPooler Pooler { get => pooler; }
            public Transform Container { get => container; }

            public virtual void OnPoolSpawn() { }

            public virtual void ReturnToPool()
            {
                if (pooler)
                {
                    pooler.BackToPooler(this);
                } else Destroy(gameObject);
            }

            public void Init(ObjectPooler pooler, string poolerTag, Transform container)
            {
                this.pooler = pooler;
                this.poolerTag = poolerTag;
                this.container = container;
            }
        }

    }
}
