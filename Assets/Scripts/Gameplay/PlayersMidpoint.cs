using UnityEngine;

public class PlayersMidpoint : MonoBehaviour
{


    void LateUpdate()
    {
        GameObject player1 = GameplayManager.Instance.Players[0];
        GameObject player2 = GameplayManager.Instance.Players[1];

        if (player1 == null || player2 == null) return;

        Vector3 midpoint = (player1.transform.position + player2.transform.position) / 2;
        transform.position = new Vector3(midpoint.x, midpoint.y, transform.position.z);
    }
}