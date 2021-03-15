using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] SpellCastingController spellCastingController;
    [SerializeField] Image spellIcon;
    [SerializeField] TMPro.TMP_Text spellCooldownText;

    private void Start()
    {
        Debug.Assert(spellCastingController != null, "SpellCastingController reference is null");

        spellIcon.sprite = spellCastingController.SimpleAttackSpellDescription.SpellIcon;
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
