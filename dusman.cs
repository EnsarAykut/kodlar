using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class dusman : MonoBehaviour
{
    NavMeshAgent ajan;// Düþmanýn hareketi için NavMeshAgent bileþeni
    GameObject hedef;// Düþmanýn hedef aldýðý oyun nesnesi
    public float health;// Düþmanýn saðlýk deðeri
    public float darbeGucu;// Düþmanýn darbe gücü
    GameObject anaKontrolcum;// Oyun kontrolcüsü oyun nesnesi
    Animator anim;// Düþmanýn animasyonlarý için Animator bileþeni

    void Start()
    {
        anim = GetComponent<Animator>();// Animator bileþenini al
        ajan = GetComponent<NavMeshAgent>();// NavMeshAgent bileþenini al
        anaKontrolcum = GameObject.FindWithTag("AnaKontrolcum");// "AnaKontrolcum" etiketine sahip oyun nesnesini bul

    }

    public void HedefBelirle(GameObject objem)
    {
        hedef = objem;// Hedef oyun nesnesini belirle

    }
    void Update()
    {
        ajan.SetDestination(hedef.transform.position);// Düþmanýn hedefe doðru ilerlemesini saðla
    }

    public void DarbeAl(float darbegucu)
    {

        health -= darbegucu;// Darbe gücü kadar saðlýðý azalt
        if (health <= 0)
        {
            oldun();// Eðer saðlýk deðeri 0 veya daha az ise düþmaný öldür
            gameObject.tag = "Untagged";// Düþmanýn etiketini "Untagged" olarak deðiþtir
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("hedef"))
        {
            
            anaKontrolcum.GetComponent<oyunKontrolu>().DarbeAl(darbeGucu);// Oyun kontrolcüsüne darbe gücünü ileterek hasar almasýný saðla
            oldun();// Düþmaný öldür

        }

    }
    public void oldun()
    {
        anaKontrolcum.GetComponent<oyunKontrolu>().DusmanSayisiGuncelle();// Oyun kontrolcüsündeki düþman sayýsýný güncelle
        anim.SetTrigger("olme"); // Ölme animasyonunu baþlat
        Destroy(gameObject, 5f);// 5 saniye sonra düþman nesnesini yok et
    }
}

}
