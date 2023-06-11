using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Animator anim;

    private readonly int runHash = Animator.StringToHash("run");
    private readonly int attackHash = Animator.StringToHash("attack");

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Attack()
    {
        anim.SetTrigger(attackHash);
    }

    public void SetRunState(bool run)
    {
        anim.SetBool(runHash, run);
    }
}
