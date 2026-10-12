using UnityEngine;

public class PlayerVisualAnimator : MonoBehaviour
{
    private Animator animator;

    private Vector3 previousPosition;
    private string direction = "Down";
    private string currentAnimation = "";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        previousPosition = transform.position;
    }

    private void Start()
    {
        PlayAnimation("IdleDown");
    }

    private void LateUpdate()
    {
        Vector3 movement =
            transform.position - previousPosition;

        previousPosition = transform.position;

        // Evita interpretar el respawn como una caminata.
        if (movement.sqrMagnitude > 1.5f)
        {
            PlayAnimation("Idle" + direction);
            return;
        }

        bool isWalking =
            movement.sqrMagnitude > 0.000001f;

        if (isWalking)
        {
            if (Mathf.Abs(movement.x) >
                Mathf.Abs(movement.y))
            {
                direction =
                    movement.x > 0 ? "Right" : "Left";
            }
            else
            {
                direction =
                    movement.y > 0 ? "Up" : "Down";
            }
        }

        string animationName =
            (isWalking ? "Walk" : "Idle") + direction;

        PlayAnimation(animationName);
    }

    private void PlayAnimation(string animationName)
    {
        if (animator == null ||
            currentAnimation == animationName)
        {
            return;
        }

        animator.Play(animationName);
        currentAnimation = animationName;
    }
}