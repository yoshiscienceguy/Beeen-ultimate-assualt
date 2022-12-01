using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : MonoBehaviour
{
    public float maxhealth = 21;
    private float currenthealth;
    // Start is called before the first frame update
    void Start()
    {
        currenthealth = maxhealth;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void takedamage(float damage)
    {
        currenthealth -= damage;
        if(currenthealth <= 0)
        {
            //oof
            Debug.Log("You suck loser, smell my feet, you're dogwater, I could beat you in my sleep, I bet a baby is better than you, you suck Like a cow playing ultimate frisbee mixed with baseball");
        }
    }
}
