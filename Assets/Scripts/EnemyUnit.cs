using UnityEngine;

public class EnemyUnit : UnitController
{
    public new PlayerUnit playerUnit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
        playerUnit = FindFirstObjectByType<PlayerUnit>();
        playerUnit.enemyList.Add(this);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
