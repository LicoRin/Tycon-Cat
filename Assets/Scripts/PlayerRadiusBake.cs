using UnityEngine;

public class PlayerRadiusBake : MonoBehaviour
{
    public Collider2D zoneRadius;
    public LayerMask objectRender;

    void Start()
    {
        
    }
    public void Bake()
    {
        if (zoneRadius == null) return;
        var bounds = zoneRadius.bounds;
        var colliders = Physics2D.OverlapBoxAll(bounds.center, bounds.size, 0f, objectRender);
        foreach (var col in colliders)
        {
            var go = col.gameObject;
            var resources = go.GetComponent<Collider2D>();
            if (resources != null)
            {
                resources.enabled = false;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
