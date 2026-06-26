using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
[RequireComponent(typeof(RectTransform))]
public class TMPDropShadow : MonoBehaviour
{
    private TextMeshProUGUI _mainTMP; //The new "white" TMP text
    private RectTransform _mainTrans; // The new "white" transform

    private TextMeshProUGUI _shadowTMP; //The initial TMP text
    private RectTransform _shadowTrans; //The initial transform

    [Header("Drop Shadow Settings")]

    [Space(5)]

    [Tooltip("The offset of the drop shadow relative to the main text.")]
    public Vector2 shadowOffset = new(10f, -10f);

    [Tooltip("The color of the drop shadow.")]
    public Color shadowColor = Color.black;

    void OnEnable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
    }
    void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
    }

    void Awake()
    {
        _shadowTMP = GetComponent<TextMeshProUGUI>(); // Original GameObject turns into the Shadow object because of Unity's UI hierarchical layering order
        _shadowTrans = GetComponent<RectTransform>();     

        Initialize();
    }

    /// <summary>
    /// Makes the copy of the original gameobject.
    /// Initializes new child that is now the "white" part of the text.
    /// Copies all the TMP properties from source to new child.
    /// </summary>
    private void Initialize()
    {
        _mainTrans = new GameObject().AddComponent<RectTransform>();
        _mainTrans.transform.SetParent(transform);
        _mainTrans.name = $"{gameObject.name}_Main";

        // _mainTrans.localPosition = shadowOffset;
        _mainTrans.localEulerAngles = Vector3.zero;
        _mainTrans.localScale = transform.localScale;
        _mainTrans.sizeDelta = _shadowTrans.sizeDelta;
        _mainTrans.anchorMin = _shadowTrans.anchorMin;
        _mainTrans.anchorMax = _shadowTrans.anchorMax;
        _mainTrans.pivot = _shadowTrans.pivot;
        _mainTrans.anchoredPosition = -shadowOffset;
        
        _shadowTrans.anchoredPosition += shadowOffset;
        _shadowTMP.overrideColorTags = true;

        #region TextMeshProUGUI Component Properties Duplication
        _mainTMP = _mainTrans.gameObject.AddComponent<TextMeshProUGUI>();
        
        // Text
        _mainTMP.text = _shadowTMP.text;
        
        // Font
        _mainTMP.font = _shadowTMP.font;
        _mainTMP.material = _shadowTMP.material;
        _mainTMP.fontStyle = _shadowTMP.fontStyle;
        _mainTMP.fontSize = _shadowTMP.fontSize;
        
        // Color
        _mainTMP.color = _shadowTMP.color;
        _mainTMP.colorGradient = _shadowTMP.colorGradient;
        _mainTMP.colorGradientPreset = _shadowTMP.colorGradientPreset;
        
        // Spacing
        _mainTMP.characterSpacing = _shadowTMP.characterSpacing;
        _mainTMP.wordSpacing = _shadowTMP.wordSpacing;
        _mainTMP.lineSpacing = _shadowTMP.lineSpacing;
        _mainTMP.paragraphSpacing = _shadowTMP.paragraphSpacing;
        
        // Alignment
        _mainTMP.alignment = _shadowTMP.alignment;
        _mainTMP.horizontalAlignment = _shadowTMP.horizontalAlignment;
        _mainTMP.verticalAlignment = _shadowTMP.verticalAlignment;
        
        // Wrapping
        // _mainTMP.enableWordWrapping = _shadowTMP.enableWordWrapping;
        _mainTMP.overflowMode = _shadowTMP.overflowMode;
        
        // Mapping
        _mainTMP.horizontalMapping = _shadowTMP.horizontalMapping;
        _mainTMP.verticalMapping = _shadowTMP.verticalMapping;
        _mainTMP.mappingUvLineOffset = _shadowTMP.mappingUvLineOffset;
        
        // Extra Settings
        _mainTMP.margin = _shadowTMP.margin;
        _mainTMP.geometrySortingOrder = _shadowTMP.geometrySortingOrder;
        _mainTMP.richText = _shadowTMP.richText;
        _mainTMP.raycastTarget = _shadowTMP.raycastTarget;
        _mainTMP.parseCtrlCharacters = _shadowTMP.parseCtrlCharacters;
        _mainTMP.useMaxVisibleDescender = _shadowTMP.useMaxVisibleDescender;
        _mainTMP.spriteAsset = _shadowTMP.spriteAsset;
        // _mainTMP.enableKerning = _shadowTMP.enableKerning;
        _mainTMP.extraPadding = _shadowTMP.extraPadding;
        #endregion

        _shadowTMP.color = shadowColor;
        _shadowTMP.gameObject.name = $"{_shadowTMP.gameObject.name}_Shadow";
    }

    /// <summary>
    /// Called when a text in the scene has changed.
    /// Checks if it was this gameobject and updates its text.
    /// </summary>
    /// <param name="obj"></param>
    private void OnTextChanged(Object obj)
    {
        if (obj == _shadowTMP)
        {
            _mainTMP.text = _shadowTMP.text;
            _mainTMP.ForceMeshUpdate();
        }
    }
}
