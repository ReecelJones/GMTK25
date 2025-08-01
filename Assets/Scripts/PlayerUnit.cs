using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PlayerUnit : UnitController
{
    public List<UnitController> enemyList = new List<UnitController>();

    private void Start()
    {
        base.Start();
        // Round to grid coordinates
        //currentPos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        //transform.position = currentPos;
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