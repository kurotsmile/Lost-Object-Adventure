using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map_Manager : MonoBehaviour
{
    [Header("Obj App")]
    public Maps map;
    public GameObject tray_item_prefab;
    public GameObject quest_item_prefab;
    public Sprite sp_icon_items;

    [Header("Asset Objs")]
    public Obj_Item[] obj_found;

    [Header("UI")]
    public Text txt_count_obj_found;
    public Text txt_count_obj_found_home;
    public Text txt_count_specify_object;
    public Text txt_count_highlight_all_object;
    public Text txt_count_coin;
    public Text txt_count_coin_found;
    public Text txt_coin_get_rewarded_npc;
    public Slider slider_total_obj_found;
    public Transform tr_all_body_tray;
    public Transform tr_all_npc;
    public Transform tr_all_ground;
    public Transform tr_all_item_find;
    public Transform tr_all_item_quest;
    public ScrollRect scroll_rect_tray;
    public GameObject obj_coin_found_panel;
    public GameObject obj_btn_specify;
    public GameObject obj_btn_highlight;

    [Header("Panel Found")]
    public GameObject panel_found;
    public Image img_obj_found;

    [Header("Panel Chat")]
    public GameObject panel_npc_chat;
    public Image img_avatar_npc_chat;
    public Image img_item_npc_chat;
    public Text txt_ui_chat_npc;
    public Text txt_name_chat_npc;
    public string[] s_tip_quest_npc;

    [Header("Effect")]
    public GameObject[] effect_prefab;

    private int count_obj_found;
    private int count_specify;
    private int count_highlight;
    private int max_obj_found;
    private int coin;
    private bool is_play = false;
    private Tray_Item item_quest_show;
    private int coin_get_rewarded;
    private float count_txt_show_chat = 0;
    private string s_msg_chat_npc;
    private float timer_effect_txt = 0;
    private bool is_effect_txt = false;

    public void on_load()
    {
        this.GetComponent<Games>().carrot.clear_contain(this.tr_all_item_quest);
        this.panel_found.SetActive(false);
        this.panel_npc_chat.SetActive(false);
        this.load_data_item_found();

        this.coin = PlayerPrefs.GetInt("coin", 0);
        this.count_obj_found = PlayerPrefs.GetInt("count_obj_found",0);
        this.count_specify = PlayerPrefs.GetInt("count_specify", 2);
        this.count_highlight = PlayerPrefs.GetInt("count_highlight",1);

        this.map.set_active(false);
        this.update_ui_emp();
    }

    private void Update()
    {
        if (this.panel_npc_chat.activeInHierarchy&&this.is_effect_txt)
        {
            this.timer_effect_txt += (3 * Time.deltaTime);
            if (this.timer_effect_txt > 0.1f)
            {
                this.count_txt_show_chat++;
                if (this.count_txt_show_chat < this.s_msg_chat_npc.Length)
                {
                    this.txt_ui_chat_npc.text = this.s_msg_chat_npc.Substring(0, (int)this.count_txt_show_chat)+"_";
                }
                else
                {
                    this.is_effect_txt = false;
                    this.txt_ui_chat_npc.text = this.s_msg_chat_npc;
                }
                this.timer_effect_txt=0;
            }
        }
    }

    public void on_play()
    {
        this.is_play = true;
        this.map.set_active(true);
    }

    private void on_stop()
    {
        this.is_play = false;
        this.map.set_active(false);
    }

    public void on_pause()
    {
        if (this.is_play)
        {
            this.map.set_active(false);
            this.set_active_all_npc(false);
        }
    }

    public void un_pause()
    {
        if (this.is_play)
        {
            this.map.set_active(true);
            this.set_active_all_npc(true);
        }
    }

    public void back_home()
    {
        this.on_stop();
    }

    public void show_found(Obj_Item item_found)
    {
        this.obj_coin_found_panel.SetActive(false);
        this.GetComponent<Games>().carrot.ads.show_ads_Interstitial();
        this.create_effect(1, item_found.transform.position);

        foreach(Transform tr_quest in this.tr_all_item_quest)
        {
            Quest_Item i_quest = tr_quest.GetComponent<Quest_Item>();
            if (i_quest.s_id == item_found.id_name)
            {
                this.txt_count_coin_found.text = "+"+i_quest.coin_rewarded.ToString();
                this.obj_coin_found_panel.SetActive(true);
                i_quest.on_done();
                this.add_coin(i_quest.coin_rewarded);
                break;
            }
        }

        foreach(Transform tr in this.tr_all_body_tray)
        {
            Tray_Item t = tr.GetComponent<Tray_Item>();
            if (t.txt_name.text == item_found.id_name)
            {
                t.add_item();
                tr.SetSiblingIndex(0);
            }
        }

        this.img_obj_found.sprite = item_found.sp_render.sprite;
        this.GetComponent<Games>().play_sound(1);
        this.GetComponent<Games>().carrot.play_vibrate();
        this.panel_found.SetActive(true);
        this.count_obj_found++;
        PlayerPrefs.SetInt("count_obj_found",this.count_obj_found);
        this.txt_count_obj_found.text = count_obj_found.ToString();
        this.slider_total_obj_found.value = count_obj_found;
        this.scroll_rect_tray.horizontalNormalizedPosition = -1f;
        this.update_ui_emp();
    }

    public void load_data_item_found()
    {
        this.max_obj_found = 0;
        this.GetComponent<Games>().carrot.clear_contain(this.tr_all_body_tray);
        for(int i = 0; i < this.obj_found.Length; i++)
        {
            GameObject obj_tray = Instantiate(this.tray_item_prefab);
            obj_tray.transform.SetParent(this.tr_all_body_tray);
            obj_tray.transform.localPosition = Vector3.zero;
            obj_tray.transform.localScale = new Vector3(1f, 1f, 1f);

            Tray_Item tray = obj_tray.GetComponent<Tray_Item>();
            tray.img_icon.sprite = this.obj_found[i].sp_render.sprite;
            tray.on_load(this.obj_found[i].id_name);
            this.max_obj_found += 10;
        }

        int index_item = 0;
        foreach(Transform tr in this.tr_all_item_find)
        {
            tr.GetComponent<Obj_Item>().on_load(index_item);
            index_item++;
        }

        this.slider_total_obj_found.maxValue = this.max_obj_found;
    }

    public void set_active_all_npc(bool is_act)
    {
        foreach(Transform tr in this.tr_all_npc)
        {
            tr.gameObject.GetComponent<Npc>().set_active(is_act);
        }
    }

    private void update_ui_emp()
    {
        this.txt_count_obj_found.text = this.count_obj_found.ToString();
        this.txt_count_obj_found_home.text = this.count_obj_found.ToString();
        this.txt_count_specify_object.text = this.count_specify.ToString();
        this.txt_count_highlight_all_object.text = this.count_highlight.ToString();
        this.txt_count_coin.text = this.coin.ToString();
        this.slider_total_obj_found.value = this.count_obj_found;
        if (this.count_specify > 0)
            this.obj_btn_specify.SetActive(true);
        else
            this.obj_btn_specify.SetActive(false);

        if (this.count_highlight > 0)
            this.obj_btn_highlight.SetActive(true);
        else
            this.obj_btn_highlight.SetActive(false);
    }

    public int get_count_obj_found()
    {
        return this.count_obj_found;
    }

    public void show_list_items()
    {
        this.on_pause();
        Carrot.Carrot_Box box_items = this.GetComponent<Games>().carrot.Create_Box("box_items");
        box_items.set_icon(this.sp_icon_items);
        box_items.set_title("Items you need to find");

        foreach (Transform tr in this.tr_all_body_tray)
        {
            Tray_Item tray = tr.GetComponent<Tray_Item>();
            Carrot.Carrot_Box_Item i_item=box_items.create_item("item_box_" + tray.txt_name.text);
            i_item.set_icon_white(tray.img_icon.sprite);
            i_item.set_title(tray.txt_name.text);
            i_item.set_tip(tray.get_count_item()+"/"+tray.get_max_item());
        }

        box_items.set_act_before_closing(this.act_close_box_items);
    }

    private void act_close_box_items()
    {
        this.un_pause();
    }

    public void specify_object()
    {
        int rand_index = Random.Range(0, this.tr_all_item_find.childCount);
        Transform tr_find = this.tr_all_item_find.GetChild(rand_index);

        this.map.mainCamera.transform.position = new Vector3(tr_find.position.x, tr_find.position.y, this.map.mainCamera.transform.position.z);
        this.create_effect(0, new Vector3(tr_find.position.x, tr_find.position.y, tr_find.position.z + 0.05f), 2.5f);
        this.count_specify--;
        PlayerPrefs.SetInt("count_specify", this.count_specify);
        this.update_ui_emp();
    }

    public void highlight_all_object()
    {
        foreach (Transform tr in this.tr_all_item_find)
        {
            this.create_effect(2, new Vector3(tr.position.x, tr.position.y, tr.position.z + 0.05f),2f);
        }

        this.count_highlight--;
        PlayerPrefs.SetInt("count_highlight",this.count_highlight);
        this.update_ui_emp();
    }

    public void add_specify_object()
    {
        this.count_specify++;
        PlayerPrefs.SetInt("count_specify", this.count_specify);
        this.update_ui_emp();
    }

    public void add_highlight_object()
    {
        this.count_highlight++;
        PlayerPrefs.SetInt("count_highlight", this.count_highlight);
        this.update_ui_emp();
    }

    public void create_effect(int index,Vector3 pos,float timer_destroy=2f)
    {
        GameObject obj_effect = Instantiate(this.effect_prefab[index]);
        obj_effect.transform.SetParent(this.transform);
        obj_effect.transform.position = pos;
        obj_effect.transform.localScale = new Vector3(1f, 1f, 1f);
        Destroy(obj_effect, timer_destroy);
    }

    public void show_chat_npc(Npc npc)
    {
        this.is_effect_txt = true;
        this.count_txt_show_chat = 0;
        this.s_msg_chat_npc= this.get_chat_msg_npc_ui();
        this.coin_get_rewarded = Random.Range(3, 10);
        this.item_quest_show = this.get_item_quest_random();
        this.img_avatar_npc_chat.sprite = npc.sp_avatar[0];
        this.txt_name_chat_npc.text = npc.id_name;
        this.txt_ui_chat_npc.text = "";
        this.txt_coin_get_rewarded_npc.text = "+"+this.coin_get_rewarded.ToString()+" Coin";
        this.img_item_npc_chat.sprite = this.item_quest_show.img_icon.sprite;
        this.on_pause();
        this.GetComponent<Games>().carrot.play_sound_click();
        this.panel_npc_chat.SetActive(true);
    }

    private string get_chat_msg_npc_ui()
    {
        int index_rand = Random.Range(0, this.s_tip_quest_npc.Length);
        return this.s_tip_quest_npc[index_rand];
    }

    public void btn_yes_chat_npc()
    {
        this.add_Quest_Item(this.item_quest_show, this.coin_get_rewarded);
        this.un_pause();
        this.GetComponent<Games>().play_sound(3);
        this.panel_npc_chat.SetActive(false);
    }

    public void btn_no_chat_npc()
    {
        this.un_pause();
        this.GetComponent<Games>().carrot.play_sound_click();
        this.panel_npc_chat.SetActive(false);
    }

    private Tray_Item get_item_quest_random()
    {
        List<Tray_Item> list_item_tray = new List<Tray_Item>();

        foreach(Transform tr in this.tr_all_body_tray)
        {
            Tray_Item tray_check = tr.GetComponent<Tray_Item>();
            if (!tray_check.is_quest) list_item_tray.Add(tray_check);
        }

        int index_rand = Random.Range(0, list_item_tray.Count);
        return list_item_tray[index_rand];
    }

    public void add_Quest_Item(Tray_Item item_show,int coin_get_rewarded)
    {
        item_show.is_quest = true;
        GameObject obj_quest = Instantiate(this.quest_item_prefab);
        obj_quest.transform.SetParent(this.tr_all_item_quest);
        obj_quest.transform.position = Vector3.zero;
        obj_quest.transform.localScale = new Vector3(1f, 1f, 1f);

        Quest_Item q = obj_quest.GetComponent<Quest_Item>();
        q.on_load(item_show.txt_name.text,item_show, coin_get_rewarded);
        q.img_icon.sprite = item_show.img_icon.sprite;
    }

    public void add_coin(int coin_add)
    {
        this.GetComponent<Games>().play_sound(5);
        this.txt_count_coin_found.text = coin_add.ToString();
        this.coin += coin_add;
        PlayerPrefs.SetInt("coin", this.coin);
        this.update_ui_emp();
    }

    public void minus_coin(int coin_minus)
    {
        this.coin -= coin_minus;
        PlayerPrefs.SetInt("coin", this.coin);
        this.update_ui_emp();
    }

    public int get_coin()
    {
        return this.coin;
    }
}
