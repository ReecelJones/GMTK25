using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Create Level")]
public class LevelData : ScriptableObject
{
    public UnitAction[] availablePlayerActions;
    public UnitAction[] availableEnemyActions;
    public TextAsset levelTextFile;


}
