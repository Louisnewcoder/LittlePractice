using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public GameObject MouseIndicator;
    public GameObject GridIndicator;
    public Grid grid;
    public BuildingSystemInputManager inputManager;

    public GameObject GridVisual;
    private int selectedBuildingIndex = -1;
    public BuildingItemsSO biSo;

    private GridData floorTileData = new GridData(); // this is only for learning the video
    private GridData BuildingTileData = new GridData();
    private List<GameObject> deployedBuildings = new List<GameObject>();
    private Renderer gridIndicatorRender;

    Vector3 mousePos;
    Vector3Int cellPos;
    Vector3 cellPosWorld;
    public float centerOffset = 0.5f;

    private void Awake()
    {
        gridIndicatorRender = GridIndicator.GetComponentInChildren<Renderer>();
    }

    private void Update()
    {
        // Vector3 mousePos = inputManager.GetPlaceByMouse();
        if (selectedBuildingIndex < 0)
        {
            return;
        }
        mousePos = BuildingSystemInputManager.GetPlaceByMouse();
        cellPos = grid.WorldToCell(mousePos);

        gridIndicatorRender.material.color = CheckGridDeployable(cellPos, selectedBuildingIndex) ? Color.white : Color.red;

        MouseIndicator.transform.position = mousePos;
        cellPosWorld = grid.CellToWorld(cellPos);
        GridIndicator.transform.position = new Vector3(cellPosWorld.x + centerOffset, cellPosWorld.y, cellPosWorld.z + centerOffset);
    }

    public void StartConstruction(int id)
    {
        //EndConstruction();
        selectedBuildingIndex = biSo.buildingItems.FindIndex(b => b.ID == id);
        GridVisual.gameObject.SetActive(true);
        GridIndicator.gameObject.SetActive(true);
        inputManager.OnClick += DeployBuilding;
        inputManager.OnExit += EndConstruction;
    }

    private void EndConstruction()
    {
        selectedBuildingIndex = -1;
        GridVisual.gameObject.SetActive(false);
        GridIndicator.gameObject.SetActive(false);
        inputManager.OnClick -= DeployBuilding;
        inputManager.OnExit -= EndConstruction;
    }

    private void DeployBuilding()
    {

        if (inputManager.CheckClickOnUI())
        {
            return;
        }
        if (!CheckGridDeployable(cellPos, selectedBuildingIndex))
            return;

        GameObject targetBuilding = Instantiate(biSo.buildingItems[selectedBuildingIndex].prefab);
        targetBuilding.transform.position = cellPosWorld;
        deployedBuildings.Add(targetBuilding);
        GridData data = biSo.buildingItems[selectedBuildingIndex].ID == 6 ? floorTileData : BuildingTileData;

        data.AddGridsEntry(cellPos, biSo.buildingItems[selectedBuildingIndex].Size, biSo.buildingItems[selectedBuildingIndex].ID,
            data.OccupiedGrids.Count - 1);
        // print(data.OccupiedGrids[cellPos].ID); check it out if the first ID is -1 ,and it is
        EndConstruction();
    }

    private bool CheckGridDeployable(Vector3Int cellPos, int selectedBuildingIndex)
    {
        GridData data = biSo.buildingItems[selectedBuildingIndex].ID == 6 ? floorTileData : BuildingTileData;
        return data.CheckGridAvailable(cellPos, biSo.buildingItems[selectedBuildingIndex].Size);
    }
}
