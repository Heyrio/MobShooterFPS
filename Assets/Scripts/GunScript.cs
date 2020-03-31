using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public int maxAmmo = 5;
    public int ammo;
    public float reloadTime = 1f;

    public ParticleSystem muzzleFlash;
    public Camera fpsCam;
    private AudioSource mAudioSrc;
    public GameObject impactEffect;
    public AudioSource reloadPlayer;
    public AudioClip reload;
    public Animation anim;
    public Text ammoDisplay;
    // Update is called once per frame
    void Update()
    {   
        ammoDisplay.text = ammo.ToString() + "/5";

        if(Input.GetKeyDown(KeyCode.R) || ammo <= 0){
            StartCoroutine(Reload());
            return;
        }
        if(Input.GetButtonDown("Fire1") && ammo > 0){
            Shoot();
            return;
        }
    }
    void Start () {
        mAudioSrc = GetComponent<AudioSource>();
        ammo = maxAmmo;
    }

    IEnumerator Reload(){
        ammo = maxAmmo;
        reloadPlayer.clip = reload;
        reloadPlayer.Play();
        anim = gameObject.GetComponent<Animation>();
        anim.Play("Cube|CubeAction");
        yield return new WaitForSeconds(reloadTime);
    }

    void Shoot(){
        ammo--;
        mAudioSrc.Play();
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)){
            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
            if(target != null){
                target.TakeDamage(damage);
            }
        }
        Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
    }
}
