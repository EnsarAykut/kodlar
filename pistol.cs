using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class pistol : MonoBehaviour
{
    Animator anim;


    [Header("AYARLAR")]
    public bool atesEdebilirmi;
    float atesEtmeSikligi;
    public float atesEtmeSiklik;
    public float menzil;
    public GameObject Cross;
    



    [Header("SESLER")]
    public AudioSource atesSesi;
    public AudioSource mermiBitisSesi;
    public AudioSource mermiAlmaSesi;





    [Header("EFEKTLER")]
    public ParticleSystem atesEfekt;
    public ParticleSystem mermi›zi;
    public ParticleSystem KanEfekti;


    [Header("D›GERLER›")]
    public Camera camim;
    float yaklasmaPOV = 40;
    float camFieldPOV;

    [Header("S›LAH AYARLARI")]

    int toplamMermiSayisi;
    public int sarjorKapasitesi;
    int kalanMermi;
    public string silahinAdi;
    bool zoomVarmi;
    public TextMeshProUGUI toplamMermi_Text;
    public TextMeshProUGUI kalanMermi_Text;
    public float DarbeGucu;

    public bool kovan_ciksinmi;
    public GameObject kovan_cikis_noktasi;
    public GameObject kovan_objesi;   
    public oyunKontrolu Kontrol;
    







    private void Start()
    {
        toplamMermiSayisi = PlayerPrefs.GetInt(silahinAdi + "_Mermi");
        kovan_ciksinmi = true;
        BaslangicMermiDoldur();
        sarjorDegistirme("normalYaz");
        anim = GetComponent<Animator>();
        camFieldPOV = camim.fieldOfView;
        


    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Mouse1)) {
            if (atesEdebilirmi && Time.time > atesEtmeSikligi && kalanMermi != 0)
            {
                if (!oyunKontrolu.oyunDurdumu) 
                {
                    AtesEt(false);
                    atesEtmeSikligi = Time.time + atesEtmeSiklik;
                }
                
            }
            if(kalanMermi == 0) {
                mermiBitisSesi.Play();
            
            }

            }



        if (Input.GetKey(KeyCode.R) )
        {
            sarjorDegistirmeAnim();
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            MermiAl();
            
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Cross.SetActive(false);
            zoomVarmi = true;
            anim.SetBool("zoomyap", true);
            camim.fieldOfView = yaklasmaPOV;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (atesEdebilirmi && Time.time > atesEtmeSikligi && kalanMermi != 0)
                {
                    AtesEt(true);
                    atesEtmeSikligi = Time.time + atesEtmeSiklik;
                }
                if (kalanMermi == 0)
                {
                    mermiBitisSesi.Play();

                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Cross.SetActive(true);
            zoomVarmi = false;
            anim.SetBool("zoomyap", false);
            if (Input.GetKey(KeyCode.Mouse0))
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mermi"))
        {
            MermiKaydet(other.transform.gameObject.GetComponent<MermiKutusu>().Olusan_Silahin_Turu, other.transform.gameObject.GetComponent<MermiKutusu>().Olusan_Mermi_sayisi);
            mermiKutusuOlustur.NoktalariKaldir(other.transform.gameObject.GetComponent<MermiKutusu>().Noktasi);
            Destroy(other.transform.parent.gameObject);

        }
        if (other.gameObject.CompareTag("Can kutusu"))
        {
            Kontrol.GetComponent<oyunKontrolu>().SaglikAl();
            canKutusuOlustur.canKutusuVarmi = false;
            Destroy(other.transform.gameObject);
        }
        if (other.gameObject.CompareTag("bomba kutusu"))
        {
            Kontrol.GetComponent<oyunKontrolu>().BombaAl();
            bombaKutusuOlustur.bombaKutusuVarmi = false;
            Destroy(other.transform.gameObject);
        }
    }


    void sarjorDegistirmeAnim() {
        anim.Play("reloadAnim");
        if (kalanMermi < sarjorKapasitesi && toplamMermiSayisi != 0)
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

    void AtesEt(bool yakinlasmavarmi) {

        AtesEtmeTeknik›slemleri(yakinlasmavarmi);
            RaycastHit hit;
        if (Physics.Raycast(camim.transform.position , camim.transform.forward, out hit ,menzil)) {

            if (hit.transform.gameObject.CompareTag("Dusman"))
            {

                Instantiate(KanEfekti, hit.point, Quaternion.LookRotation(hit.normal));
                hit.transform.gameObject.GetComponent<dusman>().DarbeAl(DarbeGucu);
            }

            else if (hit.transform.gameObject.CompareTag("Devrilebilir")) {

               Rigidbody rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(-hit.normal * 50f);
                
                

            }

            else {
                Instantiate(mermi›zi, hit.point, Quaternion.LookRotation(hit.normal));
            }

        }

        


    }

    void BaslangicMermiDoldur() {
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

    void sarjorDegistirme(string tur) {

        switch (tur) {
            case "mermiVar":

                if (toplamMermiSayisi <= sarjorKapasitesi)
                {

                    int olusanToplamDeger = kalanMermi + toplamMermiSayisi;

                    if (olusanToplamDeger > sarjorKapasitesi)
                    {
                        kalanMermi = sarjorKapasitesi;
                        toplamMermiSayisi = olusanToplamDeger - sarjorKapasitesi;
                        PlayerPrefs.SetInt(silahinAdi + "_Mermi", toplamMermiSayisi);

                    }
                    else
                    {
                        kalanMermi += toplamMermiSayisi;
                        toplamMermiSayisi = 0;
                        PlayerPrefs.SetInt(silahinAdi + "_Mermi", 0);
                    }

                }
                else {
                    toplamMermiSayisi -= sarjorKapasitesi - kalanMermi;
                    kalanMermi = sarjorKapasitesi;
                    PlayerPrefs.SetInt(silahinAdi + "_Mermi", toplamMermiSayisi);
                
                
                }
                
                toplamMermi_Text.text = toplamMermiSayisi.ToString();
                kalanMermi_Text.text = kalanMermi.ToString();

                break;

            case "mermiYok":
                if (toplamMermiSayisi <= sarjorKapasitesi)
                {
                    kalanMermi = toplamMermiSayisi;
                    toplamMermiSayisi = 0;
                    PlayerPrefs.SetInt(silahinAdi + "_Mermi", toplamMermiSayisi);

                }
                else {
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



    void AtesEtmeTeknik›slemleri(bool yakinlasmavarmi) {
        if (kovan_ciksinmi) 
        {
            GameObject obje = Instantiate(kovan_objesi, kovan_cikis_noktasi.transform.position, kovan_cikis_noktasi.transform.rotation);
            Rigidbody rb1 = obje.GetComponent<Rigidbody>();
            rb1.AddRelativeForce(new Vector3(10, 1, 0) * 30);
        }

        
        StartCoroutine(CamTitreme(.1f,.2f));
        atesSesi.Play();
        atesEfekt.Play();
        if (!yakinlasmavarmi)
        {
            anim.Play("ateset");
        }
        else {
            anim.Play("zoomateset");
        
        }
        

        kalanMermi--;
        kalanMermi_Text.text = kalanMermi.ToString();



    }

    void MermiAl() { 
        RaycastHit hit;
        if (Physics.Raycast(camim.transform.position, camim.transform.forward, out hit, 3))
        {

            if (hit.transform.gameObject.CompareTag("Mermi")) {

                MermiKaydet(hit.transform.gameObject.GetComponent<MermiKutusu>().Olusan_Silahin_Turu, hit.transform.gameObject.GetComponent<MermiKutusu>().Olusan_Mermi_sayisi);
                mermiKutusuOlustur.NoktalariKaldir(hit.transform.gameObject.GetComponent<MermiKutusu>().Noktasi);
                Destroy(hit.transform.parent.gameObject);
            
            }
        }
    
    }

    void MermiKaydet(string silahturu , int mermisayisi) {
        
        mermiAlmaSesi.Play();


        switch (silahturu) {
            case "m4":
                toplamMermiSayisi += mermisayisi;
                PlayerPrefs.SetInt("m4_Mermi", toplamMermiSayisi);
                sarjorDegistirme("normalYaz");
                break;

            case "pistol":
                PlayerPrefs.SetInt("pistol_Mermi", PlayerPrefs.GetInt("pistol_Mermi") + mermisayisi);
                break;

           

        }
    
    }

    public void ScopeYaklastirma(bool durum)
    {
        camim.fieldOfView = camFieldPOV;
    }

    IEnumerator CamTitreme(float titremeSuresi  , float magnitude) {

        Vector3 orijinalPozisyon = camim.transform.localPosition;

        float gecenSure = 0.0f;
        while (gecenSure < titremeSuresi)
        {
            float x = Random.Range(-1f,.1f) * magnitude;
            camim.transform.localPosition = new Vector3(x, orijinalPozisyon.y, orijinalPozisyon.x);
            gecenSure += Time.deltaTime;
            yield return null;
        
        }

        camim.transform.localPosition = orijinalPozisyon;
    
    }




    }

        