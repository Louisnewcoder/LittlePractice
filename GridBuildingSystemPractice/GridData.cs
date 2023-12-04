using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    public Dictionary<Vector3Int, PlacementInfo> OccupiedGrids = new Dictionary<Vector3Int, PlacementInfo>();

    public void AddGridsEntry(Vector3Int selectedGrid, Vector2Int buildingSize, int infoID, int buildingID)
    {
        List<Vector3Int> targetGrids = CalculateGridPositions(selectedGrid, buildingSize);
        PlacementInfo newEntry = new PlacementInfo(targetGrids, infoID, buildingID);
        foreach (var grid in targetGrids)
        {
            if (OccupiedGrids.ContainsKey(grid))
            {
                throw new System.Exception($"The grid has been occupied {grid}");
            }
            OccupiedGrids[grid] = newEntry;
        }
    }

    public bool CheckGridAvailable(Vector3Int selectedGrid, Vector2Int buildingSize)
    {
        List<Vector3Int> targetGrids = CalculateGridPositions(selectedGrid, buildingSize);
        foreach (var grid in targetGrids)
        {
            if (OccupiedGrids.ContainsKey(grid))
            {
                return false;
            }
        }
        return true;
    }

    public List<Vector3Int> CalculateGridPositions(Vector3Int selectedGrid, Vector2Int buildingSize)
    {
        List<Vector3Int> targetGrids = new List<Vector3Int>();
        for (int i = 0; i < buildingSize.x; i++)
        {
            for (int j = 0; j < buildingSize.y; j++)
            {
                targetGrids.Add(selectedGrid + new Vector3Int(i, 0, j));
            }
        }
        return targetGrids;

    }

}

public class PlacementInfo
{
    public List<Vector3Int> OccupiedGrid;
    public int ID { get; private set; }
    public int BuildingID { get; private set; }

    public PlacementInfo(List<Vector3Int> occupiedGrid, int iD, int buildingID)
    {
        OccupiedGrid = occupiedGrid;
        ID = iD;
        BuildingID = buildingID;
    }
}