
using UnityEngine;

namespace CromisDev.Pooling
{
    public interface IPoolable
    {
        public bool Pooled { get; set; }
        public void Show();
        public void Hide();
        public void SetParent(Transform parent);
        public bool ShouldSpawn();
    }
}
