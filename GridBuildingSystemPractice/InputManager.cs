using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class BuildingSystemInputManager : MonoBehaviour
{

    // public static LayerMask placementLayerMask;
    private static Vector3 placeLocationPointed;

    public event UnityAction OnClick, OnExit;

    public bool CheckClickOnUI() => EventSystem.current.IsPointerOverGameObject();

    public static Vector3 GetPlaceByMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out RaycastHit hitInfo, 500, 1 << LayerMask.NameToLayer("Placeable")))
        {
            placeLocationPointed = hitInfo.point;
        }
        return placeLocationPointed;
    }

    /* private void Awake()
    {
        placementLayerMask = 1 << LayerMask.NameToLayer("Placeable");
    }*/

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClick?.Invoke();
        }
        if (Input.GetMouseButtonDown(1)||Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }
    }

}
