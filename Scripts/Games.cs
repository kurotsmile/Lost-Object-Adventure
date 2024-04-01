using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Games : MonoBehaviour
{
    public Carrot.Carrot carrot;
    public Map_Manager maps;
    public AudioSource[] sound;

    [Header("Ui")]
    public GameObject panel_home;
    public GameObject panel_play;

    [Header("Shop")]
    public Sprite sp_icon_shop;
    public Sprite sp_icon_shop_coin;
    public Sprite sp_icon_shop_buy;
    public Sprite sp_icon_shop_ads;
    public Sprite sp_icon_shop_search;
    public Sprite sp_icon_shop_highlight;

    private Carrot.Carrot_Box box_shop;
    private Carrot.Carrot_Window_Msg msg_shop;
    private string s_id_item_shop;
    private string s_id_item_shop_adsRewarded;
    private int coin_buy_item_shop;
    void Start()
    {
        this.carrot.Load_Carrot(this.check_exit_app);
        this.carrot.shop.onCarrotPaySuccess += this.act_buy_success;
        this.carrot.ads.onRewardedSuccess += this.onRewardedSuccess;
        this.carrot.game.load_bk_music(this.sound[0]);
        this.maps.on_load();

        this.panel_home.SetActive(true);
        this.panel_play.SetActive(false);
    }

    private void check_exit_app()
    {
        if (this.maps.panel_npc_chat.activeInHierarchy)
        {
            this.maps.btn_no_chat_npc();
            this.carrot.set_no_check_exit_app();
        }
        else if (this.panel_play.activeInHierarchy)
        {
            this.btn_back_home();
            this.carrot.set_no_check_exit_app();
        }
    }

    public void play_sound(int index_sound)
    {
        if (this.carrot.get_status_sound()) this.sound[index_sound].Play();
    }

    public void btn_close_box_found()
    {
        this.carrot.play_sound_click();
        this.maps.panel_found.SetActive(false);
    }

    public void btn_play()
    {
        this.carrot.play_sound_click();
        this.maps.on_play();
        this.panel_play.SetActive(true);
        this.panel_home.SetActive(false);
    }

    public void btn_back_home()
    {
        this.carrot.play_sound_click();
        this.maps.back_home();
        this.panel_home.SetActive(true);
        this.panel_play.SetActive(false);
        this.carrot.game.update_scores_player(this.maps.get_count_obj_found());
    }

    public void btn_setting()
    {
        this.carrot.ads.Destroy_Banner_Ad();
        this.maps.on_pause();
        Carrot.Carrot_Box box_setting=this.carrot.Create_Setting();
        box_setting.set_act_before_closing(act_close_setting);
    }

    private void act_close_setting()
    {
        this.carrot.ads.create_banner_ads();
        this.maps.un_pause();
    }

    public void btn_rank()
    {
        this.carrot.show_rate();
    }

    public void btn_user()
    {
        this.carrot.user.show_login();
    }

    public void btn_rate()
    {
        this.carrot.show_rate();
    }

    public void btn_share()
    {
        this.carrot.show_share();
    }

    public void btn_app_other()
    {
        this.carrot.show_list_carrot_app();
    }

    public void btn_ranks()
    {
        this.carrot.game.Show_List_Top_player();
    }

    public void btn_shop()
    {
        this.carrot.ads.Destroy_Banner_Ad();
        this.maps.on_pause();
        box_shop=this.carrot.Create_Box("Shop");
        box_shop.set_icon(this.sp_icon_shop);

        Carrot.Carrot_Box_Item item_shop_search=box_shop.create_item("item_search");
        item_shop_search.set_icon(this.sp_icon_shop_search);
        item_shop_search.set_title("Magnifying Glass");
        item_shop_search.set_tip("Specify an object you are looking for");
        item_shop_search.set_act(()=>this.sel_shop_item("item_search"));
        this.create_btn_other_item_shop(item_shop_search);

        Carrot.Carrot_Box_Item item_shop_highlight = box_shop.create_item("item_highlight");
        item_shop_highlight.set_icon(this.sp_icon_shop_highlight);
        item_shop_highlight.set_title("Highlight objects");
        item_shop_highlight.set_tip("Highlight for a few seconds the objects you are looking for");
        item_shop_highlight.set_act(() => this.sel_shop_item("item_highlight"));
        this.create_btn_other_item_shop(item_shop_highlight);

        Carrot.Carrot_Box_Item item_shop_coin = box_shop.create_item("item_coin");
        item_shop_coin.set_icon_white(this.sp_icon_shop_coin);
        item_shop_coin.set_title("Buy more coins");
        item_shop_coin.set_tip("Increase the amount of gold coins to buy items +100 (" + this.maps.get_coin() + " Coin)");
        item_shop_coin.set_act(() => this.sel_shop_item("item_coin"));
        this.create_btn_other_item_shop(item_shop_coin);

        box_shop.set_act_before_closing(act_close_shop);
    }

    private void sel_shop_item(string id_item)
    {
        this.s_id_item_shop = id_item;
        this.check_price_item_shop();

        this.msg_shop=this.carrot.show_msg("Shop", "You can buy this item with gold coins, real money or watch ads to receive rewards");
        if (id_item != "item_coin") this.msg_shop.add_btn_msg(this.coin_buy_item_shop+" Coin",()=>this.act_coin_item_shop(id_item));
        this.msg_shop.add_btn_msg("Buy",()=>this.act_buy_item_shop(id_item));
        this.msg_shop.add_btn_msg("Watch ads", ()=>this.act_ads_item_shop(id_item));
    }

    private void check_price_item_shop()
    {
        if (this.s_id_item_shop == "item_search") this.coin_buy_item_shop = 50;
        if (this.s_id_item_shop == "item_highlight") this.coin_buy_item_shop = 90;
    }

    private void create_btn_other_item_shop(Carrot.Carrot_Box_Item item)
    {
        var s_id_item = item.name;

        if (item.name != "item_coin")
        {
            Carrot.Carrot_Box_Btn_Item btn_coin = item.create_item();
            btn_coin.set_icon(this.sp_icon_shop_coin);
            btn_coin.set_color(this.carrot.color_highlight);
            btn_coin.set_act(() => this.act_coin_item_shop(s_id_item));
        }

        Carrot.Carrot_Box_Btn_Item btn_buy = item.create_item();
        btn_buy.set_icon(this.sp_icon_shop_buy);
        btn_buy.set_color(this.carrot.color_highlight);
        btn_buy.set_act(() => this.act_buy_item_shop(s_id_item));

        Carrot.Carrot_Box_Btn_Item btn_ads=item.create_item();
        btn_ads.set_icon(this.sp_icon_shop_ads);
        btn_ads.set_color(this.carrot.color_highlight);
        btn_ads.set_act(() => this.act_ads_item_shop(s_id_item));
    }

    private void act_coin_item_shop(string id_item)
    {
        if (this.msg_shop != null) this.msg_shop.close();
        this.s_id_item_shop = id_item;
        this.check_price_item_shop();
        if (this.maps.get_coin() >= this.coin_buy_item_shop)
        {
            this.select_item_shop(id_item);
            this.maps.minus_coin(this.coin_buy_item_shop);
        }
        else
            this.msg_shop = this.carrot.show_msg("Shop", "You don't have enough gold coins to use this item, Collect more gold coins by searching or doing quests from game characters",Carrot.Msg_Icon.Alert);
    }

    private void act_buy_item_shop(string id_item)
    {
        this.s_id_item_shop = id_item;
        this.carrot.buy_product(2);
    }

    private void act_ads_item_shop(string id_item)
    {
        this.s_id_item_shop_adsRewarded = id_item;
        this.carrot.ads.show_ads_Rewarded();
    }

    private void act_close_shop()
    {
        this.carrot.ads.create_banner_ads();
        this.maps.un_pause();
    }

    private void select_item_shop(string s_id_name)
    {
        if (this.msg_shop != null) this.msg_shop.close();
        if (s_id_name == "item_coin") this.maps.add_coin(100);
        if (s_id_name == "item_search") this.maps.add_specify_object();
        if (s_id_name == "item_highlight") this.maps.add_highlight_object();

        if (this.box_shop != null) this.box_shop.close();
    }

    public void btn_zoom_in()
    {
        this.carrot.play_sound_click();
        this.maps.map.on_zoom_in();
    }

    public void btn_zoom_out()
    {
        this.carrot.play_sound_click();
        this.maps.map.on_zoom_out();
    }

    public void btn_items()
    {
        this.carrot.play_sound_click();
        this.maps.show_list_items();
    }

    public void btn_specify_object()
    {
        this.play_sound(2);
        this.maps.specify_object();
    }

    public void btn_highlight_all_object()
    {
        this.play_sound(2);
        this.maps.highlight_all_object();
    }

    private void act_buy_success(string s_id)
    {
        if (s_id == this.carrot.shop.get_id_by_index(2))
        {
            this.select_item_shop(this.s_id_item_shop);
        }
    }

    private void onRewardedSuccess()
    {
        if (this.s_id_item_shop_adsRewarded != "")
        {
            this.select_item_shop(this.s_id_item_shop_adsRewarded);
            this.s_id_item_shop_adsRewarded = "";
        }
    }

}
