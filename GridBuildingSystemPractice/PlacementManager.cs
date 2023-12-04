using System;
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

    Vector3 mousePos;
    Vector3Int cellPos;
    Vector3 cellPosWorld;

    public float centerOffset = 0.5f;

    private void Update()
    {
        // Vector3 mousePos = inputManager.GetPlaceByMouse();
        if (selectedBuildingIndex < 0)
        {
            return;
        }
        mousePos = BuildingSystemInputManager.GetPlaceByMouse();
        cellPos = grid.WorldToCell(mousePos);
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
        GameObject targetBuilding = Instantiate(biSo.buildingItems[selectedBuildingIndex].prefab);
        targetBuilding.transform.position = cellPosWorld;
        EndConstruction();
    }
}
