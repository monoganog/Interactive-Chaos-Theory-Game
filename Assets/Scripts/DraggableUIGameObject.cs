using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// With thanks to the GOAT @CodeMonkey https://www.youtube.com/watch?v=Mb2oua3FjZg

public class DraggableUIGameObject : MonoBehaviour, IDragHandler, IEndDragHandler
{

    public RectTransform dragRectTransform;
    public Canvas canvas;

    public void OnDrag(PointerEventData eventData)
    {
        dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }


    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
