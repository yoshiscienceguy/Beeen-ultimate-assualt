using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    good, normal, meh
}
public class Gun_properties : MonoBehaviour
{
    public float ShootingFrequency = 0.5f;
    public float bulletAmount = 1;
    public float maxDamage = 10;
    public float minimumDamage = 1;
    public bool raycasting;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
