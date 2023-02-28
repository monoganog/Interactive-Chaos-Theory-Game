using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Simple script to allow moving of the UI menu panel
///
/// Ben Monaghan 2023
/// With thanks to the GOAT @CodeMonkey https://www.youtube.com/watch?v=Mb2oua3FjZg
/// </summary>

public class DraggableUIGameObject : MonoBehaviour, IDragHandler
{
    public RectTransform dragRectTransform;
    public Canvas canvas;

    public void OnDrag(PointerEventData eventData)
    {
        dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

}
