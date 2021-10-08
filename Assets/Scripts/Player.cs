using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float velocidade;
    public float forcaPulo;
    public GameObject textoCoin;
    public AudioClip pegarCoin;

    const int MAX_PULOS = 1;

    private int qtdPulos;
    public int qtdCoins;

    private bool isDead;


    // Start is called before the first frame update
    void Start()
    {
        //this.velocidade = 1;
        this.qtdPulos = MAX_PULOS;
        this.qtdCoins = 0;
        this.isDead = false;
        AtualizarHUD();
    }

    // Update is called once per frame
    void Update()
    {
        VerificaAndar();
        VerificaPular();
        VerificaAbaixar();
        VerificaMorte();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
        {
            this.qtdPulos = MAX_PULOS;
        }
        if (col.collider.CompareTag("Monster")) this.isDead = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Coin"))
        {
            Destroy(col.gameObject);
            this.qtdCoins++;
            AtualizarHUD();

            this.GetComponent<AudioSource>().PlayOneShot(pegarCoin);
        }
    }


    public void AtualizarHUD()
    {
        textoCoin.GetComponent<Text>().text = this.qtdCoins.ToString();
    }

    public void VerificaAndar()
    {
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 posicao = this.transform.position;
            posicao.x += velocidade;
            this.transform.position = posicao;
            
            this.GetComponent<Animator>().SetBool("isRunning", true);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Vector3 posicao = this.transform.position;
            posicao.x -= velocidade;
            this.transform.position = posicao;
            this.GetComponent<Animator>().SetBool("isRunning", true);
        }
        else
        {
            this.GetComponent<Animator>().SetBool("isRunning", false);
        }
    }

    public void VerificaPular()
    {
        if (Input.GetKey(KeyCode.Space) && this.qtdPulos > 0)
        {
            this.GetComponent<Animator>().SetBool("isJumping", true);
            this.qtdPulos--;
            Vector2 forca = new Vector2(0f, this.forcaPulo);
            this.GetComponent<Rigidbody2D>().AddForce(forca, ForceMode2D.Impulse);
        }
        else
        {
            this.GetComponent<Animator>().SetBool("isJumping", false);
        }
    }
    public void VerificaAbaixar()
    {
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 posicao = this.transform.position;
            posicao.y -= velocidade;
            this.transform.position = posicao;
            this.GetComponent<Animator>().SetBool("isDown", true);
        }
        else
        {
            this.GetComponent<Animator>().SetBool("isDown", false);
        }
    }

    public void VerificaMorte()
    {
        if (this.isDead == true)
        {
            SceneManager.LoadScene("Menu");
            this.isDead = false;
        }
    }

}
