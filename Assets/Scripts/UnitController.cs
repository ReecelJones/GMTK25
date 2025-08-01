using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public List<UnitAction> actionLoop = new List<UnitAction>();
    protected int currentActionIndex = 0;
    protected Vector2 currentPos;
    public float actionDelay = 0.5f; // time between actions
    private Coroutine loopRoutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        currentPos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        transform.position = currentPos;
    }

    public void SetPosition(Vector2 pos)
    {
        currentPos = pos;
        transform.position = pos;
    }

    public void ExecuteNextAction()
    {
        if (actionLoop.Count == 0) return;

        var action = actionLoop[currentActionIndex];
        HandleAction(action);

        currentActionIndex = (currentActionIndex + 1) % actionLoop.Count;
    }

    protected virtual void HandleAction(UnitAction action)
    {
        // This can be overridden or extended in child classes
        switch (action.ActionType)
        {
            case UnitActionType.MoveUp:
                TryMove(Vector2.up);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                break;
            case UnitActionType.MoveDown:
                TryMove(Vector2.down);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;
            case UnitActionType.MoveLeft:
                TryMove(Vector2.left);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
                break;
            case UnitActionType.MoveRight:
                TryMove(Vector2.right);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                break;
            case UnitActionType.Wait:
                break;
            case UnitActionType.Attack:
                AttemptAttack();
                break;
        }
    }

    protected virtual void TryMove(Vector2 direction)
    {
        Vector2 newPosition = currentPos + direction;
        Tile tile = GridManager.Instance.GetTileAtPosition(newPosition);

        if (tile == null || tile is RockTile) return;

        currentPos = newPosition;
        transform.position = newPosition;
    }

    protected virtual void AttemptAttack()
    {
        // To be implemented differently for Player/Enemy
    }

    public void PlayActionLoop()
    {
        if (loopRoutine != null) StopCoroutine(loopRoutine);
        loopRoutine = StartCoroutine(ExecuteActionLoop());
    }

    private IEnumerator ExecuteActionLoop()
    {
        for (int i = 0; i < actionLoop.Count; i++)
        {
            ExecuteNextAction();
            yield return new WaitForSeconds(actionDelay);
        }

        // End of loop, notify game manager
        GameManager.instance.EndPlayerTurn();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
