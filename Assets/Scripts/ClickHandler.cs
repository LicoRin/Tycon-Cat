using UnityEngine;
using UnityEngine.Events;

public class ClickHandler : MonoBehaviour
{
    public  UnityEvent _onClick;
    private BoxCollider2D collider2d;
    Vector3 offset;
    private MouseInput _mouseInput;

    [System.Obsolete]
    private void Awake()
    {
        collider2d = GetComponent<BoxCollider2D>();
        _mouseInput = FindObjectOfType<MouseInput>();
        _mouseInput.Clicked += MouseOnClicked;
    }

    private void MouseOnClicked()
    {

        if(collider2d.bounds.Contains(_mouseInput.worldPosition))
        {
            _onClick?.Invoke();
        }
        
    }

    void OnMouseDown()
    {
        _onClick?.Invoke();
    }
    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}
