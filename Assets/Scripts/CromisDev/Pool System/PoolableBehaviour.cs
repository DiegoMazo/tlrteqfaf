using UnityEngine;
using UnityEngine.Events;

namespace CromisDev.Pooling
{
    public class PoolableBehaviour : MonoBehaviour, IPoolable
    {
        private bool pooled;
        public UnityEvent<bool> OnPooledStateChanged;

        public bool PreviouslyVisible { get; protected set; }

        public bool Pooled
        {
            get => pooled;
            set
            {
                pooled = value;
                OnPooledStateChanged?.Invoke(value);
            }
        }

        public virtual bool ShouldSpawn() => true;

        public virtual void Show()
        {
            gameObject.SetActive(true);
            Pooled = false;
        }
        public virtual void Hide()
        {
            gameObject.SetActive(false);
            Pooled = true;
        }

        public virtual void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

    }
}
