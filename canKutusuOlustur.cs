using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canKutusuOlustur : MonoBehaviour
{
    public List<GameObject> canKutusuPoint = new List<GameObject>();// Can kutusu oluþturulacak noktalarý içeren bir oyun nesnesi listesi
    public GameObject canKutusu;// Oluþturulacak can kutusu prefabý

    public static bool canKutusuVarmi;// Can kutusu var mý kontrolü için bool deðiþken
    int randomsayim; // Rastgele nokta indeksi

    void Start()
    {
        canKutusuVarmi = false;// Baþlangýçta can kutusu yok
        StartCoroutine(canKutusuYap());
    }



    IEnumerator canKutusuYap() {

        while (true)
        {
            yield return new WaitForSeconds(5f);// 5 saniyelik bir gecikme


            if (!canKutusuVarmi) {
                randomsayim = Random.Range(0, 1);// Rastgele bir nokta indeksi seç (0 veya 1)
                Instantiate(canKutusu, canKutusuPoint[randomsayim].transform.position, canKutusuPoint[randomsayim].transform.rotation);// Seçilen noktanýn konumuna ve rotasyonuna göre can kutusu prefabýný oluþtur
                canKutusuVarmi = true;// Can kutusu oluþturulduðunda canKutusuVarmi deðiþkenini true yap
            }
            
            
                                     
        }
      
    
    }

}
