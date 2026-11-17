using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = Color.gray;

    private bool isHovered;
    private bool isSelected;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        UpdateTint();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        UpdateTint();
    }

    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
        UpdateTint();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
        UpdateTint();
    }

    private void UpdateTint()
    {
        if (isHovered || isSelected)
        {
            targetImage.color = hoverColor;
        }
        else
        {
            targetImage.color = normalColor;
        }
    }

    private void OnDisable()
    {
        isHovered = false;
        isSelected = false;
        if (targetImage != null)
            targetImage.color = normalColor;
    }
}
