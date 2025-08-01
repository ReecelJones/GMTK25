using UnityEngine;

[System.Serializable]
public class UnitAction
{
    public UnitActionType ActionType;

    public UnitAction(UnitActionType type)
    {
        ActionType = type;
    }
}

public enum UnitActionType
{
    MoveUp,
    MoveDown,
    MoveLeft,
    MoveRight,
    Wait,
    Attack
}
