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
    private GameObject draggedObject = null;
    private Color selectedColor;
    private Vector3 swipeStartPos;
    private bool MouseFree;
    private bool isSwiping = false;
    public float doubleTapThreshold = 0.3f;
    public GameObject trailEffectPrefab;

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
    void Start()
    {
        MouseFree = true;
    }
    public void createObject()
    {
        if (Input.touchCount > 0)
        {
            Touch MiTouch = Input.GetTouch(0);
            TouchPosition = Camera.main.ScreenToWorldPoint(new Vector3(MiTouch.position.x, MiTouch.position.y, 10));
            if (canClick)
            {
                    AsignateObject(TouchPosition);
                //if (IsMouseOverSquare(TouchPosition))
                //{
                //}
                canClick = false;
            }
        }
        else
        {
            canClick = true;
        }
    }
public void Dobject()
{
    if (Input.touchCount == 1)
    {
        Touch touch = Input.GetTouch(0);
        Vector3 touchPosition = touch.position;
        if(touch.phase == TouchPhase.Began)
            {
                tapCount++;
            }
        if(touch.phase == TouchPhase.Ended)
            {
                if (tapCount == 2)
                {
                    // Aquí se eliminaría el objeto
                    Destroy(newObject);
                    tapCount = 0;
                }
            }
        }
}
    public void useVoid()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = touch.position;
            switch (touch.phase)
            {
                case TouchPhase.Moved:
                    if (draggedObject != null)
                    {
                        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Camera.main.nearClipPlane));
                        worldPosition.z = 0;
                        draggedObject.transform.position = worldPosition;
                    }

                    // Detectar el swipe
                    if (Vector2.Distance(touchPosition, swipeStartPos) > 20)
                    {
                        isSwiping = true;
                    }
                    break;

                case TouchPhase.Ended:
                    if (isSwiping)
                    {
                        CreateTrailEffect(touchPosition);
                        DeleteAllObjects();
                    }

                    draggedObject = null;
                    break;
            }
        }
        void DeleteAllObjects()
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Victim"))
            {
                Destroy(obj);
            }
        }


        void CreateTrailEffect(Vector2 swipeEndPos)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(swipeEndPos.x, swipeEndPos.y, Camera.main.nearClipPlane));
            worldPosition.z = 0;

            if (trailEffectPrefab != null)
            {
                GameObject trailEffect = Instantiate(trailEffectPrefab, worldPosition, Quaternion.identity);
                TrailRenderer trailRenderer = trailEffect.GetComponent<TrailRenderer>();
                if (trailRenderer != null)
                {
                    trailRenderer.startColor = selectedColor;
                    trailRenderer.endColor = selectedColor;
                }

                Destroy(trailEffect, 1f);
            }
        }
    }
    public void DeleteObject(Vector3 touchPosition)
{
    // Crear un raycast desde la cámara hacia la posición del toque en el mundo
    Ray ray = Camera.main.ScreenPointToRay(touchPosition);
    RaycastHit hit;

    // Verificar si el raycast ha impactado algo en el espacio 3D
    if (Physics.Raycast(ray, out hit))
    {
        // Verificar si el objeto impactado tiene un collider
        if (hit.collider != null)
        {
            Debug.Log("Objeto eliminado: " + hit.collider.gameObject.name);
            Destroy(hit.collider.gameObject);
        }
        else
        {
            Debug.Log("No se encontró ningún objeto en la posición.");
        }
    }
    else
    {
        Debug.Log("No se encontró ningún objeto en la posición.");
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
    }
    void Update()
    {
        createObject();
        Dobject();
        //useVoid();
    }
}
