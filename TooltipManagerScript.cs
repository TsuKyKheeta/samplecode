using TMPro;
using UnityEngine;

public class TooltipManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private GameObject tooltipTitle;
    [SerializeField] private GameObject tooltipCanvas;
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private TextMeshProUGUI tooltipTitleText;
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private RectTransform tooltipRectTransform;

    public static TooltipManagerScript Instance;

    public bool IsActive => tooltipPanel.activeSelf;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }

        if (tooltipPanel != null)
        {
            tooltipPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (tooltipCanvas.GetComponent<Canvas>().worldCamera == null)
        {
            tooltipCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }

    private void SetText(string text, string titleText = null, bool splitWords = false)
    {
        if (text == null || text == "")
        {
            tooltipText.text = " ";
        }
        else
        {
            tooltipText.text = text;
        }

        if (titleText != null)
        {
            tooltipTitleText.text = titleText;
            tooltipTitleText.ForceMeshUpdate();
        }

        tooltipText.ForceMeshUpdate();

        SetBackgroundSize();
    }

    public void ShowTooltip(GameObject hoveredGO, string text, string titleText = null)
    {
        if (tooltipPanel == null)
        {
            return;
        }

        SetTooltipPosition(hoveredGO);

        ShowTitle(titleText);

        SetText(text, titleText); 

        SetAnchoiredPosition();
    }

    private void ShowTitle(string titleText)
    {
        if (titleText == null)
        {
            tooltipTitle.SetActive(false);
        }
        else
        {
            tooltipTitle.SetActive(true);
        }

        tooltipPanel.SetActive(true);
    }

    private void SetTooltipPosition(GameObject hoveredGO)
    {
        tooltipPanel.transform.position = hoveredGO.transform.position;
        tooltipPanel.transform.position += new Vector3(0, 1, 0);
        tooltipPanel.transform.position -= new Vector3(1, 0, 0);
    }

    private void SetAnchoiredPosition()
    {
        var anchoredPosition = tooltipRectTransform.anchoredPosition;
        if (anchoredPosition.x + tooltipRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - tooltipRectTransform.rect.width;
        }

        if (anchoredPosition.y + tooltipRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - tooltipRectTransform.rect.height;
        }

        tooltipRectTransform.anchoredPosition = anchoredPosition;
    }

    private void SetBackgroundSize()
    {
        if (!IsActive)
        {
            return;
        }

        var textSize = tooltipText.GetRenderedValues(false);        
        var textTitleSize = tooltipTitleText.GetRenderedValues(false);

        float textSizeWidth;

        if (textSize.x > textTitleSize.x)
        {
            textSizeWidth = textSize.x;
        }
        else
        {
            textSizeWidth = textTitleSize.x;
        }

        textSize = new Vector2(textSizeWidth, textSize.y + textTitleSize.y);
        var paddingSize = new Vector2(30, 30);

        tooltipRectTransform.sizeDelta = textSize + paddingSize;
    }

    public void HideTooltip()
    {
        if (tooltipPanel == null)
        {
            return;
        }

        tooltipPanel.SetActive(false);
    }
}
