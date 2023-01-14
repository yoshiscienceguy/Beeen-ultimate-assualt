using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public enum Rarity { 
    good,bad,horrible,meh
}

public class health : NetworkBehaviour
{
    public float maxhealth = 21;
    public NetworkVariable<float> netHealth = new NetworkVariable<float>();
    private float currenthealth;
    public Image healthbar;
    public Rarity gunRarity = Rarity.horrible;

    // Start is called before the first frame update


    public override void OnNetworkSpawn()
    {

        if (IsServer)
        {
            netHealth.Value = maxhealth;
        }
        currenthealth = maxhealth;
    
        
        if (IsOwner)
        {
            healthbar = GameObject.Find("Foreground_Healthbar").GetComponent<Image>();
        }
       
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float damage) {
        if (IsServer)
        {
            netHealth.Value -= damage;
            Debug.Log(netHealth.Value);
            currenthealth -= damage;

            updateHealthClientRpc();
        }
        else {
            takedamageServerRpc(damage);
        }
        



    }


    [ClientRpc]
    public void updateHealthClientRpc() {
        if (IsOwner)
        {
            healthbar = GameObject.Find("Foreground_Healthbar").GetComponent<Image>();
            healthbar.fillAmount = netHealth.Value / maxhealth;
            if (netHealth.Value <= 0)
            {
                //oof
                Debug.Log("You suck loser, smell my feet, you're dogwater, I could beat you in my sleep, I bet a baby is better than you, you suck Like a cow playing ultimate frisbee mixed with baseball");
            }
        }
    }
    [ServerRpc (RequireOwnership =false)]
    public void takedamageServerRpc(float damage)
    {
        TakeDamage(damage);
    }
}
