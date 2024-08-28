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
    public int tapCount;
    private bool canClick;
    private GameObject newObject;
    Vector3 TouchPosition;
    private bool isDragging = false;
    private GameObject selectedObject = null;
    private Vector2 startSwipePosition;
    private Vector3 touchPosition;
    private TrailRenderer trail;
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
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            // Verificar si se ha tocado el Collider2D del objeto
            if (selectorManager.character == null)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    if (squareCollider.OverlapPoint(touchPosition))
                    {
                        tapCount++;
                    }
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    if (tapCount == 2)
                    {
                        // Aquí se eliminaría el objeto
                        Debug.Log("Objeto eliminado");
                        tapCount = 0;
                    }
                }
            }
        }
    }
    public void DetectDragAndSwipe()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                if (squareCollider.OverlapPoint(touchPosition))
                {
                    selectedObject = squareCollider.gameObject;
                    isDragging = true;
                }

                startSwipePosition = touchPosition;

                if (trail != null)
                {
                    trail.transform.position = startSwipePosition;
                    trail.enabled = true;
                }
            }
            
            if (touch.phase == TouchPhase.Moved && isDragging && selectedObject != null)
            {
                selectedObject.transform.position = touchPosition;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                if (trail != null)
                {
                    trail.enabled = false;
                }

                if (isDragging)
                {
                    selectedObject = null;
                    isDragging = false;
                }
                else
                {
                    Vector2 endSwipePosition = touchPosition;
                    Vector2 swipeDirection = endSwipePosition - startSwipePosition;

                    if (swipeDirection.magnitude > 0.5f)
                    {
                        // Detecta un Swipe, elimina todos los objetos
                        DeleteAllObjects();
                    }
                }
            }
        }
    }

    void DeleteAllObjects()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("InstantiatedObject");

        for (int i = 0; i < objects.Length; i++)
        {
            Destroy(objects[i]);
        }

        Debug.Log("Todos los objetos han sido eliminados.");
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
    }
    void Update()
    {
        createObject();
        DeleteObject();
        //DetectDragAndSwipe();
    }
}
