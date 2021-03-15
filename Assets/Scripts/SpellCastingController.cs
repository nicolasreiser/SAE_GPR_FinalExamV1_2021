using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAction
{
    bool IsInAction();
}

public class SpellCastingController : MonoBehaviour, IPlayerAction
{
    [SerializeField] Animator animator;

    private bool inAction;


    void Update()
    {
        bool simpleAttack = Input.GetButtonDown("Fire1");

        if (!inAction && simpleAttack)
        {
            StartCoroutine(SimpleAttackRoutine());
        }
    }

    private IEnumerator SimpleAttackRoutine()
    {
        inAction = true;
        animator.SetTrigger("SimpleAttack");
        yield return new WaitForSeconds(1);
        inAction = false;
    }

    public bool IsInAction()
    {
        return inAction;
    }
}
