using UnityEngine;

public class FireUp : PowerUp
{
    protected override void ApplyEffect(
        PlayerController player)
    {
        player.IncreaseExplosionRange();
    }
}