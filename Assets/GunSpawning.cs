using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GunSpawning : MonoBehaviour
{
    List<int> Places = new List<int>();
    public GameObject[] mehGuns;
    public GameObject[] anyGuns;
    public GameObject[] goodGuns;

    // Start is called before the first frame update
    void Start()
    {
        int randomPlaces = Random.Range(10, 14);

        for (int i = 0; i < randomPlaces; i++)
        {
            int RandomGunspot = 0;
            while (true)
            {
                RandomGunspot = Random.Range(0, 13);
                if (!Places.Contains(RandomGunspot))
                {
                    Places.Add(RandomGunspot);
                    break;
                }
            }
        
            Transform gunLocation = transform.GetChild(RandomGunspot);
            if(gunLocation.tag == "Meh gun")
            {
                int rgun = Random.Range(0, mehGuns.Length);
                GameObject clone = Instantiate(mehGuns[rgun], transform.position, Quaternion.identity);
                clone.GetComponent<NetworkObject>().Spawn();
            }
            if (gunLocation.tag == "Any gun")
            {
                int rgun = Random.Range(0, anyGuns.Length);
                GameObject clone = Instantiate(anyGuns[rgun], transform.position, Quaternion.identity);
                clone.GetComponent<NetworkObject>().Spawn();
            }
            if (gunLocation.tag == "Good gun")
            {
                int rgun = Random.Range(0, goodGuns.Length);
                GameObject clone = Instantiate(goodGuns[rgun], transform.position, Quaternion.identity);
                clone.GetComponent<NetworkObject>().Spawn();
            }
        }
        



    }

    // Update is called once per frame
    void Update()
    {
        
    }
 }