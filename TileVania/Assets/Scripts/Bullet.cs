using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D myRigidbody2D;
    [SerializeField] float bulletSpeed = 10f;
    PlayerMovement player;
    float xSpeed;
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        //khi ban' dan. chung' ta can` biet player quay ve` phia' ben nao` sau do' chung' ta can` biet' toc' do. cua? no' la` bao nhieu
        //o? day chung ta se~ nhan voi toc' do. cua? dan de biet duoc toc do. dan. se~ bay ra la` bao nhieu
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody2D.velocity = new Vector2(xSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
