using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyLogicController : MonoBehaviour
{
    public AnimationCurve critical, hurt, heatlthy;
    public float health = 100.0f;

    void Start()
    {
        
    }

    void Update()
    {
        checkInput();

    }

    void checkInput()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            health -= 5f;
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            health += 5f;
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            health = 20f;
        }
    }

    void evaluateValues() 
    { 
        
    }
}
