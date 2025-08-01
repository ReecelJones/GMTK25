using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIActionSlot : MonoBehaviour
{
    public int slotIndex;
    public Button button;
    public TMP_Text label;

    private UnitAction action;
    private System.Action<int> onClickCallback;

    public void Setup(UnitAction action, int index, System.Action<int> onClick)
    {
        this.action = action;
        this.slotIndex = index;
        this.onClickCallback = onClick;

        label.text = action.ActionType.ToString();
        button.onClick.AddListener(() => onClickCallback?.Invoke(slotIndex));
    }
}
