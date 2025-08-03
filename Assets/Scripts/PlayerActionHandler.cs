using System.Collections.Generic;
using UnityEngine;

public class PlayerActionHandler : MonoBehaviour
{
    public PlayerUnit playerUnit;
    [SerializeField] private Transform actionSlotParent;
    [SerializeField] private GameObject actionSlotPrefab;
    [SerializeField] private GameObject actionItemPrefab;

    private int? selectedIndex = null;

    public void RefreshUI()
    {
        Debug.Log("Action Loop Count: " + playerUnit.actionLoop.Count);
        foreach (Transform child in actionSlotParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < playerUnit.actionLoop.Count; i++)
        {
            var slot = Instantiate(actionSlotPrefab, actionSlotParent).GetComponent<UIActionSlot>();
            Debug.Log("Instantiated Slot: " + slot.name);

            var actionItemGO = Instantiate(actionItemPrefab, slot.transform);
            Debug.Log("Instantiated Action Item GO: " + actionItemGO.name);

            var actionItem = actionItemGO.GetComponent<ActionItem>();
            actionItem.Setup(playerUnit.actionLoop[i]);
        }
    }

    public void SaveActionsFromUI()
    {
        List<UnitAction> newLoop = new List<UnitAction>();

        foreach (Transform slot in actionSlotParent)
        {
            ActionItem actionItem = slot.GetComponentInChildren<ActionItem>();
            if (actionItem != null && actionItem.action != null)
            {
                newLoop.Add(actionItem.action);
            }
        }

        playerUnit.actionLoop = newLoop;
        Debug.Log("Saved action loop: " + newLoop.Count);
    }

    public void SyncActionLoop()
    {
        var newLoop = new List<UnitAction>();

        foreach (Transform slot in actionSlotParent)
        {
            var actionItem = slot.GetComponentInChildren<ActionItem>();
            if (actionItem != null)
            {
                newLoop.Add(actionItem.action);
            }
        }

        playerUnit.actionLoop = newLoop;
    }

}
