using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject SlotPrefab; // Префаб предмета
   

    private AddingItems itemAdd;



    public UISlot[] inventorySlots;


    void Start()
    {
     
        
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
  




   





    void Update()
    {
        
    }
}
