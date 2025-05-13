using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAgent : MonoBehaviour // gère les perso de manières individuel
{
    // La position de la cible qu'ils vont suivre, elle est donnée par PlayerController
    [HideInInspector] public Vector2 m_targetPos = Vector2.zero;
    // La vitesse de déplacement, elle est donnée par PlayerController
    [HideInInspector] public float m_moveSpeed = 10f;
    // Une valeur qui dit la distance au centre du joueur, c'est juste pour rendre le déplacement plus satisfaisant
    [HideInInspector] public float m_radiusFactor;
    private Rigidbody m_rb;

    // 🔹 Variables du tir 
    [SerializeField] private GameObject bulletPrefab; // Prefab de la balle
    [SerializeField] private Transform shootingSpot; // Position où les balles spawnent
    [SerializeField] public float bulletSpeed = 15f; // Vitesse des balles
    [SerializeField] private float shootingRate = 0.9f; // Intervalle entre chaque tir
    [SerializeField] private float SubshootingRate = 0.2f; // Intervalle entre chaque tir
    public PlayerController controller; // référence au controlleur
    [SerializeField] private CinemachineImpulseSource agentdeathImpulse;


    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        StartCoroutine(ShootContinuously());
    }

    private void Start()
    {
        transform.DOShakeScale(0.3f, 0.5f, 8);
    }

    // Update is called once per frame
    void Update()
    {


        // C'est ça qui le fait suivre la cible, tu devrais pas avoir a le modifier
        Vector3 targetPos = new Vector3(m_targetPos.x, transform.position.y, m_targetPos.y);
        
        Vector3 targetDir = targetPos - transform.position;
        
        

        m_rb.velocity = targetDir * (m_moveSpeed * m_radiusFactor);
    }

    // Ici tu peux rajouter ce qu'il faut pour qu'il tire, et pour détecter les collisions avec les bonus

    // TIRE 
    IEnumerator ShootContinuously()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(shootingRate);
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && shootingSpot != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootingSpot.position, shootingSpot.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {
                bulletRb.velocity = transform.forward * bulletSpeed;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bonus")
        {
            BonusAddChara bonusAddChara = other.GetComponent<BonusAddChara>();
            int playerstoadd = bonusAddChara.bonusValue;
            if (bonusAddChara.hitCount < bonusAddChara.maxHits) 
                {
                Destroy(other.gameObject);

                }
            else        
            {    
                controller.AddAgent(playerstoadd);
                Destroy(other.gameObject);
            }
            //int playerstoadd = bonusAddChara.bonusValue;
            //controller.AddAgent(playerstoadd);
            //Destroy(other.gameObject);
        }

        if (other.tag == "BonusShoot")
        {
            shootingRate -= SubshootingRate;

            // 🔹 Assure que shootingRate ne descend jamais sous 0.3
            shootingRate = Mathf.Max(shootingRate, 0.1f);

            Destroy(other.gameObject);
            print("plus");

        }
    }

    public void KillAgent()
    {
        controller.DeleteAgent(this);
        if (agentdeathImpulse)
            agentdeathImpulse.GenerateImpulse();
    }

}
