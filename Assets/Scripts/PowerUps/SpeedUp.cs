using UnityEngine;

public class SpeedUp : PowerUp
{
    protected override void ApplyEffect(
        PlayerController player)
    {
        player.IncreaseSpeed();
    }
}