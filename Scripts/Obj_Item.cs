using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Obj_Item : MonoBehaviour
{
    public string id_name;
    private int index_item;
    public SpriteRenderer sp_render;

    public void on_load(int index)
    {
        this.index_item = index;
        if (PlayerPrefs.GetInt("is_found_" + this.id_name + "_" + this.index_item, 0) == 1)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnMouseUp()
    {
        PlayerPrefs.SetInt("is_found_" + this.id_name + "_" + this.index_item, 1);
        GameObject.Find("Games").GetComponent<Games>().maps.show_found(this);
        Destroy(this.gameObject);
    }
}
