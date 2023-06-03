using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class oyunKontrolu : MonoBehaviour
{
    float health = 100;// Oyuncunun sa�l�k de�eri
    [Header("Sa�l�k Ayarlar�")]
     public Image HealthBar;// Sa�l�k �ubu�unun referans�

    [Header("Silah Ayarlar�")]
    public GameObject[] silahlar;// Silahlar�n referanslar�
    public AudioSource degisimSesi;// Silah de�i�tirme sesi
    public GameObject bomba;// Bomba prefab�
    public GameObject bombaPoint;// Bomba atma noktas�
    public Camera camim;// Oyuncu kameras�


    [Header("D��man Ayarlar�")]
    public GameObject[] dusmanlar;// D��manlar�n referanslar�
    public GameObject[] cikisNoktalari;// D��man ��k�� noktalar�
    public GameObject[] hedefNoktalari;// D��man hedef noktalar�
    public float dusmanCikmaSuresi;// D��man ��kma s�resi
    public TextMeshProUGUI kalanDusmanText;// Kalan d��man say�s� metni
    public int baslangicDusmanSayisi;// Ba�lang��ta olu�turulacak d��man say�s�
    public static int kalanDusmanSayisi;// Kalan d��man say�s� 


    [Header("Di�er Ayarlar")]
    public GameObject GameOverCanvas;// kaybedildi�inde g�r�necek canvas
    public GameObject PauseCanvas;// Oyun duraklat�ld���nda g�r�necek canvas
    public GameObject KazandinCanvas;// Oyun kazan�ld���nda g�r�necek canvas
    public AudioSource OyunSes;// Oyunun sesi
    public TextMeshProUGUI saglikSayisiText;// Sa�l�k say�s� metni
    public TextMeshProUGUI bombaSayisiText;// Bomba say�s� metni

    public static bool oyunDurdumu;// Oyun durumu 








    void Start()
    {
        baslangicIslemleri();
        oyunDurdumu = false; // Oyun durdumu ba�lang��ta false olarak ayarland�


    }
    void baslangicIslemleri() 
    {
        // PlayerPrefs kullanarak oyun ba�lama ayarlar�n� kontrol etme
        if (!PlayerPrefs.HasKey("OyunBasladimi"))
        {
            PlayerPrefs.SetInt("m4_Mermi", 500);// Ba�lang��ta m4 silah�n�n mermi say�s� 500
            PlayerPrefs.SetInt("pistol_Mermi", 500);// Ba�lang��ta pistol silah�n�n mermi say�s� 500
            PlayerPrefs.SetInt("Bomba_sayisi", 5);// Ba�lang��ta bomba say�s� 5
            PlayerPrefs.SetInt("Saglik_sayisi", 1); // Ba�lang��ta sa�l�k say�s� 1

        }
        kalanDusmanText.text = baslangicDusmanSayisi.ToString();// Kalan d��man say�s�n� metne yazd�rma
        saglikSayisiText.text = PlayerPrefs.GetInt("Saglik_sayisi").ToString(); // Sa�l�k kutusu say�s�n� metne yazd�rma
        bombaSayisiText.text = PlayerPrefs.GetInt("Bomba_sayisi").ToString();// Bomba say�s�n� metne yazd�rma
        kalanDusmanSayisi = baslangicDusmanSayisi;// Kalan d��man say�s�n� ba�lang�� d��man say�s�yla e�itleme

        StartCoroutine(DusmanCikar());// D��manlar�n ��kmas�n� ba�latma
        OyunSes = GetComponent<AudioSource>();// Oyunun ses bile�enini almak
        OyunSes.Play();// Oyun sesini �al

    }

    IEnumerator DusmanCikar() 
    {
        while (true)
        {
            yield return new WaitForSeconds(dusmanCikmaSuresi);// Belirli bir s�re bekleme
            if (baslangicDusmanSayisi != 0) 
            {
                int dusman = Random.Range(0, 4);// Rastgele bir d��man se�me
                int cikisnoktasi = Random.Range(0, 3);// Rastgele bir ��k�� noktas� se�me
                int hedefnokta = Random.Range(0, 3); // Rastgele bir hedef noktas� se�me

                GameObject obje = Instantiate(dusmanlar[dusman], cikisNoktalari[cikisnoktasi].transform.position, Quaternion.identity);// D��man� olu�turma
                obje.GetComponent<dusman>().HedefBelirle(hedefNoktalari[hedefnokta]); // D��mana hedef noktas� belirleme
                baslangicDusmanSayisi--;// Kalan d��man say�s�n� azaltma
            }
            
        }
    
    }



    void Update()
    {
        // Klavye giri�lerini kontrol etme
        if (Input.GetKey(KeyCode.Alpha1) && !oyunDurdumu)
        {
           
            SilahDegistir(0);// Silah� de�i�tirme


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
            SaglikDoldur();// Sa�l�k doldurma
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !oyunDurdumu)
        {
            Pause();// Oyunu duraklatma

        }

    }

    public void DusmanSayisiGuncelle()
    {
        kalanDusmanSayisi--;// Kalan d��man say�s�n� azalt�r
        if (kalanDusmanSayisi <= 0)
        {           
            KazandinCanvas.SetActive(true);// Kazand�n�z ekran�n� g�r�n�r yapar
            Cursor.visible = true;

            if (Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.None;// Fareyi kilitli halinden normal haline d�nd�r�r.

            }
            kalanDusmanText.text = "0";// Kalan d��man say�s�n� metne yazd�rma
            Time.timeScale = 0; // Oyun zaman�n� durdurur
        }
        else 
        {
            kalanDusmanText.text = kalanDusmanSayisi.ToString();// Kalan d��man say�s�n� metne yazd�rma
        }
        

    }

    public void DarbeAl(float darbegucu) 
    {
        health -= darbegucu;
        HealthBar.fillAmount = health / 100;// Sa�l�k �ubu�unu g�ncelleme
        if (health <= 0)// sagl�k 0a e�it veya k���kse �al���r
        {
            GameOver();
        }


    }

    public void SaglikDoldur() 
    {
        if (PlayerPrefs.GetInt("Saglik_sayisi") !=0 && health != 100) 
        {
            health = 100;// Sa�l��� 100 yapar
            HealthBar.fillAmount = health / 100;// sa�l�k �ubu�unu fuller
            PlayerPrefs.SetInt("Saglik_sayisi", PlayerPrefs.GetInt("Saglik_sayisi") - 1);// Sa�l�k say�s�n� azaltma
            saglikSayisiText.text = PlayerPrefs.GetInt("Saglik_sayisi").ToString();// Sa�l�k say�s�n� metne yazd�rma
        }       
    }
    public void SaglikAl()
    {
        PlayerPrefs.SetInt("Saglik_sayisi", PlayerPrefs.GetInt("Saglik_sayisi") + 1);// Sa�l�k kutusunu 1 artt�r�r
        saglikSayisiText.text = PlayerPrefs.GetInt("Saglik_sayisi").ToString(); // Sa�l�k kutusunun textini g�nceller
    }

    void BombaAt() 
    {
        if (PlayerPrefs.GetInt("Bomba_sayisi") != 0) //Bomba say�s� 0a e�it de�ilse �al���r
        {
            GameObject obje = Instantiate(bomba, bombaPoint.transform.position, bombaPoint.transform.rotation);// Bombay� bombaPoint noktas�ndan olu�turur
            Rigidbody rb = obje.GetComponent<Rigidbody>();
            Vector3 acimiz = Quaternion.AngleAxis(90, camim.transform.forward) * camim.transform.forward;
            rb.AddForce(acimiz * 250f);// Bombay� ileri do�ru atmam�z� sa�lar
            PlayerPrefs.SetInt("Bomba_sayisi", PlayerPrefs.GetInt("Bomba_sayisi") - 1);// Bomba say�s�n� 1 d���r�r
            bombaSayisiText.text = PlayerPrefs.GetInt("Bomba_sayisi").ToString();// Bomba say�s�n�n textini g�nceller
        }
        
    }
    public void BombaAl()
    {
        PlayerPrefs.SetInt("Bomba_sayisi", PlayerPrefs.GetInt("Bomba_sayisi") + 1);// Bomba ald���m�zda 1 artar
        bombaSayisiText.text = PlayerPrefs.GetInt("Bomba_sayisi").ToString();// Bomba textini g�nceller
    }

    void SilahDegistir(int siranumarasi) {
        degisimSesi.Play();// silah de�i�tirirken �alar
        foreach (GameObject silah in silahlar)
        {
            silah.SetActive(false);// T�m silahlar� devre d��� b�rakma
        }
        silahlar[siranumarasi].SetActive(true); // Se�ilen silah� g�r�n�r yapar

    }
    void GameOver()
    {
        Cursor.visible = true;// Fareyi g�r�n�r yapar

        if (Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None;// Fare g�r�n�r ise fare kilidini kald�r�r
        }

        GameOverCanvas.SetActive(true);// Gameover canvas�n� g�r�n�r hale getirir
        Time.timeScale = 0;// oyunu durdurur
        oyunDurdumu = true;
       


    }

    public void BastanBasla() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);// Butona bast���m�zda oyun yeniden ba�lar
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
        Debug.Log("�al��t�");
        oyunDurdumu = false;

    }
    public void AnaMenu()
    {
        SceneManager.LoadScene(0);

       

    }
}
