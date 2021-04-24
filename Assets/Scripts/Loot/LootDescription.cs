using UnityEngine;

[CreateAssetMenu]
public class LootDescription : ScriptableObject
{
    [SerializeField] private DropProbabilityPair[] drops;

    public void SetDrops(params DropProbabilityPair[] drops)
    {
        this.drops = drops;
    }

    public Drop SelectDropRandomly()
    {
        Drop drop = null;

        while (drop == null) 
        { 
            for (int i = 0; i < drops.Length; i++)
            {
                float rnd = Random.value;
                DropProbabilityPair pair = drops[i];

                if (rnd < pair.Probability)
                {
                    drop = pair.Drop;
                    break;
                }
            }
        }
        return drop;
    }
}

[System.Serializable]
public struct DropProbabilityPair
{
    public Drop Drop;

    [Range(0, 1)]
    public float Probability;
}
