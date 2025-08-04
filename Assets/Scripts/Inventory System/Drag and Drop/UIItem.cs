using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Canvas mainCanvas;
    public Image itemImage;
    public InventoryItemInfo itemInfo;

    private UISlot currentSlot; // поле для отслеживания текущего слота
    private UISlot lastSlot;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        mainCanvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        itemImage = GetComponent<Image>();
    }

    public void SetItemInfo(InventoryItemInfo newItemInfo)
    {
        if (newItemInfo != null && newItemInfo.spriteIcon != null)
        {
            itemImage.sprite = newItemInfo.spriteIcon;
            itemInfo = newItemInfo;
        }
        else
        {
            Debug.LogWarning("Проблема с информацией о предмете или его изображением.");
        }
    }

  
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        var slotTransform = rectTransform.parent;
        slotTransform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;

        
        currentSlot = slotTransform.GetComponent<UISlot>();
    }

    
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / mainCanvas.scaleFactor;
    }

   
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector2.zero;
        canvasGroup.blocksRaycasts = true;

        // Если есть текущий слот, пытаемся поменять предметы
        if (currentSlot != null)
        {
            UISlot targetSlot = eventData.pointerEnter?.GetComponent<UISlot>();

            if (targetSlot != null && !targetSlot.HasItem())
            {
                
                currentSlot.RemoveItem(this);
                targetSlot.AddUIItem(this);
            }

           
            currentSlot = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemInfo != null)
        {
           
            Debug.Log("Clicked on UIItem: " + itemInfo.title);
        }
        else
        {
            Debug.LogError("ItemInfo is null! Check your initialization logic.");
        }
    }


    
    public UISlot GetLastSlot()
    {
        return lastSlot;
    }

    
    public UISlot GetCurrentSlot()
    {
        return currentSlot;
    }

   
    public void SetLastSlot(UISlot slot)
    {
        lastSlot = slot;
    }

    
    public void SetCurrentSlot(UISlot slot)
    {
        currentSlot = slot;
    }


}
