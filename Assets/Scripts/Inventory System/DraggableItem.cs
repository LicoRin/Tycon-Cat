
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector2 startPosition;

    void Start()
    {
       
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

   
    public void OnBeginDrag(PointerEventData eventData)
    {
        
        startPosition = rectTransform.anchoredPosition;
       
        canvasGroup.blocksRaycasts = false;
    }

    
    public void OnDrag(PointerEventData eventData)
    {
        
        rectTransform.anchoredPosition += eventData.delta / canvasGroup.transform.localScale.x;
    }

    
    public void OnEndDrag(PointerEventData eventData)
    {
       
        canvasGroup.blocksRaycasts = true;

      
        if (eventData.pointerEnter != null)
        {
            UISlot targetSlot = eventData.pointerEnter.GetComponent<UISlot>();

            if (targetSlot != null)
            {
              
                rectTransform.anchoredPosition = Vector2.zero;
                transform.SetParent(targetSlot.transform);
            }
            else
            {
               
                rectTransform.anchoredPosition = startPosition;
                transform.SetParent(null);
            }
        }
        else
        {
            // Если объект не брошен ни на какой элемент UI, возвращаем его на начальную позицию
            rectTransform.anchoredPosition = startPosition;
            transform.SetParent(null);
        }
    }
}
