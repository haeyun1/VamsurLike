using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;
    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
        {
            return;
        }

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        // float diffX = Mathf.Abs(playerPos.x - myPos.x); // 절댓값
        // float diffY = Mathf.Abs(playerPos.y - myPos.y);
        // Vector3 playerDir = GameManager.instance.player.inputVec;
        // float dirX = playerDir.x < 0 ? -1 : 1;
        // float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                float diffX = playerPos.x - myPos.x; // 절댓값
                float diffY = playerPos.y - myPos.y;
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;

                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);

                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 80);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 80);
                }
                break;
            case "Enemy":
                if (coll.enabled)
                {
                    Vector3 dist = playerPos - myPos;
                    Vector3 rand = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(rand + dist * 2);
                }
                break;
        }
    }
}
