using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
public class health : NetworkBehaviour
{
    public float maxhealth = 21;
    public NetworkVariable<float> netHealth;
    private float currenthealth;
    public Image healthbar;
    // Start is called before the first frame update
    public void Awake()
    {
        currenthealth = maxhealth;
        netHealth = new NetworkVariable<float>();

            netHealth.Value = currenthealth;

        if (!IsOwner) {
            return;
        }
        
        healthbar = GameObject.Find("Foreground_Healthbar").GetComponent<Image>();
        
       
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }


    public void takedamage(float damage)
    {
        netHealth.Value -= damage;
        currenthealth -= damage;
        if (IsOwner)
        {
            healthbar = GameObject.Find("Foreground_Healthbar").GetComponent<Image>();
            healthbar.fillAmount = netHealth.Value / maxhealth;
        }
        if(currenthealth <= 0)
        {
            //oof
            Debug.Log("You suck loser, smell my feet, you're dogwater, I could beat you in my sleep, I bet a baby is better than you, you suck Like a cow playing ultimate frisbee mixed with baseball");
        }
    }
}
