using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    
    
    public GameObject placeCancelButtons;
    
    public bool Placed { get; private set; }
    public BoundsInt area;
    private Collider2D collider2d;

    Vector3 offset;

    void Awake()
    {
        collider2d = GetComponent<Collider2D>();
    }

    void OnMouseDown()
    {
        offset = transform.position - MouseWorldPosition();
    }

    void OnMouseDrag()
    {

        if (!Placed)
        {
           
            Vector3 mousePos = MouseWorldPosition() + offset;
            Vector3Int cellPos = GridBuildingSystem.current.gridLayout.LocalToCell(mousePos);

           
            transform.position = GridBuildingSystem.current.gridLayout.CellToLocalInterpolated(
                cellPos + new Vector3(.5f, .5f, 0f)
            );

            GridBuildingSystem.current.FollowBuilding();
        }
       
    }

 

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }


    #region Build Methods
    public bool CanBePlaced()
    {
      Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        if (GridBuildingSystem.current.CanTakeArea(areaTemp))
        {
            return true;
        }
        return false;
    }

    public void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        Placed = true;
        GridBuildingSystem.current.TakeArea(areaTemp);

    }


    public void PlaceBuild()
    {
       if (!CanBePlaced())
        {
            Debug.LogError("Cannot place building here!");
            return;
        }else
        {
            GridBuildingSystem.current.SetBuild();

            var navMeshBaker = FindObjectOfType<RuntimeNavMeshBaker>();
            if (navMeshBaker != null)
            {
                navMeshBaker.BakeNavMesh();
            }

            
        }
       

    }
    public void SetOffButtons()
    {

        if (Placed)
        {
            placeCancelButtons.SetActive(false);
            GridBuildingSystem.current.isBuildingMode = false;
            GloballController.current.shopButton.SetActive(true);
        }
        else
        {
            placeCancelButtons.SetActive(true);
            GloballController.current.shopButton.SetActive(true);
            GridBuildingSystem.current.isBuildingMode = true;
        }
    }
    public void CancelBuild()
    {
        GridBuildingSystem.current.UnsetBuild();
        GloballController.current.shopButton.SetActive(true);
        GridBuildingSystem.current.isBuildingMode = false;

    }

    #endregion
}
