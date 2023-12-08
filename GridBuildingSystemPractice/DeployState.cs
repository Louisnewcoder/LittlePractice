
using UnityEngine;

/// <summary>
/// orgnaize gameobjects and data to deploy buildings during building process
/// </summary>
public class DeployState : IBuildingState
{

    private int selectedBuildingIndex = -1;
    int ID;

    private PreviewManager previewManager;
    private BuildingItemsSO biSo;

    private Grid grid;
    private BuildingConstructor buildingConstructor;

    private GridData floorTileData;
    private GridData BuildingTileData;

    public DeployState(int iD,
                       PreviewManager previewManager,
                       BuildingItemsSO biSo,
                       Grid grid,
                       BuildingConstructor buildingConstructor,
                       GridData floorTileData,
                       GridData buildingTileData)
    {
        ID = iD;
        this.previewManager = previewManager;
        this.biSo = biSo;
        this.grid = grid;
        this.buildingConstructor = buildingConstructor;
        this.floorTileData = floorTileData;
        BuildingTileData = buildingTileData;

        selectedBuildingIndex = biSo.buildingItems.FindIndex(b => b.ID == iD);
        if (selectedBuildingIndex > -1) // if selectedbuilding index is valid,  ask preview manager to show previews of building and indicator
        {
            previewManager.ShowPreviewBuildings(biSo.buildingItems[selectedBuildingIndex].prefab, biSo.buildingItems[selectedBuildingIndex].Size);

        }
        else
            throw new System.Exception($"The building ID is not valid, it is {iD}");

    }

    /// <summary>
    /// when building process ended ,hide previews of building and indicator
    /// </summary>
    public void EndState()
    {
        previewManager.HidePreviewBuildings();
    }

    /// <summary>
    /// deploy a building at selected position
    /// </summary>
    /// <param name="cellPos"></param>
    public void OnAction(Vector3Int cellPos)
    {
        if (!CheckGridDeployable(cellPos, selectedBuildingIndex)) // check if selected position is available for a new building
            return;

        // deploy a building and get its entry id
        int entryID = buildingConstructor.ConsturctOnGrid(biSo.buildingItems[selectedBuildingIndex].prefab, grid.CellToWorld(cellPos));
        // check the building belongs to which type of griddata 
        GridData data = biSo.buildingItems[selectedBuildingIndex].ID == 6 ? floorTileData : BuildingTileData;
        // save the building info 
        data.AddGridsEntry(cellPos, biSo.buildingItems[selectedBuildingIndex].Size, biSo.buildingItems[selectedBuildingIndex].ID,
                entryID);
    }

    /// <summary>
    /// update previews of building and indicator  positions and color
    /// </summary>
    /// <param name="cellPos"></param>
    public void UpdateState(Vector3Int cellPos)
    {
        previewManager.UpdatePreview(grid.CellToWorld(cellPos), CheckGridDeployable(cellPos, selectedBuildingIndex));
    }

    /// <summary>
    /// check if selected data is available
    /// </summary>
    /// <param name="cellPos"></param>
    /// <param name="selectedBuildingIndex"></param>
    /// <returns></returns>
    private bool CheckGridDeployable(Vector3Int cellPos, int selectedBuildingIndex)
    {
        GridData data = biSo.buildingItems[selectedBuildingIndex].ID == 6 ? floorTileData : BuildingTileData;
        return data.CheckGridAvailable(cellPos, biSo.buildingItems[selectedBuildingIndex].Size);
    }
}