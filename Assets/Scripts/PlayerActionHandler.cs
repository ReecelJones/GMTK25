using System.Collections.Generic;
using UnityEngine;

public class PlayerActionHandler : MonoBehaviour
{
    public PlayerUnit playerUnit;
    [SerializeField] private Transform actionSlotParent;
    [SerializeField] private GameObject actionSlotPrefab;

    private int? selectedIndex = null;

    public void RefreshUI()
    {
        foreach (Transform child in actionSlotParent)
            Destroy(child.gameObject);

        for (int i = 0; i < playerUnit.actionLoop.Count; i++)
        {
            var slot = Instantiate(actionSlotPrefab, actionSlotParent).GetComponent<UIActionSlot>();
            slot.Setup(playerUnit.actionLoop[i], i, OnSlotClicked);
        }
    }

    private void OnSlotClicked(int index)
    {
        if (selectedIndex == null)
        {
            selectedIndex = index;
        }
        else
        {
            // Swap and refresh
            var temp = playerUnit.actionLoop[index];
            playerUnit.actionLoop[index] = playerUnit.actionLoop[selectedIndex.Value];
            playerUnit.actionLoop[selectedIndex.Value] = temp;

            selectedIndex = null;
            RefreshUI();
        }
    }
}
