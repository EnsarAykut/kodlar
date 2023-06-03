using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombaKutusuOlustur : MonoBehaviour
{
    public List<GameObject> bombaKutusuPoint = new List<GameObject>();// Bomba kutusu oluþturulacak noktalarý içeren bir oyun nesnesi listesi
    public GameObject bombaKutusu;// Oluþturulacak bomba kutusu prefabý


    public static bool bombaKutusuVarmi;// Bomba kutusu var mý kontrolü için bool deðiþken
    int randomsayim; // Rastgele nokta indeksi

    void Start()
    {
        bombaKutusuVarmi = false;// Baþlangýçta bomba kutusu yok
        StartCoroutine(bombaKutusuYap());
    }



    IEnumerator bombaKutusuYap() {

        while (true)
        {
            yield return new WaitForSeconds(5f);// 5 saniyelik bir gecikme

            if (!bombaKutusuVarmi)// Eðer bomba kutusu yoksa
            {
                randomsayim = Random.Range(0, 1);// Rastgele bir nokta indeksi seç (0 veya 1)

                // Seçilen noktanýn konumuna ve rotasyonuna göre bomba kutusu prefabýný oluþtur
                Instantiate(bombaKutusu, bombaKutusuPoint[randomsayim].transform.position, bombaKutusuPoint[randomsayim].transform.rotation);

                bombaKutusuVarmi = true;// Bomba kutusu oluþturulduðunda bombaKutusuVarmi deðiþkenini true yap
            }
            
            
                                     
        }
      
    
    }

}
