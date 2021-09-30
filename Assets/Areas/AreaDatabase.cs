using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AreaDB", menuName = "ScriptableObjects/AreaDB")]
public class AreaDatabase : ScriptableObject
{
    [Header("Areas")]
    public List<Area> areas = new List<Area>();
}
