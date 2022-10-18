using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip audioCoin;
    [SerializeField] int pointsForCoinPickup = 100;

    bool wasCollected = false;//kiem? tra tien` da~ dc nhat. hay chua
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //kiem? tra co' phai? la` nguoi choi hay ko va` coin nay` chua dc nhat.
        if(other.tag == "Player" && !wasCollected) 
        {
            //da~ dc nhat.
            wasCollected = true;
            FindObjectOfType<GameSession>().AddScore(pointsForCoinPickup);
            AudioSource.PlayClipAtPoint(audioCoin,Camera.main.transform.position);
            gameObject.SetActive(false);
            Destroy(gameObject);
            
        }
    }
}
