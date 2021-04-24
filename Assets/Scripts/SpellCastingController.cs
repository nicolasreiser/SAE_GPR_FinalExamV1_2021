using System.Collections;
using UnityEngine;

public interface IPlayerAction
{
    bool IsInAction();
}

public class SpellCastingController : MonoBehaviour, IPlayerAction
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform castLocationTransform;
    [SerializeField] private ProjectileSpellDescription simpleAttackSpell;
    [SerializeField] private ProjectileSpellDescription specialAttackSpell;
    [SerializeField] private DropCollector dropCollector;


    private bool inAction;
    private bool specialInAction;
    private float lastSimpleAttackTimestamp = -100;
    private float lastSpecialAttackTimestamp = -100;

    public SpellDescription SimpleAttackSpellDescription { get => simpleAttackSpell; }
    public SpellDescription SpecialAttackSpellDescription { get => specialAttackSpell; }

    private void Start()
    {
        Debug.Assert(simpleAttackSpell, "No spell assigned to SpellCastingController.");
        dropCollector.AbilityFound += SetSpecialAttack;
    }

    void Update()
    {
        bool simpleAttack = Input.GetButtonDown("Fire1");
        bool specialAttack = Input.GetButtonDown("Fire2");

        if (!inAction && !specialInAction)
        {
            if (simpleAttack && GetSimpleAttackCooldown() == 0)
            {
                StartCoroutine(SimpleAttackRoutine());
            }
            else if (specialAttackSpell != null)
            {
                if(specialAttack && GetSpecialAttackCooldown() == 0)
                {
                    if (specialAttackSpell != null)
                    {
                        StartCoroutine(SpecialAttackRoutine());
                        Debug.Log("Trigger special attack");
                    }
                }
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

        lastSimpleAttackTimestamp = Time.time;
        inAction = false;
    }
    private IEnumerator SpecialAttackRoutine()
    {
        specialInAction = true;
        animator.SetTrigger(specialAttackSpell.AnimationVariableName);

        yield return new WaitForSeconds(specialAttackSpell.ProjectileSpawnDelay);

        Instantiate(specialAttackSpell.ProjectilePrefab, castLocationTransform.position, castLocationTransform.rotation);

        yield return new WaitForSeconds(specialAttackSpell.Duration - specialAttackSpell.ProjectileSpawnDelay);

        lastSpecialAttackTimestamp = Time.time;
        specialInAction = false;
    }

    public bool IsInAction()
    {
        return inAction;
    }
    public bool SpecialIsInAction()
    {
        return specialInAction;
    }

    public float GetSimpleAttackCooldown()
    {
        return Mathf.Max(0, lastSimpleAttackTimestamp + simpleAttackSpell.Cooldown - Time.time);
    }
    public float GetSpecialAttackCooldown()
    {
        return Mathf.Max(0, lastSpecialAttackTimestamp + specialAttackSpell.Cooldown - Time.time);
    }

    public void SetSpecialAttack(ProjectileSpellDescription specialAttack)
    {
        Debug.Log("setting special attack to : " + specialAttack);
        specialAttackSpell = specialAttack;
    }
}
