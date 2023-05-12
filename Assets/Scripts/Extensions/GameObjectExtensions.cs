using System.Linq;
using UnityEngine;

namespace SQL_Quest.Extentions
{
    public static class GameObjectExtensions
    {
        public static bool IsInLayer(this GameObject go, LayerMask layer)
        {
            return layer == (layer | 1 << go.layer);
        }

        public static void SetAsFirstSiblings(params GameObject[] gameObjects)
        {
            foreach (var gameObject in gameObjects.Reverse())
                gameObject.transform.SetAsFirstSibling();
        }

        public static void SetAsLastSiblings(params GameObject[] gameObjects)
        {
            foreach (var gameObject in gameObjects)
                gameObject.transform.SetAsLastSibling();
        }
    }
}