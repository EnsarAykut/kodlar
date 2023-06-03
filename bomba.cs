using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomba : MonoBehaviour
{
    public float guc = 10f;// Bomban�n patlama g�c�
    public float menzil = 5f;// Bomban�n etkili oldu�u menzil
    public ParticleSystem PatlamaEfektim;// Patlama efekti i�in ParticleSystem bile�eni
    public AudioSource patlamaSesi;// Patlama sesi i�in AudioSource bile�eni




    void Start()
    {
        Destroy(gameObject,2f);// Oyun nesnesini 2 saniye sonra yok et
        patlamaSesi = GetComponent<AudioSource>();// AudioSource bile�enini al

    }

   
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null) // Herhangi bir �arp��ma olduysa
        {

            Patlama();
            
        }
    }

   void Patlama() 
    {
        
        patlamaSesi.Play();// Patlama sesini �al
        Vector3 patlamaPozisyonu = transform.position;// Patlaman�n ger�ekle�ti�i konumu al
        Instantiate(PatlamaEfektim, transform.position, transform.rotation);// Patlama efektini olu�tur
        Collider[] colliders = Physics.OverlapSphere(patlamaPozisyonu, menzil);// Belirtilen menzil i�erisindeki nesneleri al

        foreach (Collider hit in colliders) 
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (hit != null && rb) 
            {
                if (hit.gameObject.CompareTag("Dusman"))  // E�er �arp��an nesne "Dusman" etiketine sahipse
                {
                    hit.transform.gameObject.GetComponent<dusman>().oldun();// �arp��an d��man nesnesini �ld�r
                }
                rb.AddExplosionForce(guc, patlamaPozisyonu, menzil, 1, ForceMode.Impulse);// Patlama kuvvetini uygula
            }
        }
    
    }
}
