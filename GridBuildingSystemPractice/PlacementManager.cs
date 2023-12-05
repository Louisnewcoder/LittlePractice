
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public GameObject MouseIndicator;

    public Grid grid;
    public BuildingSystemInputManager inputManager;

    public GameObject GridVisual;
    private int selectedBuildingIndex = -1;
    public BuildingItemsSO biSo;

    private GridData floorTileData = new GridData(); // this is only for learning the video
    private GridData BuildingTileData = new GridData();
    private List<GameObject> deployedBuildings = new List<GameObject>();


    public PreviewManager previewManager;

    public Vector3Int lastMouseGridPos = Vector3Int.zero;

    Vector3 mousePos;
    Vector3Int cellPos;
    Vector3 cellPosWorld;




    private void Update()
    {
        // Vector3 mousePos = inputManager.GetPlaceByMouse();
        if (selectedBuildingIndex < 0)
        {
            return;
        }
        mousePos = BuildingSystemInputManager.GetPlaceByMouse();
        cellPos = grid.WorldToCell(mousePos);
        if (lastMouseGridPos != cellPos) // prevent from calculating too much, only calculate when mouse moving
        {
            MouseIndicator.transform.position = mousePos;
            cellPosWorld = grid.CellToWorld(cellPos);
            previewManager.UpdatePreview(cellPosWorld, CheckGridDeployable(cellPos, selectedBuildingIndex));
            lastMouseGridPos = cellPos; // set 2 values same, so if it won't run this part again if mouse doesn't move
        }
    }

    public void StartConstruction(int id)
    {
        EndConstruction();
        selectedBuildingIndex = biSo.buildingItems.FindIndex(b => b.ID == id);
        GridVisual.gameObject.SetActive(true);
        previewManager.ShowPreviewBuildings(biSo.buildingItems[selectedBuildingIndex].prefab, biSo.buildingItems[selectedBuildingIndex].Size);
        inputManager.OnClick += DeployBuilding;
        inputManager.OnExit += EndConstruction;
    }

    private void EndConstruction()
    {

        selectedBuildingIndex = -1;
        GridVisual.gameObject.SetActive(false);
        previewManager.HidePreviewBuildings();
        inputManager.OnClick -= DeployBuilding;
        inputManager.OnExit -= EndConstruction;
        lastMouseGridPos = Vector3Int.zero;
    }

    private void DeployBuilding()
    {
        Debug.Log("DeployBuilding called");
        if (inputManager.CheckClickOnUI())
            return;
        if (!CheckGridDeployable(cellPos, selectedBuildingIndex))
            return;

        GameObject targetBuilding = Instantiate(biSo.buildingItems[selectedBuildingIndex].prefab);
        targetBuilding.transform.position = cellPosWorld;
        deployedBuildings.Add(targetBuilding);
        GridData data = biSo.buildingItems[selectedBuildingIndex].ID == 6 ? floorTileData : BuildingTileData;
        data.AddGridsEntry(cellPos, biSo.buildingItems[selectedBuildingIndex].Size, biSo.buildingItems[selectedBuildingIndex].ID,
            data.OccupiedGrids.Count - 1);

        EndConstruction();
    }

    private bool CheckGridDeployable(Vector3Int cellPos, int selectedBuildingIndex)
    {
        GridData data = biSo.buildingItems[selectedBuildingIndex].ID == 6 ? floorTileData : BuildingTileData;
        return data.CheckGridAvailable(cellPos, biSo.buildingItems[selectedBuildingIndex].Size);
    }
}
