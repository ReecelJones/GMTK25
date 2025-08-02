using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
public class PlayerUnit : UnitController
{
    private LevelData levelInfo;
    [SerializeField] private AudioClip attackHit, attackMiss, move, moveBlock;

    protected override void Start()
    {
        base.Start();

        levelInfo = FindFirstObjectByType<LevelLoader>().levelData;
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
        attackDir = currentPos + lastDirection;

        Debug.Log("Player attacking!");

        if (attackEffect != null)
        {
            Instantiate(attackEffect, attackDir, Quaternion.identity);
        }

        for (int i = 0; i < GameManager.instance.enemyUnits.Count; i++)
        {
            var currentEnemy = GameManager.instance.enemyUnits[i];
            if (currentEnemy.currentPos == attackDir)
            {
                print("Yay");
                Destroy(currentEnemy.gameObject);
                GameManager.instance.OnEnemyKilled(currentEnemy);
                unitAudio.PlayOneShot(attackHit);
            }
            else
            {
                print("missed");
                unitAudio.PlayOneShot(attackMiss);
            }
        }
    }
}