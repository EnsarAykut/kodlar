using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombaKutusuOlustur : MonoBehaviour
{
    public List<GameObject> bombaKutusuPoint = new List<GameObject>();// Bomba kutusu olu�turulacak noktalar� i�eren bir oyun nesnesi listesi
    public GameObject bombaKutusu;// Olu�turulacak bomba kutusu prefab�


    public static bool bombaKutusuVarmi;// Bomba kutusu var m� kontrol� i�in bool de�i�ken
    int randomsayim; // Rastgele nokta indeksi

    void Start()
    {
        bombaKutusuVarmi = false;// Ba�lang��ta bomba kutusu yok
        StartCoroutine(bombaKutusuYap());
    }



    IEnumerator bombaKutusuYap() {

        while (true)
        {
            yield return new WaitForSeconds(5f);// 5 saniyelik bir gecikme

            if (!bombaKutusuVarmi)// E�er bomba kutusu yoksa
            {
                randomsayim = Random.Range(0, 1);// Rastgele bir nokta indeksi se� (0 veya 1)

                // Se�ilen noktan�n konumuna ve rotasyonuna g�re bomba kutusu prefab�n� olu�tur
                Instantiate(bombaKutusu, bombaKutusuPoint[randomsayim].transform.position, bombaKutusuPoint[randomsayim].transform.rotation);

                bombaKutusuVarmi = true;// Bomba kutusu olu�turuldu�unda bombaKutusuVarmi de�i�kenini true yap
            }
            
            
                                     
        }
      
    
    }

}
