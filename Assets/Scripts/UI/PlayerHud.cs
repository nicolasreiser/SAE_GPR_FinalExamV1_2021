using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] private SpellCastingController spellCastingController;
    [SerializeField] private DropCollector dropCollector;

    [SerializeField] private GameObject simpleAbilityCooldownPackage;
    [SerializeField] private GameObject specialAbilityCooldownPackage;

    [SerializeField] private Image simpleSpellIcon;
    [SerializeField] private Image specialSpellIcon;

    [SerializeField] private TMPro.TMP_Text simpleSpellCooldownText;
    [SerializeField] private TMPro.TMP_Text specialSpellCooldownText;

    [SerializeField] private GameObject collectUIObject;

    [Header("SimpleAttack")]
    [SerializeField] private Outline spellOutline;
    [SerializeField] private float scaleSpeed;
    [SerializeField] private float transitionTime;

    [Header("SpecialAttack")]
    [SerializeField] private Outline specialSpellOutline;
    [SerializeField] private float specialScaleSpeed;
    [SerializeField] private float specialTransitionTime;

    


    private Vector3 originalSimpleAbilityScale;
    private Vector3 originalSpecialAbilityScale;
    private float remainingTransitionTime;
    private bool transitionCoroutineSimpleSpell;
    private bool transitionCoroutineSpecialSpell;

    private void Start()
    {
        Debug.Assert(spellCastingController != null, "SpellCastingController reference is null");
        Debug.Assert(dropCollector != null, "DropCollector reference is null");

        simpleSpellIcon.sprite = spellCastingController.SimpleAttackSpellDescription.SpellIcon;
        spellOutline.enabled = false;
        originalSimpleAbilityScale = simpleAbilityCooldownPackage.transform.localScale;
        transitionCoroutineSimpleSpell = false;

        specialAbilityCooldownPackage.SetActive(false);

        dropCollector.DropsInRangeChanged += OnDropsInRangeChanged;
        dropCollector.AbilityFound += AddSpecialAbility;

    }

    private void OnDropsInRangeChanged()
    {
        collectUIObject.SetActive(dropCollector.DropsInRangeCount > 0);
    }

    private void Update()
    {
        float simpleSpellCooldown = spellCastingController.GetSimpleAttackCooldown();
        float specialSpellCooldown = 0;
        
        if(specialAbilityCooldownPackage.activeSelf)
        {
            specialSpellCooldown = spellCastingController.GetSpecialAttackCooldown();
        }
        
        // for simple spell
        if (simpleSpellCooldown > 0)
        {
            simpleSpellCooldownText.text = simpleSpellCooldown.ToString("0.0");
            simpleSpellIcon.color = new Color(0.25f, 0.25f, 0.25f, 1);
        }
        else
        {
            simpleSpellCooldownText.text = "";
            simpleSpellIcon.color = Color.white;
        }
        // for special spell
        if (specialSpellCooldown > 0)
        {
            specialSpellCooldownText.text = specialSpellCooldown.ToString("0.0");
            specialSpellIcon.color = new Color(0.25f, 0.25f, 0.25f, 1);
        }
        else
        {
            specialSpellCooldownText.text = "";
            specialSpellIcon.color = Color.white;
        }

        // for simple spell
        if (spellCastingController.IsInAction())
        {
            spellOutline.enabled = true;
            scaleCastedAbility(simpleAbilityCooldownPackage.transform);
        }
        if(!spellCastingController.IsInAction() && simpleSpellCooldown > 0)
        {
            spellOutline.enabled = false;
            ResetscaleSimpleSpell();
        }
        // for special spell
        if (spellCastingController.SpecialIsInAction())
        {
            specialSpellOutline.enabled = true;
            scaleCastedAbility(specialAbilityCooldownPackage.transform);
        }
        if (!spellCastingController.SpecialIsInAction() && specialSpellCooldown > 0)
        {
            specialSpellOutline.enabled = false;
            ResetscaleSpecialSpell();
        }
    }

    private void scaleCastedAbility(Transform transform)
    {
        Vector3 scale = transform.localScale;

        scale += Vector3.one * Time.deltaTime * scaleSpeed;

        transform.localScale = scale;
    }
    private void ResetscaleSimpleSpell()
    {
        if(!transitionCoroutineSimpleSpell)
        {
            transitionCoroutineSimpleSpell = true;
            StartCoroutine(TransitionTimeCoroutineForSimpleSpell());
        }
    }
    private void ResetscaleSpecialSpell()
    {
        if (!transitionCoroutineSpecialSpell)
        {
            transitionCoroutineSpecialSpell = true;
            StartCoroutine(TransitionTimeCoroutineForSpecialSpell());
        }
    }

    IEnumerator TransitionTimeCoroutineForSimpleSpell()
    {
        float currentDistance = 0;
        Vector3 startScale = simpleAbilityCooldownPackage.transform.localScale;
        while (currentDistance <= transitionTime)
        {
            float fractionOfDistance = currentDistance / transitionTime;
            simpleAbilityCooldownPackage.transform.localScale = Vector3.Lerp(startScale, originalSimpleAbilityScale,fractionOfDistance);

            currentDistance += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transitionCoroutineSimpleSpell = false;
    }
    IEnumerator TransitionTimeCoroutineForSpecialSpell()
    {
        float currentDistance = 0;
        Vector3 startScale = specialAbilityCooldownPackage.transform.localScale;
        while (currentDistance <= specialTransitionTime)
        {
            float fractionOfDistance = currentDistance / specialTransitionTime;
            specialAbilityCooldownPackage.transform.localScale = Vector3.Lerp(startScale, originalSpecialAbilityScale, fractionOfDistance);

            currentDistance += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transitionCoroutineSpecialSpell = false;
    }

    public void AddSpecialAbility(ProjectileSpellDescription projectileSpellDescription)
    {
        Debug.Log("Added Ability, name : "+ projectileSpellDescription.SpellName);
        specialAbilityCooldownPackage.SetActive(true);
        specialSpellIcon.sprite = projectileSpellDescription.SpellIcon;




    }
}
