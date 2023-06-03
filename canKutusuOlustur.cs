using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canKutusuOlustur : MonoBehaviour
{
    public List<GameObject> canKutusuPoint = new List<GameObject>();// Can kutusu olu�turulacak noktalar� i�eren bir oyun nesnesi listesi
    public GameObject canKutusu;// Olu�turulacak can kutusu prefab�

    public static bool canKutusuVarmi;// Can kutusu var m� kontrol� i�in bool de�i�ken
    int randomsayim; // Rastgele nokta indeksi

    void Start()
    {
        canKutusuVarmi = false;// Ba�lang��ta can kutusu yok
        StartCoroutine(canKutusuYap());
    }



    IEnumerator canKutusuYap() {

        while (true)
        {
            yield return new WaitForSeconds(5f);// 5 saniyelik bir gecikme


            if (!canKutusuVarmi) {
                randomsayim = Random.Range(0, 1);// Rastgele bir nokta indeksi se� (0 veya 1)
                Instantiate(canKutusu, canKutusuPoint[randomsayim].transform.position, canKutusuPoint[randomsayim].transform.rotation);// Se�ilen noktan�n konumuna ve rotasyonuna g�re can kutusu prefab�n� olu�tur
                canKutusuVarmi = true;// Can kutusu olu�turuldu�unda canKutusuVarmi de�i�kenini true yap
            }
            
            
                                     
        }
      
    
    }

}
