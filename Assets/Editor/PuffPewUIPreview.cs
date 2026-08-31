#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class PuffPewUIPreview
{
    [MenuItem("Tools/PuffPew/Create HUD Preview In Scene")]
    public static void CreateHudPreviewInScene()
    {
        GameObject uiObject = GameObject.Find("UI");
        if (uiObject == null)
        {
            uiObject = new GameObject("UI");
        }

        Canvas canvas = uiObject.GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = uiObject.AddComponent<Canvas>();
        }

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = uiObject.GetComponent<CanvasScaler>();
        if (scaler == null)
        {
            scaler = uiObject.AddComponent<CanvasScaler>();
        }

        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);

        if (uiObject.GetComponent<GraphicRaycaster>() == null)
        {
            uiObject.AddComponent<GraphicRaycaster>();
        }

        HUDController hud = uiObject.GetComponent<HUDController>();
        if (hud == null)
        {
            hud = uiObject.AddComponent<HUDController>();
        }

        hud.BuildUI();
        hud.ApplyEditorPreview();

        Selection.activeGameObject = uiObject;
        EditorSceneManager.MarkAllScenesDirty();
    }
}
#endif
