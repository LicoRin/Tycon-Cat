using Unity.VisualScripting;
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

        if (Input.GetMouseButtonUp(0) && mousePressed)
        {
            float distance = Vector2.Distance(mouseDownPos, Input.mousePosition);

            if (distance < CameraDrag.current.dragThreshold && !GridBuildingSystem.current.isBuildingMode)
            {
                Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0f;

                RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
                if (hit.collider != null)
                {
                    // Проверка на UI (включая родителей)
                    Transform current = hit.collider.transform;
                    while (current != null)
                    {
                        if (current.CompareTag("UI"))
                            return;
                        current = current.parent;
                    }

                    // Проверка на Resource
                    current = hit.collider.transform;
                    bool isResource = false;

                    while (current != null)
                    {
                        if (current.CompareTag("Resource"))
                        {
                            isResource = true;
                            break;
                        }
                        current = current.parent;
                    }

                    Vector3 targetPos = hit.point;

                    targetPos.z = 0f;

                    if (isResource)
                    {
                        targetPos = hit.collider.transform.position;
                        targetPos.z = 0f;

                        NavMeshHit navHit;
                        if (NavMesh.Raycast(agent.transform.position, targetPos, out navHit, NavMesh.AllAreas))
                        {
                            agent.SetDestination(navHit.position);
                            Debug.DrawLine(agent.transform.position, navHit.position, Color.red, 1f);
                            Debug.Log("NavMesh: путь прерван — идём до границы объекта.");
                        }
                        else
                        {
                            agent.SetDestination(targetPos);
                            Debug.DrawLine(agent.transform.position, targetPos, Color.green, 1f);
                            Debug.Log("NavMesh: путь свободен — идём к ресурсу.");
                        }

                        mousePressed = false;
                        return;
                    }
                }

                // Клик не по ресурсу — просто идём
                agent.SetDestination(mouseWorldPos);
                Debug.DrawLine(agent.transform.position, mouseWorldPos, Color.yellow, 1f);
                Debug.Log("NavMesh: Просто идём.");
            }

            mousePressed = false;
        }
    }




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
