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
    [SerializeField] GameObject simpleAttackPrefab;
    [SerializeField] Transform castLocationTransform;

    [SerializeField] float simpleAttackDuration, simpleAttackSpawnTime;

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
        yield return new WaitForSeconds(simpleAttackSpawnTime);
        Instantiate(simpleAttackPrefab, castLocationTransform.position, castLocationTransform.rotation);
        yield return new WaitForSeconds(simpleAttackDuration - simpleAttackSpawnTime);
        inAction = false;
    }

    public bool IsInAction()
    {
        return inAction;
    }
}
