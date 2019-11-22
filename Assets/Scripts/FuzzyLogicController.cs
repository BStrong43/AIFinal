using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum SNAKE_STATE 
{ 
    SEEKING_COVER = 0,
    NEED_HEALING,
    ATTACKING,
    NUM_STATES
}

public class FuzzyLogicController : MonoBehaviour
{
    public AnimationCurve critical, hurt, healthy;
    public float health = 100.0f;

    SNAKE_STATE state;

    float healthyValue = 0,
          hurtValue = 0,
          criticalValue = 0;

    void Start()
    {
        
    }

    void Update()
    {
        checkInput();
        evaluateValues();
        evaluateState();
        commitToAction();
    }

    void checkInput()
    {
        if(Input.GetKeyDown(KeyCode.I))
            health -= 5f;
        
        if(Input.GetKeyDown(KeyCode.O))
            health += 5f;
        
        if(Input.GetKeyDown(KeyCode.P))
            health = 20f;
        
        if(Input.GetKeyDown(KeyCode.U))
            health = 100f;
    }

    void evaluateValues() 
    {
        healthyValue = healthy.Evaluate(health);
        hurtValue = hurt.Evaluate(health);
        criticalValue = critical.Evaluate(health);
    }

    void evaluateState()
    {

    }

    void commitToAction()
    {

    }
}
