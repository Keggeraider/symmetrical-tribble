using UnityEngine;

public class LocomotionSMB : StateMachineBehaviour
{

    public float m_Damping = 0.15f;

    private readonly int m_HashHorizontalPara = Animator.StringToHash("horizontal");
    private readonly int m_HashVerticalPara = Animator.StringToHash("vertical");

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 input = new Vector2(horizontal, vertical).normalized;

        animator.SetFloat(m_HashHorizontalPara, input.x, m_Damping, Time.deltaTime);
        animator.SetFloat(m_HashVerticalPara, input.y, m_Damping, Time.deltaTime);
    }
}
