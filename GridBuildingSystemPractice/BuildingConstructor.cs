using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// place building in the scene and leave a record, remove the building in the scene and delete the value from the record
/// </summary>
public class BuildingConstructor : MonoBehaviour
{
    // contains which building is built and what Entry ID it has
    private List<GameObject> deployedBuildings = new List<GameObject>();

    /// <summary>
    /// place a building in the scene at the position represented by parameter,and return its Entry ID in the GridData Container
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public int ConsturctOnGrid(GameObject prefab, Vector3 pos)
    {
        GameObject targetBuilding = Instantiate(prefab);
        targetBuilding.transform.position = pos;
        deployedBuildings.Add(targetBuilding); // add instantiated building into the list
        return deployedBuildings.Count - 1; 
    }
    /// <summary>
    /// Delete Building from the scene and clear the value of its original index
    /// </summary>
    /// <param name="deployedBuildingIndex"></param>
    internal void DemlishOnGrid(int deployedBuildingIndex)
    {
        if (deployedBuildings.Count <= deployedBuildingIndex || deployedBuildings[deployedBuildingIndex]==null)
            return;
        Destroy(deployedBuildings[deployedBuildingIndex]);
        deployedBuildings[deployedBuildingIndex] = null;
    }

}

