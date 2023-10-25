using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (!collision.GetComponentInParent<Enemy>().isEntered) { return; }
            GameManager.Instance.Player.GetComponent<PlayerController>().
                PainController(collision.GetComponentInParent<Enemy>().ATK / 2);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.layer == 8)
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 7)
        {
            Destroy(collision.gameObject);
        } 
    }
}
