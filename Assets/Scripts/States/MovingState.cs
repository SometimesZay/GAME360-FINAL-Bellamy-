using Unity.VisualScripting;
using UnityEngine;

public class MovingState : PlayerState
{
    public override void EnterState(PlayerController player)
    {
        TryPlayAnimation(player, "Run");
        EventManager.TriggerEvent("OnPlayerStateChanged", "Moving");
    }

    public override void UpdateState(PlayerController player)
    {
        float horizontal = Input.GetAxis("Horizontal");

        if (Mathf.Abs(horizontal) < 0.1f)
        {
            player.ChangeState(new IdleState());
        }
    }

    public override void ExitState(PlayerController player) { }

    public override string GetStateName() => "Moving";

    private void TryPlayAnimation(PlayerController player, string animName)
    {
        if (player.animator != null &&
            player.animator.runtimeAnimatorController != null &&
            player.animator.isActiveAndEnabled)
        {
            try
            {
                player.animator.Play(animName);
            }
            catch
            {
                // Animation doesn't exist - continue without it
            }
        }
    }
}
