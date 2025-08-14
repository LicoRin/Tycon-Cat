using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class HomeButton : MonoBehaviour
{
    public UnityEvent _onClick;

    [Header("Button Sprites")]
    public Sprite normalSprite;
    public Sprite pressedSprite;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D collider2d;
    private MouseInput _mouseInput;

    [System.Obsolete]
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2d = GetComponent<BoxCollider2D>();
        _mouseInput = FindObjectOfType<MouseInput>();
        _mouseInput.Clicked += MouseOnClicked;

        
        spriteRenderer.sprite = normalSprite;
    }

    private void MouseOnClicked()
    {
        if (collider2d.bounds.Contains(_mouseInput.worldPosition))
        {
            _onClick?.Invoke();
        }
    }

    private void OnMouseDown()
    {
        spriteRenderer.sprite = pressedSprite;
        _onClick?.Invoke();
    }

    private void OnMouseUp()
    {
        spriteRenderer.sprite = normalSprite;
    }

    private void OnMouseExit()
    {

        spriteRenderer.sprite = normalSprite;
    }


    public void OpenHomePanel()
    {
        GloballController.current.SetOnHomePanel();
    }
    public void CloseHomePanel()
    {
        GloballController.current.SetOffHomePanel();
    }
}
