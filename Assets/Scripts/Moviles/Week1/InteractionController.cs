using System.Collections.Generic;
using UnityEngine;

namespace Moviles.Week1
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] private ObjectData objectData;
        [SerializeField] private GameObject objectSPrefab;
        [SerializeField] private LayerMask Layer;
        private Color _currentColor;
        private Sprite _currentSprite;

        private List<GameObject> _objectsList;
        private void Start()
        {
            _objectsList = new();
        }

        private void OnEnable()
        {
            objectData.OnColorUpdate += UpdateColor;
            objectData.OnSpriteUpdate += UpdateSrpite;
        }

        private void OnDisable()
        {
            objectData.OnColorUpdate -= UpdateColor;
            objectData.OnSpriteUpdate -= UpdateSrpite;
        }

        private void UpdateColor(Color newColor)
        {
            _currentColor = newColor;
        }

        private void UpdateSrpite(Sprite newSprite)
        {
            _currentSprite = newSprite;
        }

        public void InstantiateObject(Vector3 spawnPosition)
        {
            Debug.Log("wasaa");
            spawnPosition = ScreenToWorldPoint(spawnPosition);

            GameObject newObject = Instantiate(objectSPrefab, spawnPosition, Quaternion.identity);
            newObject.GetComponent<SpriteRenderer>().color = _currentColor;
            newObject.GetComponent<SpriteRenderer>().sprite = _currentSprite;

            _objectsList.Add(newObject);
        }
        public void deleteObject(Vector3 VictimPosition)
        {

            Ray ray = Camera.main.ScreenPointToRay(VictimPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Layer))
            {
                Debug.Log("Position To Eliminate: " + VictimPosition);
                if (hit.collider.CompareTag("Victim") && hit.collider !=null)
                {
                    Debug.Log("Object Elminate: " + hit.collider.gameObject.name);
                    Destroy(hit.collider.gameObject);
                }
                else
                {
                    Debug.Log("Don't find a object in that position!");
                }
            }

        }

        public void SelectObject(Vector3 targetPosition)
        {
            targetPosition = ScreenToWorldPoint(targetPosition);

            Debug.Log($"Selecting object at {targetPosition}");
        }

        public void MoveObject(Vector3 movePosition)
        {
            movePosition = ScreenToWorldPoint(movePosition);

            Debug.Log($"Moving object to {movePosition}");
        }

        public void DeleteAll()
        {
            for (int i = 0; i < _objectsList.Count; i++)
            {
                Destroy(_objectsList[i]);
            }

            _objectsList.Clear();
        }


        private static Vector3 ScreenToWorldPoint(Vector3 value)
        {
            value = Camera.main.ScreenToWorldPoint(value);

            return new Vector3(value.x, value.y, 0);
        }
    }
}