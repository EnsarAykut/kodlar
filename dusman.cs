using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class dusman : MonoBehaviour
{
    NavMeshAgent ajan;// D��man�n hareketi i�in NavMeshAgent bile�eni
    GameObject hedef;// D��man�n hedef ald��� oyun nesnesi
    public float health;// D��man�n sa�l�k de�eri
    public float darbeGucu;// D��man�n darbe g�c�
    GameObject anaKontrolcum;// Oyun kontrolc�s� oyun nesnesi
    Animator anim;// D��man�n animasyonlar� i�in Animator bile�eni

    void Start()
    {
        anim = GetComponent<Animator>();// Animator bile�enini al
        ajan = GetComponent<NavMeshAgent>();// NavMeshAgent bile�enini al
        anaKontrolcum = GameObject.FindWithTag("AnaKontrolcum");// "AnaKontrolcum" etiketine sahip oyun nesnesini bul

    }

    public void HedefBelirle(GameObject objem)
    {
        hedef = objem;// Hedef oyun nesnesini belirle

    }
    void Update()
    {
        ajan.SetDestination(hedef.transform.position);// D��man�n hedefe do�ru ilerlemesini sa�la
    }

    public void DarbeAl(float darbegucu)
    {

        health -= darbegucu;// Darbe g�c� kadar sa�l��� azalt
        if (health <= 0)
        {
            oldun();// E�er sa�l�k de�eri 0 veya daha az ise d��man� �ld�r
            gameObject.tag = "Untagged";// D��man�n etiketini "Untagged" olarak de�i�tir
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("hedef"))
        {
            
            anaKontrolcum.GetComponent<oyunKontrolu>().DarbeAl(darbeGucu);// Oyun kontrolc�s�ne darbe g�c�n� ileterek hasar almas�n� sa�la
            oldun();// D��man� �ld�r

        }

    }
    public void oldun()
    {
        anaKontrolcum.GetComponent<oyunKontrolu>().DusmanSayisiGuncelle();// Oyun kontrolc�s�ndeki d��man say�s�n� g�ncelle
        anim.SetTrigger("olme"); // �lme animasyonunu ba�lat
        Destroy(gameObject, 5f);// 5 saniye sonra d��man nesnesini yok et
    }
}

}
