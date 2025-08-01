using UnityEngine;

public class PlayerUnit : UnitController
{
    private Vector2 currentPos;

    private void Start()
    {
        // Round to grid coordinates
        currentPos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        transform.position = currentPos;
    }

    private void Update()
    {
        if (GameManager.instance.GameState != GameState.PlayerTurn) return;
    }

    protected override void AttemptAttack()
    {
        // Check for enemies nearby and damage them
        Debug.Log("Player attacking!");
    }
}