using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "UnitDataBase", menuName = "ScriptableObjects/UnitDataBase", order = 1)]

public class UnitDataBase : ScriptableObject
{
    public List<GameObject> unitsType1 = new List<GameObject>();
    public List<GameObject> unitsType2 = new List<GameObject>();
    public List<GameObject> unitsType3 = new List<GameObject>();

    public List<Sprite> Icons = new List<Sprite>();
}
