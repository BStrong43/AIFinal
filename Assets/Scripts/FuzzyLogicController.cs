using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SNAKE_STATE 
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
    BINARY_LOGIC
}

public class FuzzyLogicController : MonoBehaviour
{
    public AnimationCurve critical, hurt, healthy;
    public AnimationCurve creep, walk, run;
    public float health = 100.0f;
    public Transform healthKit, cover, player;

    Vector3 target;

    [SerializeField]
    DEFUZZ_MODE defuzzMode;

    public SNAKE_STATE state;
    Rigidbody rb;

    float runSpeed = 4.0f,
          walkSpeed = 2.2f,
          creepSpeed = 0.75f;

    float healthyValue = 0,
          hurtValue = 0,
          criticalValue = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

        health = Mathf.Clamp(health, 0.0f, 100.0f);
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
                target = player.position;
                break;

            case SNAKE_STATE.SEEKING_COVER:
                target = cover.position;
                break;

            case SNAKE_STATE.NEED_HEALING:
                target = healthKit.position;
                break;
        }
    }

    void performAction()
    {
        float diff = Vector3.Distance(transform.position, target);
        Debug.Log(diff);
        float runValue = run.Evaluate(diff);
        float walkValue = walk.Evaluate(diff);
        float creepValue = creep.Evaluate(diff);
        
        //Blended defuzzification
        float speed = (runSpeed * runValue) + (walkSpeed * walkValue) + (creepSpeed * creepValue);
        speed = Mathf.Clamp(speed, 0, runSpeed);
        speed *= Time.deltaTime;

        Vector3 dir = Vector3.Lerp(transform.position, target - transform.position, 1);
        //Debug.Log(dir);
        if (diff > .75f)
            rb.velocity = dir * speed;
        else
            rb.velocity = Vector3.zero;
    }

    public Vector3 getHealthValue()
    {
        return new Vector3(healthyValue, hurtValue, criticalValue);
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
        float maxBound = weightedValues.x + weightedValues.y + weightedValues.z;
        float choice = Random.Range(0.0f, maxBound);

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
        if(choice >= upperBound && choice <= maxBound)
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
