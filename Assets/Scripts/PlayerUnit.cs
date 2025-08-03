using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
public class PlayerUnit : UnitController
{
    protected override void Start()
    {
        base.Start();

        if (levelInfo != null && levelInfo.availablePlayerActions != null)
        {
            actionLoop = new List<UnitAction>(levelInfo.availablePlayerActions);
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
                //Destroy(currentEnemy.gameObject);
                var enemyHealth = currentEnemy.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(50);
                }
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