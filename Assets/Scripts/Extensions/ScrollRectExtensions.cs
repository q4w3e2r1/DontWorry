using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ScrollRectExtensions
{

    public static void ScrollToBottom(this ScrollRect scrollRect)
    {
        WaitForEnd();
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();
    }

    private static IEnumerator WaitForEnd()
    {
        yield return new WaitForEndOfFrame();
    }

    public static void ScrollToBottom(this ScrollRect scrollRect, GameObject item)
    {
        Canvas.ForceUpdateCanvases();

        item.GetComponent<HorizontalLayoutGroup>().CalculateLayoutInputVertical();
        item.GetComponent<ContentSizeFitter>().SetLayoutVertical();

        scrollRect.content.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();
        scrollRect.content.GetComponent<ContentSizeFitter>().SetLayoutVertical();

        scrollRect.verticalNormalizedPosition = 0;
    }
}
