using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class m4 : MonoBehaviour
{
    Animator anim;


    [Header("AYARLAR")]
    public bool atesEdebilirmi;// Ateþ edebilir mi kontrolü için boolean deðiþken
    float atesEtmeSikligi;// Ateþ etme sýklýðý için zaman aralýðý
    public float atesEtmeSiklik;// Ateþ etme aralýðý
    public float menzil;// Ateþin menzili
    public GameObject Cross;// Crosshair (niþangah) objesi




    [Header("SESLER")]
    public AudioSource atesSesi;// Ateþ sesi için Audio Source
    public AudioSource mermiBitisSesi;// Mermi bitiþ sesi için Audio Source
    public AudioSource mermiAlmaSesi;// Mermi alma sesi için Audio Source






    [Header("EFEKTLER")]
    public ParticleSystem atesEfekt;// Ateþ etme efekti için Particle System
    public ParticleSystem mermiÝzi;// Mermi izi efekti için Particle System
    public ParticleSystem KanEfekti;// Düþmana isabet edince çýkacak kan efekti için Particle System


    [Header("DÝGERLERÝ")]
    public Camera camim;// Oyuncu kamerasý
    float yaklasmaPOV = 40;// Yakýnlaþtýrma POV deðeri
    float camFieldPOV; // Orijinal POV deðeri

    [Header("SÝLAH AYARLARI")]

    int toplamMermiSayisi;// Toplam mermi sayýsý
    public int sarjorKapasitesi;// Sarjör kapasitesi
    int kalanMermi;// Kalan mermi sayýsý
    public string silahinAdi;// Silahýn adý
    bool zoomVarmi;// Yakýnlaþtýrma modu kontrolü
    public TextMeshProUGUI toplamMermi_Text;// Toplam mermi sayýsýnýn UI text'i
    public TextMeshProUGUI kalanMermi_Text;// Kalan mermi sayýsýnýn UI text'i
    public float DarbeGucu;// Ateþ sonucu uygulanacak darbe gücü

    public bool kovan_ciksinmi;// Kovan çýkýþý kontrolü
    public GameObject kovan_cikis_noktasi;// Kovan çýkýþ noktasý
    public GameObject kovan_objesi;// Kovan objesi
    public oyunKontrolu Kontrol;// Oyun kontrol scripti









    private void Start()
    {
        toplamMermiSayisi = PlayerPrefs.GetInt(silahinAdi + "_Mermi");// PlayerPrefs'ten toplam mermi sayýsý alýnýyor
        kovan_ciksinmi = true;// Kovan çýkýþý aktif durumda
        BaslangicMermiDoldur();// Baþlangýçta mermi doldurma iþlemi yapýlýyor
        sarjorDegistirme("normalYaz");
        anim = GetComponent<Animator>();
        camFieldPOV = camim.fieldOfView;
       


    }


    void Update()
    {

        if (Input.GetKey(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Mouse1))
        {
            if (atesEdebilirmi && Time.time > atesEtmeSikligi && kalanMermi != 0)// atesEdebilirmi true ve Time.time atesEtmeSikliginden büyükse ve kalanMermi 0a eþit deðilse
            {
                if (!oyunKontrolu.oyunDurdumu)
                {
                    AtesEt(false);
                    atesEtmeSikligi = Time.time + atesEtmeSiklik;
                }
            }
            if (kalanMermi == 0)
            {
                mermiBitisSesi.Play();

            }

        }



        if (Input.GetKey(KeyCode.R))// "R" tuþuna basýldýðýnda þarjör deðiþtirme animasyonu oynatýlýr.
        {
            sarjorDegistirmeAnim();
        }


        if (Input.GetKeyDown(KeyCode.E))// "E" tuþuna basýldýðýnda mermi alýnýr.
        {
            MermiAl();

        }

        if (Input.GetKeyDown(KeyCode.Mouse1))// Fare sað týklama tuþuna basýldýðýnda zoom yapýlýr.
        {
            Cross.SetActive(false);
            zoomVarmi = true;
            anim.SetBool("zoomyap", true);
            camim.fieldOfView = yaklasmaPOV;
            if (Input.GetKeyDown(KeyCode.Mouse0))// Fare sol týklama tuþuna basýlýrsa ateþ etme iþlemi gerçekleþtirilir.
            {
                if (atesEdebilirmi && Time.time > atesEtmeSikligi && kalanMermi != 0)
                {
                    AtesEt(true);
                    atesEtmeSikligi = Time.time + atesEtmeSiklik;
                }
                if (kalanMermi == 0)// Eðer kalan mermi sayýsý 0 ise, mermi bitiþ sesi çalýnýr:
                {
                    mermiBitisSesi.Play();

                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))// Fare sað týklama tuþu býrakýldýðýnda zoom iþlemi sonlandýrýlýr.
        {
            Cross.SetActive(true);
            zoomVarmi = false;
            anim.SetBool("zoomyap", false);
            if (Input.GetKey(KeyCode.Mouse0))// Fare sol týklama tuþuna basýlýrsa ateþ etme iþlemi gerçekleþtirilir.
            {
                if (atesEdebilirmi && Time.time > atesEtmeSikligi && kalanMermi != 0)
                {
                    AtesEt(false);
                    atesEtmeSikligi = Time.time + atesEtmeSiklik;
                }
                if (kalanMermi == 0)
                {
                    mermiBitisSesi.Play();

                }
            }
            camim.fieldOfView = camFieldPOV;
        }

    }

    private void OnTriggerEnter(Collider other)// Baþka bir collider'a girildiðinde tetiklenen fonksiyon
    {
        if (other.gameObject.CompareTag("Mermi"))// Eðer girilen collider "Mermi" etiketine sahip ise
        {
            // MermiKaydet fonksiyonuyla mermi bilgileri kaydedilir ve nesne yok edilir.
            MermiKaydet(other.transform.gameObject.GetComponent<MermiKutusu>().Olusan_Silahin_Turu, other.transform.gameObject.GetComponent<MermiKutusu>().Olusan_Mermi_sayisi);
            mermiKutusuOlustur.NoktalariKaldir(other.transform.gameObject.GetComponent<MermiKutusu>().Noktasi);
            Destroy(other.transform.parent.gameObject);

        }

        if (other.gameObject.CompareTag("Can kutusu")) // Eðer girilen collider "Can kutusu" etiketine sahip ise
        {
            // Saðlýk alýnýr, can kutusu durumu güncellenir ve nesne yok edilir.
            Kontrol.GetComponent<oyunKontrolu>().SaglikAl();
            canKutusuOlustur.canKutusuVarmi = false;
            Destroy(other.transform.gameObject);
        }

        if (other.gameObject.CompareTag("bomba kutusu"))// Eðer girilen collider "bomba kutusu" etiketine sahip ise
        {
            // Bomba alýnýr, bomba kutusu durumu güncellenir ve nesne yok edilir.
            Kontrol.GetComponent<oyunKontrolu>().BombaAl();
            bombaKutusuOlustur.bombaKutusuVarmi = false;
            Destroy(other.transform.gameObject);
        }
    }


    void sarjorDegistirmeAnim()// Þarjör deðiþtirme animasyonunu oynatan fonksiyon.
    {
        anim.Play("reloadAnim");
        if (kalanMermi < sarjorKapasitesi && toplamMermiSayisi != 0)// Eðer kalan mermi sayýsý þarjör kapasitesinden az ise
        {
            if (kalanMermi != 0)
            {
                sarjorDegistirme("mermiVar");
            }
            else
            {
                sarjorDegistirme("mermiYok");
            }
        }
    }

    void AtesEt(bool yakinlasmavarmi)// Ateþ etme iþlemini gerçekleþtiren fonksiyon
    {

        AtesEtmeTeknikÝslemleri(yakinlasmavarmi);// Ateþ etme teknik iþlemleri gerçekleþtirilir.
        RaycastHit hit;
        // Raycast ile çarpýþma kontrolü yapýlýr
        if (Physics.Raycast(camim.transform.position, camim.transform.forward, out hit, menzil))
        {

            if (hit.transform.gameObject.CompareTag("Dusman"))// Eðer çarpýþma "Dusman" etiketine sahip ise
            {
                // Kan efekti oluþturulur ve düþman darbe alýr.
                Instantiate(KanEfekti, hit.point, Quaternion.LookRotation(hit.normal));
                hit.transform.gameObject.GetComponent<dusman>().DarbeAl(DarbeGucu);
            }

            else if (hit.transform.gameObject.CompareTag("Devrilebilir"))
            {

                Rigidbody rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(-hit.normal * 50f);



            }

            else
            {
                Instantiate(mermiÝzi, hit.point, Quaternion.LookRotation(hit.normal));
            }

        }




    }

    void BaslangicMermiDoldur()
    {
        if (toplamMermiSayisi <= sarjorKapasitesi)
        {

            kalanMermi = toplamMermiSayisi;
            toplamMermiSayisi = 0;
            PlayerPrefs.SetInt(silahinAdi + "_Mermi", 0);
        }
        else
        {
            kalanMermi = sarjorKapasitesi;
            toplamMermiSayisi -= sarjorKapasitesi;
            PlayerPrefs.SetInt(silahinAdi + "_Mermi", toplamMermiSayisi);
        }




    }

    void sarjorDegistirme(string tur)
    {

        switch (tur)
        {
            case "mermiVar":

                if (toplamMermiSayisi <= sarjorKapasitesi)// Eðer toplam mermi sayýsý sarjör kapasitesinden küçük veya eþitse
                {

                    int olusanToplamDeger = kalanMermi + toplamMermiSayisi;// Oluþan toplam deðeri hesapla

                    if (olusanToplamDeger > sarjorKapasitesi)// Oluþan toplam deðer sarjör kapasitesinden büyükse
                    {
                        kalanMermi = sarjorKapasitesi;// Kalan mermi sayýsýný sarjör kapasitesine eþitle
                        toplamMermiSayisi = olusanToplamDeger - sarjorKapasitesi;// Toplam mermi sayýsýný oluþan toplam deðerin sarjör kapasitesinden çýkartýlmýþ hali olarak güncelle
                        PlayerPrefs.SetInt(silahinAdi + "_Mermi", toplamMermiSayisi); // PlayerPrefs ile silahýn adýný ve "_Mermi" ekini kullanarak toplam mermi sayýsýný güncelle

                    }
                    else
                    {
                        kalanMermi += toplamMermiSayisi;// Kalan mermi sayýsýný oluþan toplam deðere ekle
                        toplamMermiSayisi = 0; // Toplam mermi sayýsýný sýfýrla
                        PlayerPrefs.SetInt(silahinAdi + "_Mermi", 0);// PlayerPrefs ile silahýn adýný ve "_Mermi" ekini kullanarak toplam mermi sayýsýný sýfýrla
                    }

                }
                else
                {
                    toplamMermiSayisi -= sarjorKapasitesi - kalanMermi;// Toplam mermi sayýsýndan (sarjör kapasitesi - kalan mermi) kadar düþür
                    kalanMermi = sarjorKapasitesi;
                    PlayerPrefs.SetInt(silahinAdi + "_Mermi", toplamMermiSayisi);


                }
                // Toplam mermi sayýsý ve kalan mermi sayýsýný metinlere yaz
                toplamMermi_Text.text = toplamMermiSayisi.ToString();
                kalanMermi_Text.text = kalanMermi.ToString();

                break;

            case "mermiYok":
                if (toplamMermiSayisi <= sarjorKapasitesi)// Eðer toplam mermi sayýsý sarjör kapasitesinden küçük veya eþitse
                {
                    kalanMermi = toplamMermiSayisi;
                    toplamMermiSayisi = 0;
                    PlayerPrefs.SetInt(silahinAdi + "_Mermi", toplamMermiSayisi);

                }
                else
                {
                    toplamMermiSayisi -= sarjorKapasitesi;
                    PlayerPrefs.SetInt(silahinAdi + "_Mermi", toplamMermiSayisi);
                    kalanMermi = sarjorKapasitesi;


                }


                toplamMermi_Text.text = toplamMermiSayisi.ToString();
                kalanMermi_Text.text = kalanMermi.ToString();

                break;
            case "normalYaz":

                toplamMermi_Text.text = toplamMermiSayisi.ToString();
                kalanMermi_Text.text = kalanMermi.ToString();

                break;

        }


    }



    void AtesEtmeTeknikÝslemleri(bool yakinlasmavarmi)
    {
        atesSesi.Play();
        atesEfekt.Play();
        GameObject obje = Instantiate(kovan_objesi, kovan_cikis_noktasi.transform.position, kovan_cikis_noktasi.transform.rotation);// Kovana çýkan noktanýn pozisyonunda ve rotasyonunda yeni bir obje oluþtur
        Rigidbody rb1 = obje.GetComponent<Rigidbody>();// Oluþturulan objenin rigidbody bileþenini al
        rb1.AddRelativeForce(new Vector3(10, 1, 0) * 30);// Relatif bir kuvvet uygula (x: 10, y: 1, z: 0) * 30


        StartCoroutine(CamTitreme(.1f, .2f));// Kamera titreme efektini baþlat

        if (!yakinlasmavarmi)// Yakýnlaþtýrma var mý kontrol et
        {
            anim.Play("ateset");// Yakýnlaþtýrma yoksa animasyonu oynat ("ateset")
        }
        else
        {
            anim.Play("zoomateset");// Yakýnlaþtýrma varsa animasyonu oynat ("zoomateset")

        }


        kalanMermi--;// Kalan mermi sayýsýný azalt
        kalanMermi_Text.text = kalanMermi.ToString();// Kalan mermi sayýsýný güncelle



    }

    void MermiAl()// Mermi alýnma iþlemini gerçekleþtiren fonksiyon
    {
        RaycastHit hit;
        if (Physics.Raycast(camim.transform.position, camim.transform.forward, out hit, 3))
        {

            if (hit.transform.gameObject.CompareTag("Mermi"))
            {

                MermiKaydet(hit.transform.gameObject.GetComponent<MermiKutusu>().Olusan_Silahin_Turu, hit.transform.gameObject.GetComponent<MermiKutusu>().Olusan_Mermi_sayisi);
                mermiKutusuOlustur.NoktalariKaldir(hit.transform.gameObject.GetComponent<MermiKutusu>().Noktasi);
                Destroy(hit.transform.parent.gameObject);

            }
        }

    }

    void MermiKaydet(string silahturu, int mermisayisi)
    {

        mermiAlmaSesi.Play();// Mermi alma sesini çal


        switch (silahturu)
        {
            case "m4":
                toplamMermiSayisi += mermisayisi;// Toplam mermi sayýsýný artýr
                PlayerPrefs.SetInt("m4_Mermi", toplamMermiSayisi);// Artan toplam mermi sayýsýný "m4_Mermi" PlayerPref deðiþkenine kaydet
                sarjorDegistirme("normalYaz"); // "sarjorDegistirme" fonksiyonunu "normalYaz" parametresiyle çaðýr
                break;

            case "pistol":
                PlayerPrefs.SetInt("pistol_Mermi", PlayerPrefs.GetInt("pistol_Mermi") + mermisayisi);// "pistol_Mermi" PlayerPref deðiþkenine mermi sayýsýný ekle
                break;



        }

    }

    public void ScopeYaklastirma(bool durum)
    {
        camim.fieldOfView = camFieldPOV;
    }

    IEnumerator CamTitreme(float titremeSuresi, float magnitude)
    {

        Vector3 orijinalPozisyon = camim.transform.localPosition; // Kamera pozisyonunun orijinal deðerini sakla

        float gecenSure = 0.0f;// Geçen süreyi sýfýrla
        while (gecenSure < titremeSuresi)// Belirtilen süre boyunca titreme efektini uygula

        {
            float x = Random.Range(-1f, .1f) * magnitude;// Rasgele bir x deðeri oluþtur
            camim.transform.localPosition = new Vector3(x, orijinalPozisyon.y, orijinalPozisyon.x);// Kamera pozisyonunu güncelle, sadece x deðeri deðiþir
            gecenSure += Time.deltaTime;// Geçen süreyi artýr
            yield return null;

        }

        camim.transform.localPosition = orijinalPozisyon;// Titreme süresi bittiðinde, kamera pozisyonunu orijinal deðere geri döndür

    }


}
