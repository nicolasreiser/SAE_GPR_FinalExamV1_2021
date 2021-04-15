using UnityEngine;

public class AiTargetingManager : MonoBehaviour
{
    [SerializeField] private Transform aiTarget;


    public Transform GetDefaultAITarget()
    {
        return aiTarget;
    }
}
