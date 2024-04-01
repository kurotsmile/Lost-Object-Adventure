using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest_Item : MonoBehaviour
{
    public string s_id;
    public Image img_icon;
    public Slider slider_timer;
    public int coin_rewarded;

    private float timer_cout = 100;
    private Tray_Item tray_father = null;

    public void on_load(string s_id_name,Tray_Item tray_item,int coin)
    {
        this.s_id = s_id_name;
        this.tray_father = tray_item;
        this.coin_rewarded = coin;
    }

    void Update()
    {
        this.timer_cout -= Time.deltaTime;
        this.slider_timer.value = timer_cout;
        if (this.timer_cout <= 0)
        {
            if (this.tray_father != null) this.tray_father.is_quest = false;
            Destroy(this.gameObject);
            this.timer_cout = 0;
        }
    }

    public void on_done()
    {
        if(this.tray_father!=null)this.tray_father.is_quest = false;
        Destroy(this.gameObject);
    }
}
