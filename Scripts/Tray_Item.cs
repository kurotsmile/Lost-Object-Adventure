using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tray_Item : MonoBehaviour
{
    public Image img_icon;
    public Text txt_name;
    public Text txt_count;
    public Slider slider_count;
    public bool is_quest = false;

    private string id_name;
    private int count_item=0;
    private int max_item=10;

    public void on_load(string s_name)
    {
        this.id_name = s_name.Replace(" ","");
        this.txt_name.text = s_name;
        this.count_item = PlayerPrefs.GetInt("item_"+this.id_name,0);
        this.slider_count.value = this.count_item;
        this.slider_count.maxValue = max_item;
        this.txt_count.text = this.count_item.ToString()+"/"+this.max_item.ToString();

        if (this.count_item >= this.max_item)
        {
            Destroy(this.gameObject);
        }
    }

    public void add_item()
    {
        this.count_item++;
        this.slider_count.value = this.count_item;
        this.txt_count.text = this.count_item.ToString() + "/" + this.max_item.ToString();
        if (this.count_item >= this.max_item)
        {
            Destroy(this.gameObject);
        }
        PlayerPrefs.SetInt("item_" + this.id_name, this.count_item);
    }

    public int get_count_item()
    {
        return this.count_item;
    }

    public int get_max_item()
    {
        return this.max_item;
    }
}
