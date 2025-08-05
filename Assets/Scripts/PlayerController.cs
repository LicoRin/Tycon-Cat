using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems; // <-- ��� �������� UI ������

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Camera mainCamera;

    private NavMeshAgent agent;
    private Vector2 moveDir;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // ��������� ������ ��� 2D
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
    /// ��������� ����� ���� � ��������� ����� ����������
    /// </summary>
    void HandleClickToMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ���� ���� �� UI, ����������
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            // Raycast2D ��� ������
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
            if (hit.collider != null)
            {
                agent.SetDestination(hit.point);
            }
            else
            {
                agent.SetDestination(mouseWorldPos);
            }
        }
    }

    /// <summary>
    /// ��������� ����������� ��������
    /// </summary>
    void UpdateMovement()
    {
        if (agent.hasPath)
        {
            Vector2 desiredVel = agent.desiredVelocity.normalized;

            // ������������ �������� �� 4 ������������
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
    /// ���������� �������� ��������
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
