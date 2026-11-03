using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    RectTransform panel;
    Rect lastSafeArea;
    Vector2Int lastScreenSize;
    ScreenOrientation lastOrientation;

    void OnEnable()
    {
        panel = GetComponent<RectTransform>();
        Apply();
    }

    void Update()
    {
        bool hasChanged = Screen.safeArea != lastSafeArea
                          || Screen.width != lastScreenSize.x
                          || Screen.height != lastScreenSize.y
                          || Screen.orientation != lastOrientation;
        if (hasChanged) Apply();
    }

    void Apply()
    {
        if (panel == null || Screen.width <= 0 || Screen.height <= 0) return;

        Rect safe = Screen.safeArea;
        lastSafeArea = safe;
        lastScreenSize = new Vector2Int(Screen.width, Screen.height);
        lastOrientation = Screen.orientation;

        Vector2 min = new Vector2(safe.xMin / Screen.width, safe.yMin / Screen.height);
        Vector2 max = new Vector2(safe.xMax / Screen.width, safe.yMax / Screen.height);

        panel.anchorMin = min;
        panel.anchorMax = max;
        panel.offsetMin = Vector2.zero;
        panel.offsetMax = Vector2.zero;
    }
}
