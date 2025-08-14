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

  
    public void BakeNavMesh()
    {
        surface.BuildNavMesh();
        Debug.Log("NavMesh ���������� �� ����� ����!");
    }
}
