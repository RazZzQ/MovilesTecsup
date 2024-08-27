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
    public void AsignateObject(Vector3 victim)
    {
        if(selectorManager != null)
        {
            spriteRenderer.sprite = selectorManager.character.gameObject.GetComponent<SpriteRenderer>().sprite;
            spriteRenderer.color = selectorManager.color;
            GameObject newObject = Instantiate(spriteRenderer.gameObject, victim, Quaternion.identity); // Crear en la posición
            newObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    public void createObject()
    {
        if (Input.touchCount > 0) 
        {
            Touch MiTouch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(MiTouch.position.x, MiTouch.position.y, 10)); // Ajusta el valor z según sea necesario.
            AsignateObject(touchPosition);
        }
    }
    void Awake()
    {
        loadComponent();
    }
    public void loadComponent()
    {
        //spriteRenderer = selectorManager.character.gameObject.GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        createObject();
    }
}
