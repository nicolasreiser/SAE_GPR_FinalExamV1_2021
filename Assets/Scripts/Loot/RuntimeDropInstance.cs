using UnityEngine;

public class RuntimeDropInstance : MonoBehaviour, IDropOwner
{
    [SerializeField] TMPro.TMP_Text dropUIText;

    private Drop drop;


    public Drop GetDrop()
    {
        return drop;
    }

    public void SetDrop(Drop drop)
    {
        this.drop = drop;
        dropUIText.text = drop.DropName;
        dropUIText.color = drop.Rarity.ToColor();
    }
}
