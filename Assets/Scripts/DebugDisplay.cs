using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugDisplay : MonoBehaviour
{
    public Text crit, hurt, healthy;
    public Image Panel;
    public FuzzyLogicController flc;
    Vector3 healthVector;
    bool showDebug;
    void Start()
    {
        showDebug = true;
        healthVector = Vector3.zero;
    }

 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            showDebug = !showDebug;

        if (showDebug)
        {
            Panel.enabled = true;
            crit.enabled = true;
            healthy.enabled = true;
            hurt.enabled = true;

            healthVector = flc.getHealthValue();//X is healthy, Y is Hurt, Z is Critical
            healthy.text = "Healthy Value: " + healthVector.x.ToString();
            hurt.text = "Hurt Value: " + healthVector.y.ToString();
            crit.text = "Healthy Value: " + healthVector.z.ToString();
        }
        else
        {
            Panel.enabled = false;
            crit.enabled = false;
            healthy.enabled = false;
            hurt.enabled = false;
        }
    }
}
