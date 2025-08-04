using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishDoor : MonoBehaviour
{
    public bool state = false;
    public GameObject panelDoor;
    public bool doorIsOpen = false;
    private void Start()
    {
        panelDoor.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            state = true;
        }
    }

    public void OpenFinishDoor()
    {
        if (state == true)
        {
            panelDoor.SetActive(true);
            state = false;
            doorIsOpen = true;
        }
    }
}
