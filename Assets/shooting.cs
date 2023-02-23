using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class shooting : NetworkBehaviour
{
    public float lrfreq = .1f;
    public Transform barrel;
    public LayerMask lm;
    public float knokback = 50f;
    public float lrrange;
    public GameObject lrmf;
    public GameObject lr;
    private float ctime;
    public Light lrlight;
    public float lrcurrentmag;
    public float lrmaxmag = 30;
    public TextMeshProUGUI ammo;
    public GameObject hitmarker;
    public bool reloading;
    public Animator anim;
    public bool canshoot;
    public bool splodeyboi;
    public float explosiveforce = 0;
    public float exlposionradios = 25;
    public float weaponDamage = 3;
    public GameObject bulletDecal;
    public GameObject Scope;
    public GameObject currentGun;
    public GameObject[] GunSkins;
    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsServer)
        {
            transform.parent.name = "Player " + transform.parent.GetComponent<NetworkObject>().OwnerClientId.ToString();
        }
        if (!GetComponentInParent<NetworkObject>().IsOwner)
        {
            enabled = false;
            return;
        }
        lrcurrentmag = lrmaxmag;
        ammo = GameObject.Find("Ammo Count").GetComponent<TextMeshProUGUI>();
        Debug.Log(name);
        ammo.text = lrcurrentmag.ToString() + "/" + lrmaxmag.ToString();

        reloading = false;
        canshoot = true;
        lrmaxmag = 30;
    }

    public void reloaded()
    {
        reloading = false;
    }

    [ServerRpc]
    public void PlayerShootGunServerRpc(Vector3 barrelo, Vector3 barrelTD, float knockback, float dmg, ulong whoShot, ServerRpcParams serverRpcParams = default)
    {
        RaycastHit hit;
        if (Physics.Raycast(barrelo, barrelTD, out hit, lrrange, lm))
        {
            if (splodeyboi == true)
            {
                Collider[] victems = Physics.OverlapSphere(hit.point, exlposionradios);
                foreach (Collider body in victems)
                {

                    Rigidbody rb = body.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.AddExplosionForce(explosiveforce, hit.point, exlposionradios);
                    }
                }
            }
            else
            {
                GameObject victem = hit.collider.gameObject;

                if (victem.GetComponent<Rigidbody>())
                {
                    //StartCoroutine("hit");
                    victem.GetComponent<Rigidbody>().AddForce(barrelTD * knockback, ForceMode.Impulse);

                    health healthScript = hit.collider.gameObject.GetComponent<health>();
                    if (healthScript != null)
                    {
                        //healthScript.netHealth.Value -= 3;
                        Debug.Log("hit : " + hit.collider.gameObject.name);
                        healthScript.takedamageServerRpc(dmg, whoShot);
                    }
                }

            }

            GameObject decalClone = Instantiate(bulletDecal, hit.point, Quaternion.identity);
            decalClone.GetComponent<NetworkObject>().Spawn();



        }
    }

    float zoom = 15;
    float zoomSpeed = 200;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Scope.SetActive(true);
            Camera.main.fieldOfView = zoom;
            zoom -= Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
            zoom = Mathf.Clamp(zoom, 1, 30);
        }
        else
        {
            Scope.SetActive(false);
            Camera.main.fieldOfView = 60;
        }
        if (!GetComponentInParent<NetworkObject>().IsOwner) { return; }
        if (currentGun != null)
        {


            if (ctime >= lrfreq)
            {
                if (canshoot == true)
                {


                    if (Input.GetMouseButton(0) && !reloading)
                    {
                        if (lrcurrentmag <= 0)
                        {
                            reloading = true;
                            anim.SetTrigger("reload");
                            StartCoroutine("wait");
                            lrcurrentmag = lrmaxmag;
                            reloading = false;
                        }



                        //PlayerShootGunServerRpc(barrel.position, barrel.TransformDirection(Vector3.forward));
                        PlayerShootGunServerRpc(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), knokback, weaponDamage, transform.parent.GetComponent<NetworkObject>().OwnerClientId);

                        //lorenzo fix this
                        //StartCoroutine("hit");

                        lrcurrentmag--;
                        StartCoroutine("lrMf");


                        ammo.text = lrcurrentmag.ToString() + "/" + lrmaxmag.ToString();

                        lrlight.intensity = (lrcurrentmag / lrmaxmag) * 10.0f;
                        ctime = 0;
                    }

                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        reloading = true;
                        anim.SetTrigger("reload");
                        reloading = false;
                        lrcurrentmag = lrmaxmag;
                    }
                }

            }
            else
            {
                ctime += Time.deltaTime;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentGun != null)
            {
               
                currentGun.GetComponent<gunID>().gunAppearServerRpc(transform.position + new Vector3(0, 1, 0), Camera.main.transform.TransformDirection(new Vector3(0, 0, 10)), transform.parent.GetComponent<Rigidbody>().velocity);
                changeParentServerRpc(currentGun, currentGun, true);
                currentGun = null;
                foreach (GameObject gun in GunSkins)
                {
                    gun.gameObject.SetActive(false);
                }
            }
        }
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, 15, lm))
        {
            if (hit.collider.gameObject.CompareTag("Any gun") ||
                hit.collider.gameObject.CompareTag("Good gun") ||
                hit.collider.gameObject.CompareTag("Meh gun"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (currentGun != null)
                    {
                        //currentGun.transform.SetParent(null);
                        changeParentServerRpc(currentGun, currentGun,true);
                        currentGun.GetComponent<gunID>().gunAppearServerRpc(transform.position + new Vector3(0, 1, 0), Camera.main.transform.TransformDirection(new Vector3(0, 0, 10)), transform.parent.GetComponent<Rigidbody>().velocity);

                        currentGun = null;
                        foreach (GameObject gun in GunSkins)
                        {
                            gun.gameObject.SetActive(false);
                        }
                    }
                    currentGun = hit.collider.gameObject;
                    currentGun.GetComponent<gunID>().gunDisappearServerRpc();
                    foreach (GameObject gun in GunSkins)
                    {
                        int gunid = gun.GetComponent<gunID>().id;
                        if (currentGun.GetComponent<gunID>().id == gunid)
                        {
                            Debug.Log("hi");
                            gun.gameObject.SetActive(true);
                        }
                        else
                        {
                            gun.gameObject.SetActive(false);
                        }
                    }
                    //currentGun.transform.SetParent(transform.parent);
                    changeParentServerRpc(currentGun, transform.parent.gameObject);
                    GetGunProperties(currentGun.GetComponent<Gun_properties>());
                    //absorb gun properties
                    //enable gun
                }
            }
        }


    }

    public void GetGunProperties(Gun_properties gp) {
        lrfreq = gp.ShootingFrequency;

    }

    [ServerRpc]
    void changeParentServerRpc(NetworkObjectReference target, NetworkObjectReference newParent,bool ignoreParent = false)
    {

        NetworkObject targetObject = target;
        NetworkObject newParentObject = newParent;

        if (ignoreParent)
        {
            targetObject.transform.SetParent(null);
        }
        else {
            targetObject.transform.SetParent(newParentObject.transform);
            targetObject.transform.localPosition = Vector3.up;
        }
          
       
        
            // deal damage or something to target object.
      
    }
    IEnumerator lrMf()
    {
        lrmf.SetActive(true);
        yield return new WaitForSeconds(.05f);
        lrmf.SetActive(false);
    }
    IEnumerator hit()
    {
        hitmarker.SetActive(true);
        yield return new WaitForSeconds(.05f);
        hitmarker.SetActive(false);
    }
    IEnumerator wait()
    {
        lrmf.SetActive(true);
        yield return new WaitForSeconds(1f);
        lrmf.SetActive(false);
    }
}
