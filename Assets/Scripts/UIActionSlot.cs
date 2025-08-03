using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIActionSlot : MonoBehaviour, IDropHandler
{
    public int slotIndex;
    private UnitAction action;

    private UIManager UIManager;

    public void Setup(int index, System.Action<int> onClick)
    {
        this.slotIndex = index;
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

    }
    public void UnlockAction()
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            ActionItem draggableItem = dropped.GetComponent<ActionItem>();
            draggableItem.parentAfterDrag = transform;
        }
        else
        {
            Debug.Log("Should Swap");
            GameObject dropped = eventData.pointerDrag;
            ActionItem draggableItem = dropped.GetComponent<ActionItem>();

            GameObject current = transform.GetChild(0).gameObject;
            ActionItem currentDraggable = current.GetComponent<ActionItem>();

            currentDraggable.transform.SetParent(draggableItem.parentAfterDrag);
            draggableItem.parentAfterDrag = transform;
        }
    }
}
