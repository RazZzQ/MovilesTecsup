using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class CreatorObject : MonoBehaviour
{
    [SerializeField] SelectorManager selectorManager;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Collider2D squareCollider;
    [SerializeField] LayerMask layer;
    private float tapCount;
    private bool canClick;
    private GameObject newObject;
    Vector3 TouchPosition;
    RaycastHit hit;

    public void AsignateObject(Vector3 victim)
    {
        if(selectorManager != null)
        {
            spriteRenderer.sprite = selectorManager.character.gameObject.GetComponent<SpriteRenderer>().sprite;
            spriteRenderer.color = selectorManager.color;
            newObject = Instantiate(spriteRenderer.gameObject, victim, Quaternion.identity);
            newObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    public void createObject()
    {
        if (Input.touchCount > 0)
        {
            Touch MiTouch = Input.GetTouch(0);
            TouchPosition = Camera.main.ScreenToWorldPoint(new Vector3(MiTouch.position.x, MiTouch.position.y, 10));
            if (canClick)
            {
                if (IsMouseOverSquare(TouchPosition))
                {
                    AsignateObject(TouchPosition);
                }
                canClick = false;
            }
        }
        else
        {
            canClick = true;
        }
    }
    public void DeleteObject()
    {
        
            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    tapCount++;
                }
                if(Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    if (tapCount == 2)
                    {
                        Debug.Log("adwas");

                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layer);
                        Debug.Assert((hit.collider == null));
                        // Dibuja el rayo para depuración (solo se ve en el editor).
                        Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Color.green, 10f);

                        if (hit.collider != null)
                        {
                            if (hit.collider.gameObject.CompareTag("Victim"))
                            {
                                Destroy(newObject);
                                Debug.Log("wasaa");
                            }
                        }
                    tapCount = 0;
                    }
                }
            }
        
    }
    bool IsMouseOverSquare(Vector2 position)
    {
        return squareCollider.OverlapPoint(position);
    }
    void Awake()
    {
        loadComponent();
    }
    public void loadComponent()
    {
        canClick = true;
        //spriteRenderer = selectorManager.character.gameObject.GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        createObject();
        DeleteObject();
    }
}
