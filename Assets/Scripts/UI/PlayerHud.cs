using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] SpellCastingController spellCastingController;
    [SerializeField] DropCollector dropCollector;

    [SerializeField] Image spellIcon;
    [SerializeField] TMPro.TMP_Text spellCooldownText;
    [SerializeField] GameObject collectUIObject;

    private void Start()
    {
        Debug.Assert(spellCastingController != null, "SpellCastingController reference is null");
        Debug.Assert(dropCollector != null, "DropCollector reference is null");

        spellIcon.sprite = spellCastingController.SimpleAttackSpellDescription.SpellIcon;

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
    }
}
