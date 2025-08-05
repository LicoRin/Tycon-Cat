using UnityEngine;
using UnityEngine.AI;
using NavMeshPlus;
using NavMeshPlus.Components;

[RequireComponent(typeof(NavMeshSurface))]
public class RuntimeNavMeshBaker : MonoBehaviour
{
    private NavMeshSurface surface;

    void Update()
    {
        surface = GetComponent<NavMeshSurface>();

        // Строим навмеш в рантайме
        surface.BuildNavMesh();
    }
}