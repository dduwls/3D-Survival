using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagalbe
{
    void TakePhysicalDamage(int damage);
}
public class PlayerCondition : MonoBehaviour , IDamagalbe
{   
    public UICondition uiCondition;

    Condition health {get {return uiCondition.health;}}
    Condition hunger {get {return uiCondition.health;}}
    Condition stamina {get {return uiCondition.health;}}

    public float noHungerHealthDecay;

    public event Action onTakeDamage;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(hunger.passiveValue * Time.deltaTime);

        if(hunger.curValue == 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
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

    public void Eet(float amount)
    {
        health.Add(amount);
    }

    public void Die()
    {
        Debug.Log("죽었다!");
    }
    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    } 
}
