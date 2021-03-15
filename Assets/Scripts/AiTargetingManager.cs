using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTargetingManager : MonoBehaviour
{
    [SerializeField] Transform aiTarget;


    public Transform GetDefaultAITarget()
    {
        return aiTarget;
    }
}
