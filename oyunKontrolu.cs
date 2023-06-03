using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class oyunKontrolu : MonoBehaviour
{
    float health = 100;// Oyuncunun saðlýk deðeri
    [Header("Saðlýk Ayarlarý")]
     public Image HealthBar;// Saðlýk çubuðunun referansý

    [Header("Silah Ayarlarý")]
    public GameObject[] silahlar;// Silahlarýn referanslarý
    public AudioSource degisimSesi;// Silah deðiþtirme sesi
    public GameObject bomba;// Bomba prefabý
    public GameObject bombaPoint;// Bomba atma noktasý
    public Camera camim;// Oyuncu kamerasý


    [Header("Düþman Ayarlarý")]
    public GameObject[] dusmanlar;// Düþmanlarýn referanslarý
    public GameObject[] cikisNoktalari;// Düþman çýkýþ noktalarý
    public GameObject[] hedefNoktalari;// Düþman hedef noktalarý
    public float dusmanCikmaSuresi;// Düþman çýkma süresi
    public TextMeshProUGUI kalanDusmanText;// Kalan düþman sayýsý metni
    public int baslangicDusmanSayisi;// Baþlangýçta oluþturulacak düþman sayýsý
    public static int kalanDusmanSayisi;// Kalan düþman sayýsý 


    [Header("Diðer Ayarlar")]
    public GameObject GameOverCanvas;// kaybedildiðinde görünecek canvas
    public GameObject PauseCanvas;// Oyun duraklatýldýðýnda görünecek canvas
    public GameObject KazandinCanvas;// Oyun kazanýldýðýnda görünecek canvas
    public AudioSource OyunSes;// Oyunun sesi
    public TextMeshProUGUI saglikSayisiText;// Saðlýk sayýsý metni
    public TextMeshProUGUI bombaSayisiText;// Bomba sayýsý metni

    public static bool oyunDurdumu;// Oyun durumu 








    void Start()
    {
        baslangicIslemleri();
        oyunDurdumu = false; // Oyun durdumu baþlangýçta false olarak ayarlandý


    }
    void baslangicIslemleri() 
    {
        // PlayerPrefs kullanarak oyun baþlama ayarlarýný kontrol etme
        if (!PlayerPrefs.HasKey("OyunBasladimi"))
        {
            PlayerPrefs.SetInt("m4_Mermi", 500);// Baþlangýçta m4 silahýnýn mermi sayýsý 500
            PlayerPrefs.SetInt("pistol_Mermi", 500);// Baþlangýçta pistol silahýnýn mermi sayýsý 500
            PlayerPrefs.SetInt("Bomba_sayisi", 5);// Baþlangýçta bomba sayýsý 5
            PlayerPrefs.SetInt("Saglik_sayisi", 1); // Baþlangýçta saðlýk sayýsý 1

        }
        kalanDusmanText.text = baslangicDusmanSayisi.ToString();// Kalan düþman sayýsýný metne yazdýrma
        saglikSayisiText.text = PlayerPrefs.GetInt("Saglik_sayisi").ToString(); // Saðlýk kutusu sayýsýný metne yazdýrma
        bombaSayisiText.text = PlayerPrefs.GetInt("Bomba_sayisi").ToString();// Bomba sayýsýný metne yazdýrma
        kalanDusmanSayisi = baslangicDusmanSayisi;// Kalan düþman sayýsýný baþlangýç düþman sayýsýyla eþitleme

        StartCoroutine(DusmanCikar());// Düþmanlarýn çýkmasýný baþlatma
        OyunSes = GetComponent<AudioSource>();// Oyunun ses bileþenini almak
        OyunSes.Play();// Oyun sesini çal

    }

    IEnumerator DusmanCikar() 
    {
        while (true)
        {
            yield return new WaitForSeconds(dusmanCikmaSuresi);// Belirli bir süre bekleme
            if (baslangicDusmanSayisi != 0) 
            {
                int dusman = Random.Range(0, 4);// Rastgele bir düþman seçme
                int cikisnoktasi = Random.Range(0, 3);// Rastgele bir çýkýþ noktasý seçme
                int hedefnokta = Random.Range(0, 3); // Rastgele bir hedef noktasý seçme

                GameObject obje = Instantiate(dusmanlar[dusman], cikisNoktalari[cikisnoktasi].transform.position, Quaternion.identity);// Düþmaný oluþturma
                obje.GetComponent<dusman>().HedefBelirle(hedefNoktalari[hedefnokta]); // Düþmana hedef noktasý belirleme
                baslangicDusmanSayisi--;// Kalan düþman sayýsýný azaltma
            }
            
        }
    
    }



    void Update()
    {
        // Klavye giriþlerini kontrol etme
        if (Input.GetKey(KeyCode.Alpha1) && !oyunDurdumu)
        {
           
            SilahDegistir(0);// Silahý deðiþtirme


        }
        if (Input.GetKey(KeyCode.Alpha2) && !oyunDurdumu)
        {
            SilahDegistir(1);
            
        }
        if (Input.GetKeyDown(KeyCode.G) && !oyunDurdumu)
        {
            BombaAt();// Bomba atma
        }
        if (Input.GetKeyDown(KeyCode.E) && !oyunDurdumu)
        {
            SaglikDoldur();// Saðlýk doldurma
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !oyunDurdumu)
        {
            Pause();// Oyunu duraklatma

        }

    }

    public void DusmanSayisiGuncelle()
    {
        kalanDusmanSayisi--;// Kalan düþman sayýsýný azaltýr
        if (kalanDusmanSayisi <= 0)
        {           
            KazandinCanvas.SetActive(true);// Kazandýnýz ekranýný görünür yapar
            Cursor.visible = true;

            if (Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.None;// Fareyi kilitli halinden normal haline döndürür.

            }
            kalanDusmanText.text = "0";// Kalan düþman sayýsýný metne yazdýrma
            Time.timeScale = 0; // Oyun zamanýný durdurur
        }
        else 
        {
            kalanDusmanText.text = kalanDusmanSayisi.ToString();// Kalan düþman sayýsýný metne yazdýrma
        }
        

    }

    public void DarbeAl(float darbegucu) 
    {
        health -= darbegucu;
        HealthBar.fillAmount = health / 100;// Saðlýk çubuðunu güncelleme
        if (health <= 0)// saglýk 0a eþit veya küçükse çalýþýr
        {
            GameOver();
        }


    }

    public void SaglikDoldur() 
    {
        if (PlayerPrefs.GetInt("Saglik_sayisi") !=0 && health != 100) 
        {
            health = 100;// Saðlýðý 100 yapar
            HealthBar.fillAmount = health / 100;// saðlýk çubuðunu fuller
            PlayerPrefs.SetInt("Saglik_sayisi", PlayerPrefs.GetInt("Saglik_sayisi") - 1);// Saðlýk sayýsýný azaltma
            saglikSayisiText.text = PlayerPrefs.GetInt("Saglik_sayisi").ToString();// Saðlýk sayýsýný metne yazdýrma
        }       
    }
    public void SaglikAl()
    {
        PlayerPrefs.SetInt("Saglik_sayisi", PlayerPrefs.GetInt("Saglik_sayisi") + 1);// Saðlýk kutusunu 1 arttýrýr
        saglikSayisiText.text = PlayerPrefs.GetInt("Saglik_sayisi").ToString(); // Saðlýk kutusunun textini günceller
    }

    void BombaAt() 
    {
        if (PlayerPrefs.GetInt("Bomba_sayisi") != 0) //Bomba sayýsý 0a eþit deðilse çalýþýr
        {
            GameObject obje = Instantiate(bomba, bombaPoint.transform.position, bombaPoint.transform.rotation);// Bombayý bombaPoint noktasýndan oluþturur
            Rigidbody rb = obje.GetComponent<Rigidbody>();
            Vector3 acimiz = Quaternion.AngleAxis(90, camim.transform.forward) * camim.transform.forward;
            rb.AddForce(acimiz * 250f);// Bombayý ileri doðru atmamýzý saðlar
            PlayerPrefs.SetInt("Bomba_sayisi", PlayerPrefs.GetInt("Bomba_sayisi") - 1);// Bomba sayýsýný 1 düþürür
            bombaSayisiText.text = PlayerPrefs.GetInt("Bomba_sayisi").ToString();// Bomba sayýsýnýn textini günceller
        }
        
    }
    public void BombaAl()
    {
        PlayerPrefs.SetInt("Bomba_sayisi", PlayerPrefs.GetInt("Bomba_sayisi") + 1);// Bomba aldýðýmýzda 1 artar
        bombaSayisiText.text = PlayerPrefs.GetInt("Bomba_sayisi").ToString();// Bomba textini günceller
    }

    void SilahDegistir(int siranumarasi) {
        degisimSesi.Play();// silah deðiþtirirken çalar
        foreach (GameObject silah in silahlar)
        {
            silah.SetActive(false);// Tüm silahlarý devre dýþý býrakma
        }
        silahlar[siranumarasi].SetActive(true); // Seçilen silahý görünür yapar

    }
    void GameOver()
    {
        Cursor.visible = true;// Fareyi görünür yapar

        if (Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None;// Fare görünür ise fare kilidini kaldýrýr
        }

        GameOverCanvas.SetActive(true);// Gameover canvasýný görünür hale getirir
        Time.timeScale = 0;// oyunu durdurur
        oyunDurdumu = true;
       


    }

    public void BastanBasla() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);// Butona bastýðýmýzda oyun yeniden baþlar
        Time.timeScale = 1;//Oyunu devam ettirir
        oyunDurdumu = false;

    }
    public void Pause()
    {
        Cursor.visible = true;

        if (Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        PauseCanvas.SetActive(true);
        Time.timeScale = 0;
        oyunDurdumu = true;

    }
    public void DevamEt()
    {
        PauseCanvas.SetActive(false);
        Time.timeScale = 1;
        Debug.Log("çalýþtý");
        oyunDurdumu = false;

    }
    public void AnaMenu()
    {
        SceneManager.LoadScene(0);

       

    }
}
