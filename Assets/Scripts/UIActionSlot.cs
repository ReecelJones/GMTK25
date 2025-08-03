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

    private UIManager UIManager;

    public void Setup(UnitAction action, int index, System.Action<int> onClick)
    {
        this.action = action;
        this.slotIndex = index;
        this.onClickCallback = onClick;

        label.text = action.ActionType.ToString();
        button.onClick.AddListener(() => onClickCallback?.Invoke(slotIndex));
        UIManager = FindFirstObjectByType<UIManager>();
    }

    public void Update()
    {
        if (GameManager.instance.GameState == GameState.SetActions)
        {
            UnlockAction();
        }
        else
        {
            LockAction();
        }
    }

    public void LockAction()
    {
        UIManager.startTurnButton.interactable = false;
        button.interactable = false;
    }
    public void UnlockAction()
    {
        UIManager.startTurnButton.interactable = true;
        button.interactable = true;
    }
}
