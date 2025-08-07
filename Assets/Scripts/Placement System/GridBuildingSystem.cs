using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;


public class GridBuildingSystem : MonoBehaviour
{


    [Header("Tile Assets")]

    [SerializeField] private TileBase whiteTile;
    [SerializeField] private TileBase greenTile;
    [SerializeField] private TileBase redTile;
    public static GridBuildingSystem current;

    public GameObject buildingTilemap;
    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    private Building temp;
    private Vector3 prevPos;

    private BoundsInt prevArea;

    public bool isBuildingMode= false;

    #region Unity Methods

    private void Awake()
    {
        current = this;
    }
  
    private void Start()
    {
        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.White, whiteTile);
        tileBases.Add(TileType.Green, greenTile);
        tileBases.Add(TileType.Red, redTile);
    }

    private void Update()
    {
        if (!temp)
        {
            return;
        }


        /*if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return; // Ignore clicks on UI elements
            }
            if (!temp.Placed)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Vector3Int cellPos = gridLayout.LocalToCell(touchPos);


                if (prevPos != cellPos)
                {
                    temp.transform.position = gridLayout.CellToLocalInterpolated(cellPos
                        + new Vector3(.5f, .5f, 0f));
                    prevPos = cellPos;
                    FollowBuilding();
                }
            }
        }*/
       
    }




    public void SetBuild()
    {
        if (temp.CanBePlaced())
        {
            temp.Place();
            buildingTilemap.SetActive(false);


        }
    }
    public void UnsetBuild()
    {
        ClearArea();
        Destroy(temp.gameObject);
        buildingTilemap.SetActive(false);
    }


    #endregion


    #region Tilemap Management
    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, v.z);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }
        return array;
    }


    private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        int size = area.size.x * area.size.y * area.size.z;
        TileBase[] tileArray = new TileBase[size];
        FillTiles(tileArray, type);
        tilemap.SetTilesBlock(area, tileArray);
    }


    private static void FillTiles(TileBase[] arr, TileType type)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBases[type];
        }
    }


    #endregion

    #region Building Placement

    public void InitializeWithBuilding(GameObject buildingPrefab)
{

    // Берем позицию мыши
    Vector3 mousePosition = Input.mousePosition;
    mousePosition.z = 10f; // расстояние до камеры
    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Выравниваем по центру клетки
        Vector3Int cellPosition = gridLayout.WorldToCell(worldPosition);
        Vector3 snappedPosition = gridLayout.CellToWorld(cellPosition)
                                + 0.5f * gridLayout.cellSize;


        // Спавним объект уже в центре клетки
        temp = Instantiate(buildingPrefab, snappedPosition, Quaternion.identity)
                 .GetComponent<Building>();

    // Устанавливаем область в ту же клетку
    temp.area.position = cellPosition;

    FollowBuilding();
    buildingTilemap.SetActive(true);
}



    private void ClearArea()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        FillTiles(toClear, TileType.Empty);
        TempTilemap.SetTilesBlock(prevArea, toClear);


    }

    public void  FollowBuilding()
    {
        isBuildingMode = true;
        ClearArea();
        temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
        BoundsInt buildingArea = temp.area;

        TileBase[] baseArray = GetTilesBlock(buildingArea, MainTilemap);

        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];


        for (int i = 0; i < baseArray.Length; i++)
        {
          if (baseArray[i] == tileBases[TileType.White])
            {
             tileArray[i] = tileBases[TileType.Green];
            }
            else
            {
                FillTiles(tileArray, TileType.Red);
                break;
            }


        }
        TempTilemap.SetTilesBlock(buildingArea, tileArray);
        prevArea = buildingArea;
    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
        
        foreach(var b in baseArray)
        {
            if(b != tileBases[TileType.White] )
            {
                Debug.Log("Cannot place building here, area is not empty");
                return false;
            }
        }
        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.Empty, TempTilemap);
        SetTilesBlock(area, TileType.Green, MainTilemap);
    }

    #endregion
}

public enum TileType
{
    Empty,
    White,
    Green,
    Red
}
