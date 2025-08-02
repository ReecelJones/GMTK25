using UnityEngine;

public class EnemyUnit : UnitController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        playerUnit = FindFirstObjectByType<PlayerUnit>();
        GameManager.instance.enemyUnits.Add(this);
        GameManager.instance.objectiveKills++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
