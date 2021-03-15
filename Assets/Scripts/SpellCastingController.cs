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
    [SerializeField] Transform castLocationTransform;
    [SerializeField] ProjectileSpellDescription simpleAttackSpell;


    private bool inAction;


    void Update()
    {
        bool simpleAttack = Input.GetButtonDown("Fire1");
        bool specialAttack = Input.GetButtonDown("Fire2");

        if (!inAction)
        {
            if (simpleAttack)
            {
                StartCoroutine(SimpleAttackRoutine());
            }
            else if (specialAttack)
            {
                Debug.Log("Trigger special attack");
            }
        }
    }

    private IEnumerator SimpleAttackRoutine()
    {
        inAction = true;
        animator.SetTrigger(simpleAttackSpell.AnimationVariableName);
        yield return new WaitForSeconds(simpleAttackSpell.ProjectileSpawnDelay);
        Instantiate(simpleAttackSpell.ProjectilePrefab, castLocationTransform.position, castLocationTransform.rotation);
        yield return new WaitForSeconds(simpleAttackSpell.Duration - simpleAttackSpell.ProjectileSpawnDelay);
        inAction = false;
    }

    public bool IsInAction()
    {
        return inAction;
    }
}
