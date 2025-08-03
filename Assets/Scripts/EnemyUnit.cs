using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : UnitController
{

    private bool hasCompletedTurn = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        playerUnit = FindFirstObjectByType<PlayerUnit>();
        GameManager.instance.enemyUnits.Add(this);
        GameManager.instance.objectiveKills++;

        if (levelInfo != null && levelInfo.availableEnemyActions != null)
        {
            actionLoop = new List<UnitAction>(levelInfo.availableEnemyActions);
        }
        else
        {
            Debug.LogWarning("LevelData or availableActions is missing!");
        }
    }

    public override void InitiateEndTurn()
    {
        GameManager.instance.enemyCompletedTurn++;
        if (GameManager.instance.enemyCompletedTurn == GameManager.instance.enemyUnits.Count)
        {
            base.InitiateEndTurn();
            GameManager.instance.enemyCompletedTurn = 0;
        }
        else return;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GameState != GameState.EnemyTurn) return;
    }
}
