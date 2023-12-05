using UnityEngine;


public class PreviewManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gridIndicator;
    private Renderer gridIndicatorRenderer;
    public float indicatorCenterOffset = 0.5f;

    [SerializeField]
    private Material previewMaterial; // as Source, not for value-setting-up
    private Material previewMaterialInstance; // used for setting up value

    private GameObject previewObject;
    public float offsetYforPreviewObject = 0.06f;

    private void Awake()
    {
        previewMaterialInstance = new Material(previewMaterial);
        gridIndicatorRenderer = gridIndicator.GetComponentInChildren<Renderer>(true);
    }

    /// <summary>
    /// display Preview related gameobjects, building preview(gameobject) and grid indicator
    /// and gird indicator's size and material display with align with building preview size
    /// </summary>
    /// <param name="po">preview GameObject prefab</param>
    /// <param name="size">building size</param>
    public void ShowPreviewBuildings(GameObject po, Vector2Int size)
    {
        previewObject = Instantiate(po); // get previewObject
        PrepareGridIndicator(size); // set up Grid Indicator align with preview object
        PrepareBuildingPreview(previewObject); // set up preview object
        gridIndicator.SetActive(true); // display indicator
    }


    private void PrepareBuildingPreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer rd in renderers)
        {                                                  // !!! tricky stuff!!!
            Material[] materials = rd.materials;          // Unity [renderer.materials] gives the copy of the materials
            for (int i = 0; i < rd.materials.Length; i++)  
            {
                materials[i] = previewMaterialInstance; // so, here cannot do rd.materials[i] as it would be the copy
            }
            rd.materials = materials;                // therefore, it has to re-value it by materials declared above
        }
    }


    private void PrepareGridIndicator(Vector2Int size)
    {
        if (size!=Vector2Int.zero)
        {
            gridIndicator.transform.localScale = new Vector3Int(size.x, 1, size.y);
            gridIndicatorRenderer.material.mainTextureScale = size;   // it is the material's Tiling value
        }
    }

    public void HidePreviewBuildings()
    {
        gridIndicator.SetActive(false);
        if (previewObject!=null)
        {
            Destroy(previewObject);// later, should experiment Disable and ObjectPool
        }
      
    }

    public void UpdatePreview(Vector3 pos, bool availabilty)
    {
        MoveGridIndicator(pos);
        MovePreviewBuilding(pos);
        SwitchColorForGridIndicator(availabilty);
    }

    public void SwitchColorForGridIndicator(bool availability)
    {
        Color c = availability ? Color.white : Color.red;
        c.a = 0.4f;
        gridIndicatorRenderer.material.color = c; 
    }

    public void MovePreviewBuilding(Vector3 pos)
    {
        previewObject.transform.position = new Vector3(pos.x, pos.y+offsetYforPreviewObject, pos.z);
    }

    public void MoveGridIndicator(Vector3 pos)
    {
        gridIndicator.transform.position = new Vector3(pos.x + indicatorCenterOffset, pos.y, pos.z + indicatorCenterOffset);
    }
}
