using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mermiKutusuOlustur : MonoBehaviour
{
    public List<GameObject> mermiKutusuPoint = new List<GameObject>(); // Mermi kutusu olu�turulacak noktalar� i�eren bir oyun nesnesi listesi
    public GameObject MermiKutusu;// Olu�turulacak mermi kutusu prefab�


    public static bool mermiKutusuVarmi;// Mermi kutusu var m� kontrol� i�in bool de�i�ken
    static List<int> noktalar = new List<int>();// Mermi kutusu olu�turulacak noktalar�n indekslerini i�eren bir tamsay� listesi
    int randomsayim;// Rastgele nokta indeksi

    void Start()
    {
        StartCoroutine(MermiKutusuYap());
    }



    IEnumerator MermiKutusuYap() {

        while (true)
        {
            
                yield return new WaitForSeconds(5f);// 5 saniyelik bir gecikme
            randomsayim = Random.Range(0, 5);// Rastgele bir nokta indeksi se�

            if (!noktalar.Contains(randomsayim))// Se�ilen nokta indeksi daha �nce eklenmemi�se
            {
                noktalar.Add(randomsayim);// Nokta indeksini listeye ekle
            }
            else {
                randomsayim = Random.Range(0, 5);// Yeniden rastgele bir nokta indeksi se�

                continue;
            }

            // Mermi kutusu prefab�n� se�ilen noktan�n konumuna ve rotasyonuna g�re olu�tur
            GameObject objem = Instantiate(MermiKutusu, mermiKutusuPoint[randomsayim].transform.position, mermiKutusuPoint[randomsayim].transform.rotation);
            
            
            // Olu�turulan mermi kutusu bile�eninin Noktasi �zelli�ine rastgele nokta indeksini ata
            objem.transform.gameObject.GetComponentInChildren<MermiKutusu>().Noktasi = randomsayim;
                                     
        }
      
    
    }

    public static void NoktalariKaldir(int deger) {

        noktalar.Remove(deger);// Belirli bir nokta indeksini listeden kald�r

    }
}
