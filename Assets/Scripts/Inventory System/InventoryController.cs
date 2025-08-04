using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject SlotPrefab; // Префаб предмета
    private FinishDoor finishDoor;
    private KeyScript codeKey;
    private AddingItems itemAdd;
    public SlotForKey keySlot;
    public GameObject panelDoor;
    public GameObject PanelFinish;

    public bool buttonPresed = false;
    public UISlot[] inventorySlots;


    void Start()
    {
        if (buttonPresed == false)
        {
            PanelFinish.SetActive(false);
        }
        
        
    }

    

    public UISlot GetFreeSlot()
    {
        foreach (UISlot slot in inventorySlots)
        {
            if (!slot.HasItem())
            {
                return slot;
            }
        }
        return null;
    }
  




   

   public void TurnOffPanel()
    {
        panelDoor.SetActive(false);
        
    }


    public void FinishLevel()
    {
            if (keySlot.isKeyOnSlot == true)
            {
                PanelFinish.SetActive(true);
                panelDoor.SetActive(false);
            }
    }
    void Update()
    {
        
    }
}
