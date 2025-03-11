using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector3 originalPosition;

    [SerializeField] private Sprite combinedSprite;

    private string _evidenceName;
    public string evidenceName
    {
        get
        {
            if (string.IsNullOrEmpty(_evidenceName))
            {
                _evidenceName = gameObject.name;
            }
            return _evidenceName;
        }
        set
        {
            _evidenceName = value; 
        }
    }


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        Debug.Log(gameObject.name + " has evidenceName: " + evidenceName);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = transform.position;
        transform.SetParent(originalParent.root); 
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        GameObject part1 = GameObject.FindGameObjectWithTag("part1");
        GameObject part2 = GameObject.FindGameObjectWithTag("part2");
        GameObject placeholderImage = GameObject.Find("PlaceholderImage");

        bool isInPart1 = part1 != null && IsPointerOverUIElement("part1");
        bool isInPart2 = part2 != null && IsPointerOverUIElement("part2");

        if (isInPart1)
        {
            Debug.Log(gameObject.name + " assigned to part1");
            transform.SetParent(part1.transform, false);
        }
        else if (isInPart2)
        {
            Debug.Log(gameObject.name + " assigned to part2");
            transform.SetParent(part2.transform, false);
        }

        StartCoroutine(WaitAndCheck(placeholderImage));
    }


    private IEnumerator WaitAndCheck(GameObject placeholderImage)
    {
        GameObject part1 = GameObject.FindGameObjectWithTag("part1");
        GameObject part2 = GameObject.FindGameObjectWithTag("part2");

        while (part1.GetComponentsInChildren<DraggableItem>().Length == 0 ||
               part2.GetComponentsInChildren<DraggableItem>().Length == 0)
        {
            yield return null; 
        }

        Debug.Log("Checking children in part1: ");
        foreach (Transform child in part1.transform)
        {
            Debug.Log("child name 1 " + child.name);
        }
        Debug.Log("Checking children in part2:");
        foreach (Transform child in part2.transform)
        {
            Debug.Log("child name 2 " + child.name);
        }

        DraggableItem[] itemsInPart1 = part1.GetComponentsInChildren<DraggableItem>();
        DraggableItem[] itemsInPart2 = part2.GetComponentsInChildren<DraggableItem>();

        DraggableItem itemInPart1 = itemsInPart1.Length > 0 ? itemsInPart1[0] : null;
        DraggableItem itemInPart2 = itemsInPart2.Length > 0 ? itemsInPart2[0] : null;

        Debug.Log("Final Check - itemInPart1: " + (itemInPart1 != null ? itemInPart1.evidenceName : "NULL"));
        Debug.Log("Final Check - itemInPart2: " + (itemInPart2 != null ? itemInPart2.evidenceName : "NULL"));

        if (itemInPart1 != null && itemInPart2 != null)
        {
            if ((itemInPart1.evidenceName == "photo1" && itemInPart2.evidenceName == "photo2") ||
                (itemInPart1.evidenceName == "photo2" && itemInPart2.evidenceName == "photo1"))
            {
                Debug.Log("Correct items placed! Updating image...");

                if (placeholderImage != null)
                {
                    Image imageComponent = placeholderImage.GetComponent<Image>();
                    if (imageComponent != null)
                    {
                        Debug.Log("Placeholder image updated!");
                        imageComponent.sprite = combinedSprite;
                    }
                }
            }
        }
    }







    private bool IsPointerOverUIElement(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            RectTransform rect = obj.GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition))
            {
                return true;
            }
        }
        return false;
    }
}
