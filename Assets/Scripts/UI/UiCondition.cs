using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiCondition : MonoBehaviour
{
    public Condition Health;
    public Condition Stamina;
    public Condition Hunger;

    // Start is called before the first frame update
    void Start()
    {
        CharacterManager.Instance.Player.condition.uicondition = this;
    }

}
