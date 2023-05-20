using UnityEngine;

namespace SQL_Quest.Components.UI
{
    public class ChildsSizeFitter : MonoBehaviour
    {
        [SerializeField] private RectTransform parent;
        [SerializeField] private RectTransform[] childs;

        public void Update()
        {
            var width = Vector2.zero;
            foreach (var child in childs)
                width += child.sizeDelta;

            width.x += childs.Length * 5 + 20;
            parent.GetComponent<RectTransform>().sizeDelta = width;
        }
    }
}