using UnityEngine;

public class AttackStateHandler : StateMachineBehaviour
{
    private CircleCollider2D attackCollider;
    private const string colliderName = "AttackCheckPos";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackCollider = animator.transform.Find(colliderName).GetComponent<CircleCollider2D>();

        if (attackCollider != null)
        {
            attackCollider.enabled = true;
        } else
        {
            Debug.LogError("No Collider!");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }
}
