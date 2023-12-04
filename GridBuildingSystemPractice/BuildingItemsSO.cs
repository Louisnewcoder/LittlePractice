
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName ="BuildingItemConfig",
    menuName ="GridBuildingSO/BuildingItemSO",
    order =0)]
public class BuildingItemsSO : ScriptableObject
{
    public List<BuildingItem> buildingItems ;
}

[Serializable]
public class BuildingItem
{
    [field:SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public int ID { get; private set; }
    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;
    [field: SerializeField]
    public GameObject prefab { get; private set; }
}