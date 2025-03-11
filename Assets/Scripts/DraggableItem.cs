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

    private Sprite combinedSprite;

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

        combinedSprite = InventoryManager.instance.GetCombinedSprite(evidenceName);

        if (combinedSprite != null)
        {
            Debug.Log("combinedSprite assigned from InventoryManager: " + combinedSprite.name);
        }
        else
        {
            Debug.LogError("combinedSprite is NULL! Make sure it's set in InventoryManager's evidenceSpriteList.");
        }
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

        DraggableItem[] itemsInPart1 = part1.GetComponentsInChildren<DraggableItem>();
        DraggableItem[] itemsInPart2 = part2.GetComponentsInChildren<DraggableItem>();

        DraggableItem itemInPart1 = itemsInPart1.Length > 0 ? itemsInPart1[0] : null;
        DraggableItem itemInPart2 = itemsInPart2.Length > 0 ? itemsInPart2[0] : null;

        if (itemInPart1 != null && itemInPart2 != null)
        {
            string name1 = itemInPart1.evidenceName;
            string name2 = itemInPart2.evidenceName;

            Debug.Log($"Checking {name1} (in part1) and {name2} (in part2) for combination...");

            if ((name1 == "photo1" && name2 == "photo2" && itemInPart1.transform.parent == part1.transform && itemInPart2.transform.parent == part2.transform) ||
                (name1 == "photo2" && name2 == "photo1" && itemInPart1.transform.parent == part2.transform && itemInPart2.transform.parent == part1.transform))
            {
                Debug.Log("Correct items placed in the correct parts! Updating image...");

                Sprite correctCombinedSprite = InventoryManager.instance.GetCombinedSprite(name1);

                if (correctCombinedSprite != null && placeholderImage != null)
                {
                    Image imageComponent = placeholderImage.GetComponent<Image>();
                    if (imageComponent != null)
                    {
                        imageComponent.sprite = correctCombinedSprite;
                        imageComponent.SetNativeSize();
                        imageComponent.enabled = false;
                        imageComponent.enabled = true;

                        Debug.Log("Placeholder image updated with: " + correctCombinedSprite.name);
                    }
                }
            }
            else
            {
                Debug.LogWarning("Items are not in the correct slots. No update will occur.");
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
