using UnityEngine;

public class Maps : MonoBehaviour
{
    private bool isDragging;
    private bool isActive;
    private Vector2 offset;

    [Header("UI")]
    public GameObject obt_btn_zoom_out;
    public GameObject obt_btn_zoom_in;

    public float zoomSpeed = 1.0f;
    public float moveSpeed = 1.0f;

    public Camera mainCamera;

    void Update()
    {
        if (this.isActive)
        {

                float zoomDelta = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
                mainCamera.orthographicSize += zoomDelta;
                mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 1f, 10f);
        }
    }

    private void OnMouseUp()
    {
        if (this.isActive)
        {
            this.isDragging = false;
            GameObject.Find("Games").GetComponent<Games>().maps.set_active_all_npc(true);
        }
    }

    private void OnMouseDown()
    {
        if (this.isActive)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                offset = hit.transform.position - (Vector3)hit.point;
                this.isDragging = true;
            }
            GameObject.Find("Games").GetComponent<Games>().maps.set_active_all_npc(false);
        }

    }

    private void OnMouseDrag()
    {
        if (this.isDragging&&this.isActive)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + (Vector3)offset;
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }
    }

    public void set_active(bool is_act)
    {
        this.isActive = is_act;
    }

    public void on_zoom_out()
    {
        this.mainCamera.orthographicSize += 1f;
        this.update_ui_emp();
    }

    public void on_zoom_in()
    {
        this.mainCamera.orthographicSize -= 1f;
        this.update_ui_emp();
    }

    public void on_reset()
    {
        this.mainCamera.orthographicSize = 2.50f;
        this.transform.localPosition = Vector3.zero;
        this.update_ui_emp();
    }

    private void update_ui_emp()
    {
        if (this.mainCamera.orthographicSize > 10f)
        {
            this.mainCamera.orthographicSize = 10f;
            this.obt_btn_zoom_out.SetActive(false);
        }
        else
        {
            this.obt_btn_zoom_out.SetActive(true);
        }

        if (this.mainCamera.orthographicSize < 1f)
        {
            this.mainCamera.orthographicSize = 1f;
            this.obt_btn_zoom_in.SetActive(false);
        }
        else
        {
            this.obt_btn_zoom_in.SetActive(true);
        }

        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 1f, 10f);
    }
}
