using UnityEngine;
using UnityEngine.InputSystem;

public class CameraDrag : MonoBehaviour
{
    private Vector3 origin;
    private Vector3 difference;
    private Vector2 mouseDownPosition;

    private Camera mainCamera;

    public bool isDragging;
    public static CameraDrag current { get; private set; }

    [SerializeField] public float dragThreshold = 5f; 

    private void Awake()
    {
        mainCamera = Camera.main;
        current = this;
    }

    public void OnDrag(InputAction.CallbackContext ctx)
    {
       if (!GridBuildingSystem.current.isBuildingMode)
        {
            if (ctx.started)
            {
                mouseDownPosition = Mouse.current.position.ReadValue();
                origin = GetMouseWorldPosition2D();
                isDragging = false;
            }

            if (ctx.performed)
            {
                var currentMouse = Mouse.current.position.ReadValue();
                var distance = Vector2.Distance(mouseDownPosition, currentMouse);

                if (distance > dragThreshold)
                {
                    isDragging = true;
                }
            }

            if (ctx.canceled)
            {
                isDragging = false;
            }
        }
       
    }

    private void LateUpdate()
    {
        if (!isDragging) return;

        difference = GetMouseWorldPosition2D() - transform.position;
        transform.position = origin - difference;
    }

    private Vector3 GetMouseWorldPosition2D()
    {
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        mouseScreenPos.z = 0f;
        return mainCamera.ScreenToWorldPoint(mouseScreenPos);
    }
}
