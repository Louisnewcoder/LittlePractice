using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// it stores the whole information of a piece of "Land", it describes which piece of "land" stores what kind of "building"
/// it also provides methods to add new information entry(new building, built on where), to check some piece of land is available or not
/// and Calculate required size of land based on target building's size
/// as well as retrieve building EntryID and delete entries
/// </summary>
public class GridData
{
    /// <summary>
    /// information container, the key is "land" coordinates,the value is "building's detail"
    /// </summary>
    public Dictionary<Vector3Int, PlacementInfo> OccupiedGrids = new Dictionary<Vector3Int, PlacementInfo>();

    /// <summary>
    /// when a building is deployed on the "land", add a new entry to the info container
    /// </summary>
    /// <param name="selectedGrid"> (where Mouse is placed)origin coordinate of target place to build</param>
    /// <param name="buildingSize"> how big the building is</param>
    /// <param name="infoID"> index of this new entry in the container</param>
    /// <param name="buildingID"> building's id in the whole buildable list</param>
    /// <exception cref="System.Exception"></exception>
    public void AddGridsEntry(Vector3Int selectedGrid, Vector2Int buildingSize, int buildingID, int infoID)
    {
        List<Vector3Int> targetGrids = CalculateGridPositions(selectedGrid, buildingSize);
        PlacementInfo newEntry = new PlacementInfo(targetGrids, infoID, buildingID);
        foreach (var grid in targetGrids)  // check each coordinate ,if it is available, then add it as a key of the container, then save the new entry as its value
        {
            if (OccupiedGrids.ContainsKey(grid)) // check if a key has existed already
            {
                throw new System.Exception($"The grid has been occupied {grid}"); // if so, throw an exception
            }
            OccupiedGrids[grid] = newEntry;
        }
        // after the method, there will be one or several keys reference(s) a same value, which is the building's info
    }
    /// <summary>
    /// check if the set of coordinates of target piece of land is available , means any of them don't exist in the container
    /// </summary>
    /// <param name="selectedGrid">(where Mouse is placed)origin coordinate of target place to build</param>
    /// <param name="buildingSize">building's size</param>
    /// <returns></returns>
    public bool CheckGridAvailable(Vector3Int selectedGrid, Vector2Int buildingSize)
    {
        List<Vector3Int> targetGrids = CalculateGridPositions(selectedGrid, buildingSize); // get target cooridnates
        foreach (var grid in targetGrids) // traverse each coordinate of target place,to see if any of them existed in the container
        {
            if (OccupiedGrids.ContainsKey(grid))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// calculate which set of corrdinates is need for target building, based on origin of target coordinate and building size
    /// </summary>
    /// <param name="selectedGrid">(where Mouse is placed)origin coordinate of target place to build</param>
    /// <param name="buildingSize">building's size</param>
    /// <returns></returns>
    public List<Vector3Int> CalculateGridPositions(Vector3Int selectedGrid, Vector2Int buildingSize)
    {
        List<Vector3Int> targetGrids = new List<Vector3Int>();
        for (int i = 0; i < buildingSize.x; i++)  // use building size's length and width as each layer of loops limit
        {
            for (int j = 0; j < buildingSize.y; j++)
            {
                targetGrids.Add(selectedGrid + new Vector3Int(i, 0, j));
            }
        }
        return targetGrids;

    }

    /// <summary>
    /// If any building exists on appointed grid return its EntryID
    /// </summary>
    /// <param name="cellPos"></param>
    /// <returns>EntryID is cooresponding with the building's index in Building list records of BuildingConstructor</returns>
    internal int GetbuildingEntryID(Vector3Int cellPos)
    {
        if (!OccupiedGrids.ContainsKey(cellPos)) //  if the container doesn't contain this key, means there is no building on the grid
            return -1; // return -1 as invalid ID
        return OccupiedGrids[cellPos].ID; // if found a building, then return its EntryID
    }

    /// <summary>
    /// Delete the set of information of a building in the container
    /// </summary>
    /// <param name="cellPos"></param>
    internal void RemoveBuildingByGrid(Vector3Int cellPos)
    {
        foreach (Vector3Int pos in OccupiedGrids[cellPos].OccupiedGrid) //find all of the keys relevant to a building
        {
            OccupiedGrids.Remove(pos);
        }
    }
}

/// <summary>
/// entry information, contains a set of coordinates a building is occupying, entry ID , and Building's ID
/// </summary>
public class PlacementInfo
{
    public List<Vector3Int> OccupiedGrid;   // all grids on which a bulding is placed
    public int ID { get; private set; }     // Entry index of this info in the container
    public int BuildingID { get; private set; }     // building ID in Building database

    public PlacementInfo(List<Vector3Int> occupiedGrid, int iD, int buildingID)
    {
        OccupiedGrid = occupiedGrid;
        ID = iD;
        BuildingID = buildingID;
    }
}