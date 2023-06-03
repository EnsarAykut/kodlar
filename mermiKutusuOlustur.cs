using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mermiKutusuOlustur : MonoBehaviour
{
    public List<GameObject> mermiKutusuPoint = new List<GameObject>(); // Mermi kutusu oluþturulacak noktalarý içeren bir oyun nesnesi listesi
    public GameObject MermiKutusu;// Oluþturulacak mermi kutusu prefabý


    public static bool mermiKutusuVarmi;// Mermi kutusu var mý kontrolü için bool deðiþken
    static List<int> noktalar = new List<int>();// Mermi kutusu oluþturulacak noktalarýn indekslerini içeren bir tamsayý listesi
    int randomsayim;// Rastgele nokta indeksi

    void Start()
    {
        StartCoroutine(MermiKutusuYap());
    }



    IEnumerator MermiKutusuYap() {

        while (true)
        {
            
                yield return new WaitForSeconds(5f);// 5 saniyelik bir gecikme
            randomsayim = Random.Range(0, 5);// Rastgele bir nokta indeksi seç

            if (!noktalar.Contains(randomsayim))// Seçilen nokta indeksi daha önce eklenmemiþse
            {
                noktalar.Add(randomsayim);// Nokta indeksini listeye ekle
            }
            else {
                randomsayim = Random.Range(0, 5);// Yeniden rastgele bir nokta indeksi seç

                continue;
            }

            // Mermi kutusu prefabýný seçilen noktanýn konumuna ve rotasyonuna göre oluþtur
            GameObject objem = Instantiate(MermiKutusu, mermiKutusuPoint[randomsayim].transform.position, mermiKutusuPoint[randomsayim].transform.rotation);
            
            
            // Oluþturulan mermi kutusu bileþeninin Noktasi özelliðine rastgele nokta indeksini ata
            objem.transform.gameObject.GetComponentInChildren<MermiKutusu>().Noktasi = randomsayim;
                                     
        }
      
    
    }

    public static void NoktalariKaldir(int deger) {

        noktalar.Remove(deger);// Belirli bir nokta indeksini listeden kaldýr

    }
}
