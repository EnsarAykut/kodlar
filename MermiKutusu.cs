using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MermiKutusu : MonoBehaviour
{
    string[] silahlar =// Silahların adlarını içeren bir dizi
        {
            "m4",
            "pistol"
            
        };

    int[] mermiSayisi =// mermi kutusunun mermi sayılarını içeren bir dizi
    {
            10,
            20,
            5,
            30

    };

    public List<Sprite> Silah_resimleri = new List<Sprite>();// Silahların resimlerini içeren bir sprite listesi
    public Image Silahin_resimi;// Silahın resmini tutan bir UI görüntüsü


    public string Olusan_Silahin_Turu;
    public int Olusan_Mermi_sayisi;
    public int Noktasi;// Mermi kutusunun bağlı olduğu noktanın indeksi

    void Start()
    {

         int gelenanahtar = Random.Range(0, silahlar.Length);// Silahlar dizisinde rastgele bir indeks seç

        Olusan_Silahin_Turu = silahlar[gelenanahtar];// Oluşan silahın türünü seçilen indekse göre ayarla
        Olusan_Mermi_sayisi = mermiSayisi[Random.Range(0, mermiSayisi.Length)];// Oluşan silahın mermi sayısını rastgele bir indeksten seç

        Silahin_resimi.sprite = Silah_resimleri[gelenanahtar];// Silahın resmini seçilen indekse göre ayarla

        


    }



}
