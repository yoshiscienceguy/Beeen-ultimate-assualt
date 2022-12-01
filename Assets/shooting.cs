using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shooting : MonoBehaviour
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
    public Text ammo;
    public GameObject hitmarker;
    public bool reloading;
    public Animator anim;
    public bool canshoot;
    
    // Start is called before the first frame update
    void Start()
    {
        lrcurrentmag = lrmaxmag;
        ammo.text = lrcurrentmag.ToString() + "/" + lrmaxmag.ToString();
        reloading = false;
        canshoot = true;
    }
    
    public void reloaded()
    {
        reloading = false;
    }
    // Update is called once per frame
    void Update()
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
                    RaycastHit hit;
                    if (Physics.Raycast(barrel.position, barrel.TransformDirection(Vector3.forward), out hit, lrrange, lm))
                    {
                        GameObject victem = hit.collider.gameObject;
                        if (victem.GetComponent<Rigidbody>())
                        {
                            StartCoroutine("hit");
                            victem.GetComponent<Rigidbody>().AddForce(barrel.transform.TransformDirection(Vector3.forward) * knokback, ForceMode.Impulse);
                        }

                    }
                    StartCoroutine("lrMf");
                    if (lrcurrentmag > 0)
                    {
                        lrcurrentmag -= 1;
                    }
                    ammo.text = lrcurrentmag.ToString() + "/" + lrmaxmag.ToString();
                    lrlight.intensity = (lrcurrentmag / lrmaxmag) * 10.0f;
                    Debug.Log((lrcurrentmag / lrmaxmag));
                    Debug.Log((lrcurrentmag / lrmaxmag) * 10.0f);
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
