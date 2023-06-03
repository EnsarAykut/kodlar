using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomba : MonoBehaviour
{
    public float guc = 10f;// Bombanýn patlama gücü
    public float menzil = 5f;// Bombanýn etkili olduðu menzil
    public ParticleSystem PatlamaEfektim;// Patlama efekti için ParticleSystem bileþeni
    public AudioSource patlamaSesi;// Patlama sesi için AudioSource bileþeni




    void Start()
    {
        Destroy(gameObject,2f);// Oyun nesnesini 2 saniye sonra yok et
        patlamaSesi = GetComponent<AudioSource>();// AudioSource bileþenini al

    }

   
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null) // Herhangi bir çarpýþma olduysa
        {

            Patlama();
            
        }
    }

   void Patlama() 
    {
        
        patlamaSesi.Play();// Patlama sesini çal
        Vector3 patlamaPozisyonu = transform.position;// Patlamanýn gerçekleþtiði konumu al
        Instantiate(PatlamaEfektim, transform.position, transform.rotation);// Patlama efektini oluþtur
        Collider[] colliders = Physics.OverlapSphere(patlamaPozisyonu, menzil);// Belirtilen menzil içerisindeki nesneleri al

        foreach (Collider hit in colliders) 
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (hit != null && rb) 
            {
                if (hit.gameObject.CompareTag("Dusman"))  // Eðer çarpýþan nesne "Dusman" etiketine sahipse
                {
                    hit.transform.gameObject.GetComponent<dusman>().oldun();// Çarpýþan düþman nesnesini öldür
                }
                rb.AddExplosionForce(guc, patlamaPozisyonu, menzil, 1, ForceMode.Impulse);// Patlama kuvvetini uygula
            }
        }
    
    }
}
