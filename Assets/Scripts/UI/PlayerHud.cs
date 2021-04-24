using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] private SpellCastingController spellCastingController;
    [SerializeField] private DropCollector dropCollector;

    [SerializeField] private GameObject abilityCooldownPackage;
    [SerializeField] private Image spellIcon;
    [SerializeField] private TMPro.TMP_Text spellCooldownText;
    [SerializeField] private GameObject collectUIObject;
    [SerializeField] private Outline spellOutline;
    [SerializeField] private float scaleSpeed;
    [SerializeField] private float transitionTime;

    private Vector3 originalAbilityScale;
    private float remainingTransitionTime;
    private bool transitionCoroutine;

    private void Start()
    {
        Debug.Assert(spellCastingController != null, "SpellCastingController reference is null");
        Debug.Assert(dropCollector != null, "DropCollector reference is null");

        spellIcon.sprite = spellCastingController.SimpleAttackSpellDescription.SpellIcon;
        spellOutline.enabled = false;
        originalAbilityScale = abilityCooldownPackage.transform.localScale;
        transitionCoroutine = false;

        dropCollector.DropsInRangeChanged += OnDropsInRangeChanged;
    }

    private void OnDropsInRangeChanged()
    {
        collectUIObject.SetActive(dropCollector.DropsInRangeCount > 0);
    }

    private void Update()
    {
        float cooldown = spellCastingController.GetSimpleAttackCooldown();
        if (cooldown > 0)
        {
            spellCooldownText.text = cooldown.ToString("0.0");
            spellIcon.color = new Color(0.25f, 0.25f, 0.25f, 1);
        }
        else
        {
            spellCooldownText.text = "";
            spellIcon.color = Color.white;
        }

        if(spellCastingController.IsInAction())
        {
            spellOutline.enabled = true;
            scaleCastedAbility();
        }
        if(!spellCastingController.IsInAction() && cooldown > 0)
        {
            spellOutline.enabled = false;
            Resetscale();
        }
    }

    private void scaleCastedAbility()
    {
        Vector3 scale = abilityCooldownPackage.transform.localScale;

        scale += Vector3.one * Time.deltaTime * scaleSpeed;

        abilityCooldownPackage.transform.localScale = scale;
    }
    private void Resetscale()
    {
        if(!transitionCoroutine)
        {
            transitionCoroutine = true;
            StartCoroutine(TransitionTimeCoroutine());
        }
    }

    IEnumerator TransitionTimeCoroutine()
    {
        float currentDistance = 0;
        Vector3 startScale = abilityCooldownPackage.transform.localScale;
        while (currentDistance <= transitionTime)
        {
            float fractionOfDistance = currentDistance / transitionTime;
            abilityCooldownPackage.transform.localScale = Vector3.Lerp(startScale, originalAbilityScale,fractionOfDistance);

            currentDistance += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transitionCoroutine = false;
    }
}
