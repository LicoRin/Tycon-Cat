using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    [Tooltip("Объекты, которые будут активироваться в этой зоне")]
    public GameObject[] objectsToActivate;

    [Tooltip("Радиус активации зоны")]
    public float activationRadius = 5f;

    [Tooltip("Ссылка на игрока (если не задана — ищется по тегу Player)")]
    public Transform player;

    private bool isActive = false;

    void Start()
    {
        // Деактивируем все объекты при старте
        foreach (var obj in objectsToActivate)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        if (player == null)
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (!isActive && dist <= activationRadius)
        {
            SetObjectsActive(true);
            isActive = true;
        }
        else if (isActive && dist > activationRadius)
        {
            SetObjectsActive(false);
            isActive = false;
        }
    }

    private void SetObjectsActive(bool active)
    {
        foreach (var obj in objectsToActivate)
        {
            if (obj != null)
                obj.SetActive(active);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.15f);
        Gizmos.DrawSphere(transform.position, activationRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, activationRadius);
    }
}