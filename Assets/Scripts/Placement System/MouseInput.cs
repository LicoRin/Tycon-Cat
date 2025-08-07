using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouseInput : MonoBehaviour
{
    public Vector2 worldPosition { get; private set; }
    public event Action Clicked;


    private void OnLook(InputValue value)
    {
        Vector2 mousePosition = value.Get<Vector2>();
        worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void OnAction(InputValue _)
    {
        Clicked?.Invoke();
    }
}
