using System.Collections.Generic;

namespace CromisDev.Extensions.Collections
{
    public static class CollectionUtils
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int randIndex = UnityEngine.Random.Range(i, list.Count);
                (list[i], list[randIndex]) = (list[randIndex], list[i]);
            }
        }
    }
}
