using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Npc_Direction {up,down,left,right,none}
public class Npc : MonoBehaviour
{
    public string id_name;
    public GameObject foot_effect_prefab;
    public GameObject coin_items_prefab;
    public Animator ani;
    public Sprite[] sp_avatar;

    private float speed = 1f;
    private Npc_Direction direction;
    private float timer_change_direction = 0;
    private float timer_change_direction_max = 3f;
    private float timer_create_foot_effect = 0;
    private float timer_create_coin = 0;
    private bool isActive = true;

    void Start()
    {
        this.change_direction_random();
    }

    void Update()
    {
        if (this.isActive)
        {
            this.timer_change_direction += Time.deltaTime;

            if (this.timer_change_direction > this.timer_change_direction_max)
            {
                this.change_direction_random();
                this.timer_change_direction = 0;
            }

            if (this.direction != Npc_Direction.none)
            {
                this.timer_create_foot_effect += (1 * Time.deltaTime);

                if (this.timer_create_foot_effect > 0.2f)
                {
                    this.create_foot_effect();
                    this.timer_create_foot_effect = 0;
                }

                this.timer_create_coin += (1 * Time.deltaTime);
                if (this.timer_create_coin > 5f)
                {
                    this.create_coin();
                    this.timer_create_coin = 0;
                }
            }

            if (this.direction == Npc_Direction.left)
                this.transform.Translate(Vector3.left * (this.speed * Time.deltaTime));
            else if (this.direction == Npc_Direction.right)
                this.transform.Translate(Vector3.right * (this.speed * Time.deltaTime));
            else if (this.direction == Npc_Direction.up)
                this.transform.Translate(Vector3.up * (this.speed * Time.deltaTime));
            else if (this.direction == Npc_Direction.down)
                this.transform.Translate(Vector3.down * (this.speed * Time.deltaTime));
        }
    }

    private void change_direction_random()
    {
        Npc_Direction[] arr_d = new Npc_Direction[5];
        arr_d[0] = Npc_Direction.up;
        arr_d[1] = Npc_Direction.down;
        arr_d[2] = Npc_Direction.left;
        arr_d[3] = Npc_Direction.right;
        arr_d[4] = Npc_Direction.none;

        int index_rand = Random.Range(0, arr_d.Length);
        this.direction = arr_d[index_rand];
        this.ani.Play(this.id_name + "_" + this.direction.ToString());
        this.change_attr_other();
    }

    private void change_attr_other()
    {
        this.speed = Random.Range(1f, 1.5f);
        this.timer_change_direction_max = Random.Range(1f, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name== "Tilemap_Rock_Mountain")
        {
            this.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), this.transform.position.z);
        }
        else
        {
            if (this.direction == Npc_Direction.up)
                this.direction = Npc_Direction.down;
            else if (this.direction == Npc_Direction.down)
                this.direction = Npc_Direction.up;
            else if (this.direction == Npc_Direction.left)
                this.direction = Npc_Direction.right;
            else if (this.direction == Npc_Direction.right)
                this.direction = Npc_Direction.left;

            this.change_attr_other();
            this.ani.Play(this.id_name + "_" + this.direction.ToString());
        }
    }

    public void set_active(bool is_act)
    {
        this.isActive = is_act;
    }

    private void create_foot_effect()
    {
        GameObject obj_foot = Instantiate(this.foot_effect_prefab);
        obj_foot.transform.SetParent(GameObject.Find("Games").GetComponent<Games>().maps.tr_all_ground);
        obj_foot.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -0.2f);
        obj_foot.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        if (this.direction == Npc_Direction.up)
            obj_foot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        else if (this.direction == Npc_Direction.left)
            obj_foot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        else if (this.direction == Npc_Direction.right)
            obj_foot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90f));
        else
            obj_foot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        Destroy(obj_foot, 4f);
    }

    private void create_coin()
    {
        if (Random.Range(0, 6) == 1)
        {
            GameObject obj_coin = Instantiate(this.coin_items_prefab);
            obj_coin.transform.SetParent(GameObject.Find("Games").GetComponent<Games>().maps.tr_all_ground);
            obj_coin.transform.position = this.transform.position;
            obj_coin.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            Destroy(obj_coin, Random.Range(4f, 8f));
        }
    }

    private void OnMouseDown()
    {
        if(this.isActive) GameObject.Find("Games").GetComponent<Games>().maps.show_chat_npc(this);
    }
}
