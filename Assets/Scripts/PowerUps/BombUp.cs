using UnityEngine;

public class BombUp : PowerUp
{
    protected override void ApplyEffect(
        PlayerController player)
    {
        player.IncreaseMaxBombs();
    }
}
