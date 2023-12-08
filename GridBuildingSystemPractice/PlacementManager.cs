using UnityEngine;

/// <summary>
/// overall organizer, it conveys data and orders
/// </summary>
public class PlacementManager : MonoBehaviour
{
    public BuildingSystemInputManager inputManager; // used to get position and implement click event
    public Grid grid; // used to call its public methods to convert world vector to grid vector
    public GameObject GridVisual; // used to show grids on the map

    public BuildingItemsSO biSo; // building database

    private GridData floorTileData = new GridData(); // this is only for learning the video, save floor type buildings
    private GridData buildingTileData = new GridData(); //  save normal type buildings

    public PreviewManager previewManager;   // used to execute displaying buildingpreview, grid indicators
    public BuildingConstructor buildingConstructor;  // used to implement deploy or remove buildings in the scene

    private IBuildingState buildingState; // used to implement deploying/demolishing behavior

    private Vector3Int lastMouseGridPos = Vector3Int.zero; // mark last grid where the mouse hung over, used to prevent Update Computing cost

    Vector3 mousePos; // save mousePosition from InputManager
    Vector3Int cellPos; // save grid position which the Mouse is hanging over




    private void Update()
    {

        if (buildingState == null) // if no any state active then stop updating
            return;
        mousePos = BuildingSystemInputManager.GetPlaceByMouse(); // get mouse position
        cellPos = grid.WorldToCell(mousePos); // convert mouse position vector3 to grid position Vector3Int
        if (lastMouseGridPos != cellPos) // prevent from calculating too much, only calculate when mouse moving
        {
            buildingState.UpdateState(cellPos); // update behavior based on a specific state
            lastMouseGridPos = cellPos; // set 2 values same, so if it won't run this part again if mouse doesn't move
        }
    }

    /// <summary>
    /// activate Deploy state
    /// </summary>
    /// <param name="id"> building ID</param>
    public void StartConstruction(int id)
    {
        EndConstruction();
        GridVisual.gameObject.SetActive(true);

        // initialize Deploy state
        buildingState = new DeployState(id, previewManager, biSo, grid, buildingConstructor, floorTileData, buildingTileData);

        inputManager.OnClick += DeployBuilding;  // register construction behavior
        inputManager.OnExit += EndConstruction;  // register End-construction behavior
    }

    /// <summary>
    /// activate Demolish state
    /// </summary>
    public void StartDemolish()
    {
        EndConstruction();
        GridVisual.gameObject.SetActive(true);

        // initialize Demolish state
        buildingState = new DemolishState(previewManager, /*grid,*/ buildingConstructor, floorTileData, buildingTileData);

        inputManager.OnClick += DeployBuilding;  // register construction behavior
        inputManager.OnExit += EndConstruction; // register End-construction behavior
    }

    private void EndConstruction()
    {
        if (buildingState == null)  // if there is no any state is active then no needs to keep working
            return;
        GridVisual.gameObject.SetActive(false);  // turn off grid visualization

        buildingState.EndState(); // call state's endstate method

        inputManager.OnClick -= DeployBuilding;     // deregister construction behavior
        inputManager.OnExit -= EndConstruction;     // deregister End-construction behavior
        lastMouseGridPos = Vector3Int.zero;  // set up last grid position as default
         
        buildingState = null;  // clear state reference
    }

    private void DeployBuilding()  // Construction behavior, actually the name of the method is not appropriate after refatoring
    {
        if (inputManager.CheckClickOnUI())  // if clicked on UI then do nothing
            return;

        buildingState.OnAction(cellPos); // act based on a spcific state

        EndConstruction(); 
    }

}
