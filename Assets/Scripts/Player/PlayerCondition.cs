using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IDamageAble
{
    void TakePhysicalDamage(int damage);
}


public class PlayerCondition : MonoBehaviour, IDamageAble
{
    public UiCondition uicondition;

    Condition health { get { return uicondition.Health; } }
    Condition hunger { get { return uicondition.Hunger; } }
    Condition stamina { get { return uicondition.Stamina; } }
    // Start is called before the first frame update

    public float noHungerHealthDeacy;

    public event Action onTakeDamage;

    public float boostAmount = 1.5f;
    private Coroutine boostCoroutine;

    public PlayerController controller;
    

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        hunger.Substract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);
        health.Add(health.passiveValue * Time.deltaTime);

        if (hunger.curValue == 0f)
        {
            health.Substract(noHungerHealthDeacy * Time.deltaTime);
        }

        if(health.curValue == 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void Die()
    {
        Debug.Log("die");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Substract(damage);
        onTakeDamage?.Invoke();
    }


    public void OnBoost(float duration)
    {
        if (boostCoroutine != null)
        {
            StopCoroutine(boostCoroutine);
            controller.MoveSpeed -= boostAmount; 
        }
        boostCoroutine = StartCoroutine(Boost(duration));
    }

    private IEnumerator Boost(float duration)
    {
        controller.MoveSpeed += boostAmount;

        yield return new WaitForSeconds(duration);

        controller.MoveSpeed -= boostAmount;
        boostCoroutine = null;
    }
}
