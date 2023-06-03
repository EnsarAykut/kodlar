using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class m4 : MonoBehaviour
{
    Animator anim;


    [Header("AYARLAR")]
    public bool atesEdebilirmi;// Ate� edebilir mi kontrol� i�in boolean de�i�ken
    float atesEtmeSikligi;// Ate� etme s�kl��� i�in zaman aral���
    public float atesEtmeSiklik;// Ate� etme aral���
    public float menzil;// Ate�in menzili
    public GameObject Cross;// Crosshair (ni�angah) objesi




    [Header("SESLER")]
    public AudioSource atesSesi;// Ate� sesi i�in Audio Source
    public AudioSource mermiBitisSesi;// Mermi biti� sesi i�in Audio Source
    public AudioSource mermiAlmaSesi;// Mermi alma sesi i�in Audio Source






    [Header("EFEKTLER")]
    public ParticleSystem atesEfekt;// Ate� etme efekti i�in Particle System
    public ParticleSystem mermi�zi;// Mermi izi efekti i�in Particle System
    public ParticleSystem KanEfekti;// D��mana isabet edince ��kacak kan efekti i�in Particle System


    [Header("D�GERLER�")]
    public Camera camim;// Oyuncu kameras�
    float yaklasmaPOV = 40;// Yak�nla�t�rma POV de�eri
    float camFieldPOV; // Orijinal POV de�eri

    [Header("S�LAH AYARLARI")]

    int toplamMermiSayisi;// Toplam mermi say�s�
    public int sarjorKapasitesi;// Sarj�r kapasitesi
    int kalanMermi;// Kalan mermi say�s�
    public string silahinAdi;// Silah�n ad�
    bool zoomVarmi;// Yak�nla�t�rma modu kontrol�
    public TextMeshProUGUI toplamMermi_Text;// Toplam mermi say�s�n�n UI text'i
    public TextMeshProUGUI kalanMermi_Text;// Kalan mermi say�s�n�n UI text'i
    public float DarbeGucu;// Ate� sonucu uygulanacak darbe g�c�

    public bool kovan_ciksinmi;// Kovan ��k��� kontrol�
    public GameObject kovan_cikis_noktasi;// Kovan ��k�� noktas�
    public GameObject kovan_objesi;// Kovan objesi
    public oyunKontrolu Kontrol;// Oyun kontrol scripti









    private void Start()
    {
        toplamMermiSayisi = PlayerPrefs.GetInt(silahinAdi + "_Mermi");// PlayerPrefs'ten toplam mermi say�s� al�n�yor
        kovan_ciksinmi = true;// Kovan ��k��� aktif durumda
        BaslangicMermiDoldur();// Ba�lang��ta mermi doldurma i�lemi yap�l�yor
        sarjorDegistirme("normalYaz");
        anim = GetComponent<Animator>();
        camFieldPOV = camim.fieldOfView;
       


    }


    void Update()
    {

        if (Input.GetKey(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Mouse1))
        {
            if (atesEdebilirmi && Time.time > atesEtmeSikligi && kalanMermi != 0)// atesEdebilirmi true ve Time.time atesEtmeSikliginden b�y�kse ve kalanMermi 0a e�it de�ilse
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



        if (Input.GetKey(KeyCode.R))// "R" tu�una bas�ld���nda �arj�r de�i�tirme animasyonu oynat�l�r.
        {
            sarjorDegistirmeAnim();
        }


        if (Input.GetKeyDown(KeyCode.E))// "E" tu�una bas�ld���nda mermi al�n�r.
        {
            MermiAl();

        }

        if (Input.GetKeyDown(KeyCode.Mouse1))// Fare sa� t�klama tu�una bas�ld���nda zoom yap�l�r.
        {
            Cross.SetActive(false);
            zoomVarmi = true;
            anim.SetBool("zoomyap", true);
            camim.fieldOfView = yaklasmaPOV;
            if (Input.GetKeyDown(KeyCode.Mouse0))// Fare sol t�klama tu�una bas�l�rsa ate� etme i�lemi ger�ekle�tirilir.
            {
                if (atesEdebilirmi && Time.time > atesEtmeSikligi && kalanMermi != 0)
                {
                    AtesEt(true);
                    atesEtmeSikligi = Time.time + atesEtmeSiklik;
                }
                if (kalanMermi == 0)// E�er kalan mermi say�s� 0 ise, mermi biti� sesi �al�n�r:
                {
                    mermiBitisSesi.Play();

                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))// Fare sa� t�klama tu�u b�rak�ld���nda zoom i�lemi sonland�r�l�r.
        {
            Cross.SetActive(true);
            zoomVarmi = false;
            anim.SetBool("zoomyap", false);
            if (Input.GetKey(KeyCode.Mouse0))// Fare sol t�klama tu�una bas�l�rsa ate� etme i�lemi ger�ekle�tirilir.
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

    private void OnTriggerEnter(Collider other)// Ba�ka bir collider'a girildi�inde tetiklenen fonksiyon
    {
        if (other.gameObject.CompareTag("Mermi"))// E�er girilen collider "Mermi" etiketine sahip ise
        {
            // MermiKaydet fonksiyonuyla mermi bilgileri kaydedilir ve nesne yok edilir.
            MermiKaydet(other.transform.gameObject.GetComponent<MermiKutusu>().Olusan_Silahin_Turu, other.transform.gameObject.GetComponent<MermiKutusu>().Olusan_Mermi_sayisi);
            mermiKutusuOlustur.NoktalariKaldir(other.transform.gameObject.GetComponent<MermiKutusu>().Noktasi);
            Destroy(other.transform.parent.gameObject);

        }

        if (other.gameObject.CompareTag("Can kutusu")) // E�er girilen collider "Can kutusu" etiketine sahip ise
        {
            // Sa�l�k al�n�r, can kutusu durumu g�ncellenir ve nesne yok edilir.
            Kontrol.GetComponent<oyunKontrolu>().SaglikAl();
            canKutusuOlustur.canKutusuVarmi = false;
            Destroy(other.transform.gameObject);
        }

        if (other.gameObject.CompareTag("bomba kutusu"))// E�er girilen collider "bomba kutusu" etiketine sahip ise
        {
            // Bomba al�n�r, bomba kutusu durumu g�ncellenir ve nesne yok edilir.
            Kontrol.GetComponent<oyunKontrolu>().BombaAl();
            bombaKutusuOlustur.bombaKutusuVarmi = false;
            Destroy(other.transform.gameObject);
        }
    }


    void sarjorDegistirmeAnim()// �arj�r de�i�tirme animasyonunu oynatan fonksiyon.
    {
        anim.Play("reloadAnim");
        if (kalanMermi < sarjorKapasitesi && toplamMermiSayisi != 0)// E�er kalan mermi say�s� �arj�r kapasitesinden az ise
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

    void AtesEt(bool yakinlasmavarmi)// Ate� etme i�lemini ger�ekle�tiren fonksiyon
    {

        AtesEtmeTeknik�slemleri(yakinlasmavarmi);// Ate� etme teknik i�lemleri ger�ekle�tirilir.
        RaycastHit hit;
        // Raycast ile �arp��ma kontrol� yap�l�r
        if (Physics.Raycast(camim.transform.position, camim.transform.forward, out hit, menzil))
        {

            if (hit.transform.gameObject.CompareTag("Dusman"))// E�er �arp��ma "Dusman" etiketine sahip ise
            {
                // Kan efekti olu�turulur ve d��man darbe al�r.
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
                Instantiate(mermi�zi, hit.point, Quaternion.LookRotation(hit.normal));
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

                if (toplamMermiSayisi <= sarjorKapasitesi)// E�er toplam mermi say�s� sarj�r kapasitesinden k���k veya e�itse
                {

                    int olusanToplamDeger = kalanMermi + toplamMermiSayisi;// Olu�an toplam de�eri hesapla

                    if (olusanToplamDeger > sarjorKapasitesi)// Olu�an toplam de�er sarj�r kapasitesinden b�y�kse
                    {
                        kalanMermi = sarjorKapasitesi;// Kalan mermi say�s�n� sarj�r kapasitesine e�itle
                        toplamMermiSayisi = olusanToplamDeger - sarjorKapasitesi;// Toplam mermi say�s�n� olu�an toplam de�erin sarj�r kapasitesinden ��kart�lm�� hali olarak g�ncelle
                        PlayerPrefs.SetInt(silahinAdi + "_Mermi", toplamMermiSayisi); // PlayerPrefs ile silah�n ad�n� ve "_Mermi" ekini kullanarak toplam mermi say�s�n� g�ncelle

                    }
                    else
                    {
                        kalanMermi += toplamMermiSayisi;// Kalan mermi say�s�n� olu�an toplam de�ere ekle
                        toplamMermiSayisi = 0; // Toplam mermi say�s�n� s�f�rla
                        PlayerPrefs.SetInt(silahinAdi + "_Mermi", 0);// PlayerPrefs ile silah�n ad�n� ve "_Mermi" ekini kullanarak toplam mermi say�s�n� s�f�rla
                    }

                }
                else
                {
                    toplamMermiSayisi -= sarjorKapasitesi - kalanMermi;// Toplam mermi say�s�ndan (sarj�r kapasitesi - kalan mermi) kadar d���r
                    kalanMermi = sarjorKapasitesi;
                    PlayerPrefs.SetInt(silahinAdi + "_Mermi", toplamMermiSayisi);


                }
                // Toplam mermi say�s� ve kalan mermi say�s�n� metinlere yaz
                toplamMermi_Text.text = toplamMermiSayisi.ToString();
                kalanMermi_Text.text = kalanMermi.ToString();

                break;

            case "mermiYok":
                if (toplamMermiSayisi <= sarjorKapasitesi)// E�er toplam mermi say�s� sarj�r kapasitesinden k���k veya e�itse
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



    void AtesEtmeTeknik�slemleri(bool yakinlasmavarmi)
    {
        atesSesi.Play();
        atesEfekt.Play();
        GameObject obje = Instantiate(kovan_objesi, kovan_cikis_noktasi.transform.position, kovan_cikis_noktasi.transform.rotation);// Kovana ��kan noktan�n pozisyonunda ve rotasyonunda yeni bir obje olu�tur
        Rigidbody rb1 = obje.GetComponent<Rigidbody>();// Olu�turulan objenin rigidbody bile�enini al
        rb1.AddRelativeForce(new Vector3(10, 1, 0) * 30);// Relatif bir kuvvet uygula (x: 10, y: 1, z: 0) * 30


        StartCoroutine(CamTitreme(.1f, .2f));// Kamera titreme efektini ba�lat

        if (!yakinlasmavarmi)// Yak�nla�t�rma var m� kontrol et
        {
            anim.Play("ateset");// Yak�nla�t�rma yoksa animasyonu oynat ("ateset")
        }
        else
        {
            anim.Play("zoomateset");// Yak�nla�t�rma varsa animasyonu oynat ("zoomateset")

        }


        kalanMermi--;// Kalan mermi say�s�n� azalt
        kalanMermi_Text.text = kalanMermi.ToString();// Kalan mermi say�s�n� g�ncelle



    }

    void MermiAl()// Mermi al�nma i�lemini ger�ekle�tiren fonksiyon
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

        mermiAlmaSesi.Play();// Mermi alma sesini �al


        switch (silahturu)
        {
            case "m4":
                toplamMermiSayisi += mermisayisi;// Toplam mermi say�s�n� art�r
                PlayerPrefs.SetInt("m4_Mermi", toplamMermiSayisi);// Artan toplam mermi say�s�n� "m4_Mermi" PlayerPref de�i�kenine kaydet
                sarjorDegistirme("normalYaz"); // "sarjorDegistirme" fonksiyonunu "normalYaz" parametresiyle �a��r
                break;

            case "pistol":
                PlayerPrefs.SetInt("pistol_Mermi", PlayerPrefs.GetInt("pistol_Mermi") + mermisayisi);// "pistol_Mermi" PlayerPref de�i�kenine mermi say�s�n� ekle
                break;



        }

    }

    public void ScopeYaklastirma(bool durum)
    {
        camim.fieldOfView = camFieldPOV;
    }

    IEnumerator CamTitreme(float titremeSuresi, float magnitude)
    {

        Vector3 orijinalPozisyon = camim.transform.localPosition; // Kamera pozisyonunun orijinal de�erini sakla

        float gecenSure = 0.0f;// Ge�en s�reyi s�f�rla
        while (gecenSure < titremeSuresi)// Belirtilen s�re boyunca titreme efektini uygula

        {
            float x = Random.Range(-1f, .1f) * magnitude;// Rasgele bir x de�eri olu�tur
            camim.transform.localPosition = new Vector3(x, orijinalPozisyon.y, orijinalPozisyon.x);// Kamera pozisyonunu g�ncelle, sadece x de�eri de�i�ir
            gecenSure += Time.deltaTime;// Ge�en s�reyi art�r
            yield return null;

        }

        camim.transform.localPosition = orijinalPozisyon;// Titreme s�resi bitti�inde, kamera pozisyonunu orijinal de�ere geri d�nd�r

    }


}
