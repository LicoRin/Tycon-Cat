using UnityEngine;
using UnityEngine.AI;
using NavMeshPlus;
using NavMeshPlus.Components;

[RequireComponent(typeof(NavMeshSurface))]
public class RuntimeNavMeshBaker : MonoBehaviour
{
    public NavMeshSurface surface;

    public void BakeRuntime()
    {
        surface = GetComponent<NavMeshSurface>();

        // Строим навмеш в рантайме
        surface.BuildNavMesh();
    }
}