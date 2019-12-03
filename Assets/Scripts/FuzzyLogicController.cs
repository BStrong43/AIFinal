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

enum DEFUZZ_MODE
{
    HIGHEST_PRIORITY,
    WEIGHTED_RANDOM,
    CENTER_OF_GRAV,
    BINARY_LOGIC
}

public class FuzzyLogicController : MonoBehaviour
{
    public AnimationCurve critical, hurt, healthy;
    public float health = 100.0f;
    public Transform healthKit, cover, player;
    Transform target;
    [SerializeField]
    DEFUZZ_MODE defuzzMode;

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
        if (defuzzMode != DEFUZZ_MODE.BINARY_LOGIC)
        {
            getFuzzyValues();
            defuzzValues();
        }
        commitToAction();
        performAction();
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

    void getFuzzyValues() 
    {
        healthyValue = healthy.Evaluate(health);
        hurtValue = hurt.Evaluate(health);
        criticalValue = critical.Evaluate(health);
    }

    void defuzzValues()
    {
        switch (defuzzMode)
        {
            case DEFUZZ_MODE.HIGHEST_PRIORITY:
                priorityDecision();
                break;

            case DEFUZZ_MODE.WEIGHTED_RANDOM:
                weightedRandomDecision();
                break;

            case DEFUZZ_MODE.CENTER_OF_GRAV:
                centerOfGravityDecision();
                break;

            case DEFUZZ_MODE.BINARY_LOGIC:
                binaryDecision();
                break;
        }
    }

    void commitToAction()
    {
        switch (state)
        {
            case SNAKE_STATE.ATTACKING:
                target = player;
                break;

            case SNAKE_STATE.SEEKING_COVER:
                target = cover;
                break;

            case SNAKE_STATE.NEED_HEALING:
                target = healthKit;
                break;
        }
    }

    void performAction()
    {
        //Blended fuzzy logic for speed
        //Look at books example
    }

    public Vector3 getHealthValue()
    {
        return new Vector3(healthyValue, hurtValue, criticalValue);
    }

    void centerOfGravityDecision()
    {

    }

    void priorityDecision()
    {
        if(healthyValue > hurtValue && healthyValue > criticalValue)
        {
            state = SNAKE_STATE.ATTACKING;
        }

        if(hurtValue > healthyValue && hurtValue > criticalValue)
        {
            state = SNAKE_STATE.SEEKING_COVER;
        }

        if(criticalValue > healthyValue && criticalValue > hurtValue)
        {
            state = SNAKE_STATE.NEED_HEALING;
        }
    }

    void weightedRandomDecision()
    {
        Vector3 weightedValues = getHealthValue().normalized;
        float choice = Random.Range(0.0f, 1.0f);

        float lowerBound = weightedValues.x,
              upperBound = weightedValues.x + weightedValues.y;

        if(choice >= 0.0f && choice <= lowerBound)
        {
            //Chose Healthy Option
            state = SNAKE_STATE.ATTACKING;
        }
        if(choice > lowerBound && choice < upperBound)
        {
            //Chose Hurt option
            state = SNAKE_STATE.SEEKING_COVER;
        }
        if(choice >= upperBound && choice <= 1.0f)
        {
            //Chose critical option
            state = SNAKE_STATE.NEED_HEALING;
        }
    }

    void binaryDecision()
    {
        if (health > 66)
            state = SNAKE_STATE.ATTACKING;
        else if (health > 33)
            state = SNAKE_STATE.SEEKING_COVER;
        else
            state = SNAKE_STATE.NEED_HEALING;
    }
}
