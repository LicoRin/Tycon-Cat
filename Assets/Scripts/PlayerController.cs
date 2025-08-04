using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerPos;
    public float moveSpeed = 5f;
    private Vector3 targetPos;
    private bool isMoving = false;
    private Animator animator;

    void Start()
    {
        targetPos = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetMouseButtonDown(0))
        {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = transform.position.z;
            isMoving = true;
        }

        if (isMoving && transform.position != targetPos)
        {
            direction = (targetPos - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
        else
        {
            isMoving = false;
        }

        // Передаем параметры движения в аниматор
        if (animator != null)
        {
            animator.SetFloat("MoveX", direction.x);
            animator.SetFloat("MoveY", direction.y);
            animator.SetBool("IsMoving", isMoving);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, targetPos);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetPos, 0.2f);
    }
}
