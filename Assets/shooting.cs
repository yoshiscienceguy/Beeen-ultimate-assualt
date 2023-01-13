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
    public float explosiveforce = 200;
    public float exlposionradios = 25;

    // Start is called before the first frame update
    void Awake()
    {
        name = "Player " + GetComponentInParent<NetworkObject>().OwnerClientId.ToString();
        if (!GetComponentInParent<NetworkObject>().IsOwner) {
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
    public void PlayerShootGunServerRpc(Vector3 barrelo, Vector3 barrelTD, ServerRpcParams serverRpcParams = default)
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
                    victem.GetComponent<Rigidbody>().AddForce(barrel.transform.TransformDirection(Vector3.forward) * knokback, ForceMode.Impulse);

                    health healthScript = hit.collider.gameObject.GetComponent<health>();
                    if (healthScript != null)
                    {
                        healthScript.takedamage(3);
                    }
                }
            }


        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!GetComponentInParent<NetworkObject>().IsOwner) { return; }
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



                    PlayerShootGunServerRpc(barrel.position, barrel.TransformDirection(Vector3.forward));

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
