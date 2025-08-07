using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems; // <-- для проверки UI кликов

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Camera mainCamera;
    private Vector2 mouseDownPos;

    private NavMeshAgent agent;
    private Vector2 moveDir;
    private bool mousePressed = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // Настройка агента для 2D
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        if (animator == null)
            animator = GetComponent<Animator>();

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        HandleClickToMove();
        UpdateMovement();
        UpdateAnimation();
    }

    /// <summary>
    /// Обработка клика мыши и установка точки назначения
    /// </summary>
    void HandleClickToMove()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            mousePressed = true;
            mouseDownPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0) )
        {
            if (mousePressed)
            {
                float distance = Vector2.Distance(mouseDownPos, Input.mousePosition);

                // Если мышь почти не двигалась — это клик, а не drag
                if (distance < CameraDrag.current.dragThreshold && !GridBuildingSystem.current.isBuildingMode)
                {
                    Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    mouseWorldPos.z = 0f;

                    RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
                    if (hit.collider != null)
                    {
                        // Проверка на тег UI у объекта и его родителей
                        Transform current = hit.collider.transform;
                        while (current != null)
                        {
                            if (current.CompareTag("UI"))
                            {
                                return; // Не перемещаемся
                            }
                            current = current.parent;
                        }
                    
                       
                    }
                    else
                    {
                        agent.SetDestination(mouseWorldPos);
                    }
                }
            }

            mousePressed = false;
        }
    

}  /// <summary>
        /// Обновляем направление движения
        /// </summary>
        void UpdateMovement()
    {
        if (agent.hasPath)
        {
            Vector2 desiredVel = agent.desiredVelocity.normalized;

            // Ограничиваем движение по 4 направлениям
            if (Mathf.Abs(desiredVel.x) > Mathf.Abs(desiredVel.y))
                moveDir = new Vector2(Mathf.Sign(desiredVel.x), 0f);
            else
                moveDir = new Vector2(0f, Mathf.Sign(desiredVel.y));
        }
        else
        {
            moveDir = Vector2.zero;
        }
    }

    /// <summary>
    /// Обновление анимации движения
    /// </summary>
    void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetFloat("MoveX", moveDir.x);
            animator.SetFloat("MoveY", moveDir.y);
            animator.SetBool("IsMoving", moveDir.sqrMagnitude > 0.01f);
        }
    }
}
