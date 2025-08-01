using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Create Level")]
public class LevelData : ScriptableObject
{
    public UnitAction[] availableActions;
    public TextAsset levelTextFile;
}
