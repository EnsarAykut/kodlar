using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boskovan : MonoBehaviour
{

    AudioSource yeredusmesesi;// Yer düşmesi sesi için AudioSource bileşeni
    void Start()
    {
        yeredusmesesi = GetComponent<AudioSource>();// AudioSource bileşenini al
        Destroy(gameObject, 2f);    // kovan nesnesini 2 saniye sonra yok et    
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Yol"))// Çarpışan nesne "Yol" etiketine sahipse
        {
            yeredusmesesi.Play();// Yer düşmesi sesini çal

            if (!yeredusmesesi.isPlaying)// Ses çalınmıyorsa
            {
                Destroy(gameObject,1f);// Oyun nesnesini 1 saniye sonra yok et

            }
           

        }
    }

   
}
