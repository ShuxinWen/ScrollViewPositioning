using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Positioning : MonoBehaviour
{

    private ScrollRect scrollRect1;


    float target_OnlyOne;
    // Start is called before the first frame update
    void Start()
    {
        scrollRect1 = GameObject.Find("Scroll View").GetComponent<ScrollRect>();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Button button = this.transform.GetChild(i).GetComponent<Button>();
            button.onClick.AddListener(() =>
            {

                target_OnlyOne = GetScrollViewNormalizedPositionOnlyOne(scrollRect1, int.Parse(button.name) - 1, true);
                scrollRect1.verticalNormalizedPosition = target_OnlyOne;

            });
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// 获取ScrollView对应VerticalNormalizedPosition或者HorizontalNormalizedPosition
    /// vertical和horizontal只勾选一个的情况
    /// </summary>
    /// <param name="currentChildIndex">物体在数组中的index</param>
    /// <param name="inverse">是否反着来，从上而下、从右往左要反着来</param>
    /// <param name="pixelOffset">像素偏移，向下向右为正</param>
    /// <returns>0 ~ 1，VerticalNormalizedPosition或者HorizontalNormalizedPosition</returns>
    public static float GetScrollViewNormalizedPositionOnlyOne(ScrollRect scrollRect, int currentChildIndex, bool inverse = false, float pixelOffset = 0)
    {
        if (scrollRect.viewport == null || scrollRect.content == null)
        {
            Debug.LogError("ScrollView的Content或Viewport为空");
            return inverse ? 1 : 0;
        }
        var childTrans = scrollRect.content.GetChild(0) as RectTransform;
        if (childTrans == null)
        {
            Debug.LogError("Content下面没有物体或不是RectTransform");
            return inverse ? 1 : 0;
        }

        if (scrollRect.vertical && scrollRect.horizontal)
        {
            Debug.LogError("vertical和horizontal只能勾选一个");
            return inverse ? 1 : 0;
        }

        Rect viewportRect = scrollRect.viewport.rect;
        Rect contentRect = scrollRect.content.rect;
        Rect childrenRect = childTrans.rect;


        if (scrollRect.vertical)
        {
            VerticalLayoutGroup group = scrollRect.content.GetComponent<VerticalLayoutGroup>();
            if (group == null)
            {
                Debug.LogError("获取VerticalLayoutGroup失败");
                return inverse ? 1 : 0;
            }

            var diff = contentRect.height - viewportRect.height;
            float elementLength = childrenRect.height + group.spacing;

            if (inverse)
                return Mathf.Clamp01(1 - (currentChildIndex * elementLength + pixelOffset) / diff);
            else
                return Mathf.Clamp01((currentChildIndex * elementLength - pixelOffset) / diff);
        }

        if (scrollRect.horizontal)
        {
            HorizontalLayoutGroup group = scrollRect.content.gameObject.GetComponent<HorizontalLayoutGroup>();
            if (group == null)
            {
                Debug.LogError("获取HorizontalLayoutGroup失败");
                return inverse ? 1 : 0;
            }

            var diff = contentRect.width - viewportRect.width;
            float elementLength = childrenRect.width + group.spacing;

            if (inverse)
                return Mathf.Clamp01(1 - (currentChildIndex * elementLength - pixelOffset) / diff);
            else
                return Mathf.Clamp01((currentChildIndex * elementLength + pixelOffset) / diff);
        }

        return inverse ? 1 : 0;
    }
}
