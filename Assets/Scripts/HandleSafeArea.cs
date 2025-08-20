using UnityEngine;

// Will always apply even if we dont hit play
[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class HandleSafeArea : MonoBehaviour
{
    private RectTransform rectTransform;
    private Rect lastSafeArea;
    private DrivenRectTransformTracker tracker;

    private void OnEnable()
    {
        // safe because we added [RequiredComponent] at the top
        rectTransform = GetComponent<RectTransform>();
        tracker.Add(this, rectTransform, DrivenTransformProperties.All);
        Canvas.willRenderCanvases += OnWillRenderCanvases;
    }

    private void OnDisable()
    {
        Canvas.willRenderCanvases -= OnWillRenderCanvases;
    }

    private void OnWillRenderCanvases()
    {
        Rect safeArea = Screen.safeArea;

        if (lastSafeArea == safeArea) return;

        lastSafeArea = safeArea;

        Vector2 screenResolution = new(Screen.width, Screen.height);

        Vector2 minAnchor = safeArea.position / screenResolution;
        Vector2 maxAnchor = minAnchor + (safeArea.size / screenResolution);

        rectTransform.anchorMin = minAnchor;
        rectTransform.anchorMax = maxAnchor;
        rectTransform.localScale = Vector3.one;

    }
}
