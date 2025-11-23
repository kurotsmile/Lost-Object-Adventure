using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameObject.Find("Games").GetComponent<Games>().maps.add_coin(1);
        Destroy(this.gameObject);
    }
}
