using UnityEngine;

public class FastEnemy : EnemyBase
{
    protected override void Start()
    {
        moveDuration = 0.65f;

        base.Start();
    }
}
