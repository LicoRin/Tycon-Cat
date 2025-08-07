using UnityEngine;
using NavMeshPlus.Components;

[RequireComponent(typeof(NavMeshSurface))]
public class RuntimeNavMeshBaker : MonoBehaviour
{
    private NavMeshSurface surface;
    public static RuntimeNavMeshBaker current;

    void Awake()
    {
        surface = GetComponent<NavMeshSurface>();
        current = this;
    }

    // Метод, который можно вызвать из UI кнопки
    public void BakeNavMesh()
    {
        surface.BuildNavMesh();
        Debug.Log("NavMesh перестроен во время игры!");
    }
}
