using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemolishState : IBuildingState
{
    private int deployedBuildingIndex = -1;
    private PreviewManager previewManager;
    //private Grid grid;
    private BuildingConstructor buildingConstructor;
    private GridData floorTileData;
    private GridData BuildingTileData;

    public DemolishState(PreviewManager previewManager, /*Grid grid,*/ BuildingConstructor buildingConstructor, GridData floorTileData, GridData buildingTileData)
    {
        this.previewManager = previewManager;
    //    this.grid = grid;
        this.buildingConstructor = buildingConstructor;
        this.floorTileData = floorTileData;
        BuildingTileData = buildingTileData;

        //create a new method used to show "demolishIndicator" in previewManager named ShowDemolisher(), and call it 

        previewManager.ShowDemolisher();
    }

    public void EndState()
    {
        previewManager.HidePreviewBuildings();
    }

    /// <summary>
    /// demolish a building on a specific position
    /// </summary>
    /// <param name="cellPos"></param>
    public void OnAction(Vector3Int cellPos)
    {
        // first check if there is anything at the position
         GridData currentGrid = null; 
        if (!BuildingTileData.CheckGridAvailable(cellPos, Vector2Int.one))
        {
            currentGrid = BuildingTileData;
        }
        else if (!floorTileData.CheckGridAvailable(cellPos, Vector2Int.one))
        { currentGrid = floorTileData; }

        if (currentGrid == null)
        {
            Debug.Log($"Nothing on this grid {cellPos}");
            // play a visual effect or sound
        }
        else   
        {   // then get its entry id
            deployedBuildingIndex = currentGrid.GetbuildingEntryID(cellPos);
            if (deployedBuildingIndex == -1)
                return;
            currentGrid.RemoveBuildingByGrid(cellPos);   // clear all the keys (positions the building occupied) relevant to the buiding 
            buildingConstructor.DemlishOnGrid(deployedBuildingIndex); // clear value of its index 
        }
        // because I turn off actions after deploying or demolishing a building , so I don't need to update position here
        // so I don't need field:grid and 
       // previewManager.UpdatePreview(cellPos, CheckDemolishable(cellPos));
    }

    /// <summary>
    /// check if selected position is occupied by a building
    /// </summary>
    /// <param name="cellPos"></param>
    /// <returns></returns>
    private bool CheckDemolishable(Vector3Int cellPos)
    {
        // it is not available for all kinds of griddata, then it means there is something there
        return !(floorTileData.CheckGridAvailable(cellPos, Vector2Int.one)&& BuildingTileData.CheckGridAvailable(cellPos, Vector2Int.one));
    }

    /// <summary>
    /// update indicator's position and color
    /// </summary>
    /// <param name="cellPos"></param>
    public void UpdateState(Vector3Int cellPos)
    {
        previewManager.UpdatePreview(cellPos, CheckDemolishable(cellPos));
    }

}
