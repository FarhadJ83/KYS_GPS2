using UnityEngine;

public class YinYangAnimation : MonoBehaviour
{
    public bool isMoving = false;
    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Update()
    {
        isMoving = false;
        animator.SetBool("IsMoving", false);
    }
}
