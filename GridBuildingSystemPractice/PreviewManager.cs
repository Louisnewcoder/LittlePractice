using UnityEngine;

/// <summary>
/// taking care of show and hide grid indicator and preview-building gameobject, when its being called
/// </summary>
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
        gridIndicator.SetActive(true);  // display indicator
    }

    /// <summary>
    /// Set up all materials of the displayed gameobjects with the transparent material
    /// </summary>
    /// <param name="previewObject">Gameobject which need to be displayed as a preview</param>
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

    /// <summary>
    /// set up GridIndicator's scale and material Tiling value align with displayed preview-GameObject
    /// </summary>
    /// <param name="size"></param>
    private void PrepareGridIndicator(Vector2Int size)
    {
        if (size!=Vector2Int.zero)
        {
            gridIndicator.transform.localScale = new Vector3Int(size.x, 1, size.y);
            gridIndicatorRenderer.material.mainTextureScale = size;   // it is the material's Tiling value
        }
    }

    /// <summary>
    /// Hide GridIndicator and destroy preview-GameObject if there is one
    /// </summary>
    public void HidePreviewBuildings()
    {
        gridIndicator.SetActive(false);
        if (previewObject!=null)
        {
            Destroy(previewObject);// later, should experiment Disable and ObjectPool
        }
      
    }

    /// <summary>
    /// Update preview's position and GridIndicator's position, as well as switch GridIndicator's color depends on availability of the grid
    /// </summary>
    /// <param name="pos">target position</param>
    /// <param name="availabilty"> availability of the grid</param>
    public void UpdatePreview(Vector3 pos, bool availabilty)
    {
        if (previewObject != null)  // if preview object is instantiated (not null)
        {
            MovePreviewBuilding(pos); // move preview-Gameobject to the position the parameter represents
        }
        MoveGridIndicator(pos);  // move GridIndicator to the position the parameter represents
        SwitchColorForGridIndicator(availabilty); // switch Indicator's colr depends on Grid availability

    }

    /// <summary>
    ///  switch Indicator's colr depends on Grid availability
    /// </summary>
    /// <param name="availability"></param>
    public void SwitchColorForGridIndicator(bool availability)
    {
        Color c = availability ? Color.white : Color.red;
        c.a = 0.4f;
        gridIndicatorRenderer.material.color = c; 
    }

    /// <summary>
    /// set up preview¡ªGameObject's position
    /// </summary>
    /// <param name="pos"></param>
    public void MovePreviewBuilding(Vector3 pos)
    {
        previewObject.transform.position = new Vector3(pos.x, pos.y+offsetYforPreviewObject, pos.z);
    }

    /// <summary>
    ///  set up Indicator's position
    /// </summary>
    /// <param name="pos"></param>
    public void MoveGridIndicator(Vector3 pos)
    {
        gridIndicator.transform.position = new Vector3(pos.x + indicatorCenterOffset, pos.y, pos.z + indicatorCenterOffset);
    }

    /// <summary>
    /// display Demolisher Indicator. it is same with GridIndicator but always be  1*1 and only show available when target at a building rather than a empty grid
    /// </summary>
    internal void ShowDemolisher()
    {
        PrepareGridIndicator(Vector2Int.one);
        SwitchColorForGridIndicator(false);
        gridIndicator.SetActive(true);
    }
}
