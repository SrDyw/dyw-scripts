using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DywFunctions;
public class DebugSystem : MonoBehaviour
{
    private Dictionary<string, TextMeshProUGUI> debugTexts;
    private VerticalLayoutGroup verticalGroup;
    private RectTransform verticalGroupRect;
    public static DebugSystem instance;

    private void Awake()
    {
        instance = this;
        debugTexts = new Dictionary<string, TextMeshProUGUI>();
        verticalGroup = GetComponentInChildren<VerticalLayoutGroup>();
        verticalGroupRect = verticalGroup.GetComponent<RectTransform>();
    }

    /// <summary>
    /// Prints in screen a specified value to a debug text object. If the debug text object doesn't exist, it creates a new one.
    /// </summary>
    /// <param name="value">The value to be printed on screen.</param>
    /// <param name="tag">The tag of the debug text object. If the tag is not specified or is an empty string, "test1" will be used as the default tag.</param>
    public static void Print(object value, string tag = "")
    {
        InstanceSystemIfNotExists();
        string parsedValue = value?.ToString();
        var verticalRect = instance.verticalGroupRect;
        tag = tag == "" ? "test1" : tag;

        if (instance.debugTexts.TryGetValue(tag, out TextMeshProUGUI debugText))
        {
            debugText.text = parsedValue;
        }
        else
        {
            var textGO = new GameObject(tag);

            textGO.transform.SetParent(instance.verticalGroup.transform, false);
            var tmp = textGO.AddComponent<TextMeshProUGUI>();
            tmp.text = tag + ": " + parsedValue;
            tmp.enableAutoSizing = true;
            var rect = tmp.GetComponent<RectTransform>();

            rect.sizeDelta = new Vector2(verticalRect.sizeDelta.x, 25);

            instance.debugTexts.Add(tag, tmp);
        }
    }

    public static void PrintAtParent(object value, string tag, Transform parent, Vector2 offset = default)
    {
        if (!parent) {
            Debug.LogError("Cannot assign a debugger text to a null parent");
            return;
        }
        var canvas = parent.ExtractChild("LocalDebugCanvas");
        offset = offset == Vector2.zero ? Vector2.up * 2.5f : offset;

        if (!canvas)
        {
            canvas = Instantiate(Resources.Load<GameObject>("LocalDebugCanvas")).transform;
            canvas.name = "LocalDebugCanvas";
            canvas.SetParent(parent, false);
        }
        canvas.localPosition = offset;
        var verticalGroup = canvas.ExtractChild("VerticalGroup");
        var textInParent = verticalGroup.ExtractChild(tag)?.GetComponent<TextMeshProUGUI>();

        if (!textInParent)
        {
            var textGO = new GameObject(tag);
            textGO.transform.SetParent(verticalGroup.transform, false);

            textInParent = textGO.AddComponent<TextMeshProUGUI>();
            var rect = textInParent.GetComponent<RectTransform>();

            textInParent.enableAutoSizing = true;
            textInParent.fontSizeMin = 0;
            rect.sizeDelta = new Vector2(verticalGroup.GetComponent<RectTransform>().sizeDelta.x, 1);
        }

        textInParent.text = tag + ": " + value?.ToString();
    }

    public static void InstanceSystemIfNotExists()
    {
        if (!instance)
        {
            var debug = Instantiate(Resources.Load<GameObject>("DebugSystem")).AddComponent<DebugSystem>();
            instance = debug;
        }
    }
}
