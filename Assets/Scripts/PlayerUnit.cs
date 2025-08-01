using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
public class PlayerUnit : UnitController
{
    public List<UnitController> enemyList = new List<UnitController>();
    private Vector2 currentPos;
    private LevelData levelInfo;

    protected override void Start()
    {
        base.Start();
        // Round to grid coordinates
        //currentPos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        //transform.position = currentPos;
        levelInfo = FindFirstObjectByType<LevelLoader>().levelData;

        // Round to grid coordinates
        currentPos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        transform.position = currentPos;

        if (levelInfo != null && levelInfo.availableActions != null)
        {
            actionLoop = new List<UnitAction>(levelInfo.availableActions);
        }
        else
        {
            Debug.LogWarning("LevelData or availableActions is missing!");
        }

    }

    private void Update()
    {
        if (GameManager.instance.GameState != GameState.PlayerTurn) return;
    }

    protected override void AttemptAttack()
    {
        base.AttemptAttack();
        // Check for enemies nearby and damage them
        Debug.Log("Player attacking!");
        for (int i = 0; i < enemyList.Count; i++)
        {
            var currentEnemy = enemyList[i];
            if (currentEnemy.currentPos == attackDir)
            {
                print("Yay");
                currentEnemy.GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                print("missed");
            }
        }

    }
}