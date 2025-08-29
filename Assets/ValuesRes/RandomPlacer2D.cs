using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class RandomPlacer2D : MonoBehaviour
{
    [Header("Префабы (деревья, камни и т.п.)")]
    public GameObject[] prefabs;

    [Header("Зона расстановки (в мировых координатах)")]
    public Vector2 areaSize = new Vector2(20f, 10f);
    public Vector2 areaCenterOffset = Vector2.zero;

    [Header("Количество")]
    public int count = 50;

    [Header("Случайный поворот вокруг Z (градусы)")]
    public Vector2 rotationZRange = Vector2.zero; 

    [Header("Анти-накладки ")]
    public LayerMask overlapMask = ~0;
    public float minDistance = 0.5f;
    public int maxAttemptsPerItem = 25;

    [Header("Куда складывать результат")]
    public Transform container;

    [Tooltip("0 = по-настоящему случайно. Любое число — фиксированный сид.")]
    public int seed = 0;

    public Rect GetRectWorld()
    {
        Vector2 center = (Vector2)transform.position + areaCenterOffset;
        return new Rect(center - areaSize * 0.5f, areaSize);
    }

#if UNITY_EDITOR
    public void Clear()
    {
        if (!container) return;
        var toDestroy = new System.Collections.Generic.List<GameObject>();
        foreach (Transform child in container) toDestroy.Add(child.gameObject);
        foreach (var go in toDestroy) Undo.DestroyObjectImmediate(go);
    }

    public void Place()
    {
        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogWarning("RandomPlacer2D: Не заданы префабы.");
            return;
        }

        if (!container)
        {
            container = new GameObject(name + "_PLACED").transform;
            Undo.RegisterCreatedObjectUndo(container.gameObject, "Create container");
            container.SetParent(transform);
            container.localPosition = Vector3.zero;
        }

        var rect = GetRectWorld();
        var rand = (seed != 0) ? new System.Random(seed) : new System.Random();

        int placed = 0;
        int guard = 0;

        while (placed < count && guard < count * maxAttemptsPerItem)
        {
            guard++;

            float x = (float)(rect.xMin + rand.NextDouble() * rect.width);
            float y = (float)(rect.yMin + rand.NextDouble() * rect.height);
            var pos = new Vector3(x, y, 0f);

         
            if (minDistance > 0f)
            {
                var hits = Physics2D.OverlapCircleAll(pos, minDistance, overlapMask);
                if (hits != null && hits.Length > 0) continue;

                bool tooClose = false;
                foreach (Transform child in container)
                {
                    if (Vector2.Distance(child.position, pos) < minDistance) { tooClose = true; break; }
                }
                if (tooClose) continue;
            }

            var prefab = prefabs[rand.Next(0, prefabs.Length)];
            if (!prefab) continue;

            GameObject instance = null;

#if UNITY_EDITOR
            instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
#endif
            if (!instance) instance = Instantiate(prefab);

            Undo.RegisterCreatedObjectUndo(instance, "Place object");
            instance.transform.SetParent(container, false);
            instance.transform.position = pos;

   
            float zRot = Mathf.Lerp(rotationZRange.x, rotationZRange.y, (float)rand.NextDouble());
            instance.transform.rotation = Quaternion.Euler(0f, 0f, zRot);

    
            instance.transform.localScale = prefab.transform.localScale;

            placed++;
        }

        EditorUtility.SetDirty(container);
        EditorSceneManager.MarkSceneDirty(container.gameObject.scene);
        Debug.Log($"RandomPlacer2D: Расставлено {placed} объектов.");
    }
#endif

    private void OnDrawGizmosSelected()
    {
        var rect = GetRectWorld();
        Gizmos.color = new Color(0f, 1f, 0f, 0.15f);
        Gizmos.DrawCube(rect.center, rect.size);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(rect.center, rect.size);
    }
}
