using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

[System.Serializable] 
public class EvidenceSprite
{
    public string evidenceName; // should match GameObject of evidence name
    public Sprite evidenceSprite; // corresponding UI sprite
}


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public Transform itemEvidenceContent;
    public Transform itemPuzzleContent;
    public GameObject puzzleItemPrefab;
    public GameObject evidenceItemPrefab;
    public GameObject comboResultPrefab;
    public List<EvidenceSprite> evidenceSpriteList = new List<EvidenceSprite>(); 
    public Dictionary<string, Sprite> evidenceSprites = new Dictionary<string, Sprite>(); 

    private List<string> pickedEvidences = new List<string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (var entry in evidenceSpriteList)
        {
            evidenceSprites[entry.evidenceName] = entry.evidenceSprite;
        }
    }

    public void AddEvidenceToInventory(string evidenceName, string itemID = "", bool isDraggable = false)
    {
        if (!pickedEvidences.Contains(evidenceName))
        {
            pickedEvidences.Add(evidenceName);

            if (evidenceSprites.ContainsKey(evidenceName))
            {
                // Choose correct prefab and parent container
                GameObject prefabToUse = isDraggable ? puzzleItemPrefab : evidenceItemPrefab;
                Transform parentTransform = isDraggable ? itemPuzzleContent : itemEvidenceContent;

                // Instantiate the correct prefab
                GameObject newItem = Instantiate(prefabToUse, parentTransform);
                Image itemImage = newItem.GetComponent<Image>();
                itemImage.sprite = evidenceSprites[evidenceName];

                if (isDraggable)
                {
                    DraggableItem draggable = newItem.AddComponent<DraggableItem>();
                    draggable.itemID = string.IsNullOrEmpty(itemID) ? evidenceName : itemID;
                    draggable.canCombine = CanBeCombined(draggable.itemID);
                }
                else
                {
                    // Disable raycast target on non-draggable images to prevent any interaction
                    itemImage.raycastTarget = false;
                }
            }
        }
    }



    private bool CanBeCombined(string itemID)
    {
        HashSet<string> combinableItems = new HashSet<string> { "itemA", "itemB", "itemC" };
        return combinableItems.Contains(itemID);
    }


    public void CheckCombination(DraggableItem droppedItem)
    {
        if (!droppedItem.canCombine) return; // Ignore non-combinable items

        List<DraggableItem> itemsInComboArea = new List<DraggableItem>();

        foreach (Transform child in GameObject.Find("ComboArea").transform)
        {
            DraggableItem item = child.GetComponent<DraggableItem>();
            if (item != null && item.canCombine)
            {
                itemsInComboArea.Add(item);
            }
        }

        DraggableItem matchedItem = null;
        foreach (DraggableItem item in itemsInComboArea)
        {
            if (item != droppedItem && item.itemID == droppedItem.itemID)
            {
                matchedItem = item;
                break;
            }
        }

        if (matchedItem != null)
        {
            Destroy(droppedItem.gameObject);
            Destroy(matchedItem.gameObject);

            GameObject newItem = Instantiate(puzzleItemPrefab, GameObject.Find("ComboArea").transform);
            newItem.GetComponent<Image>().sprite = GetComboResultSprite(droppedItem.itemID);
            DraggableItem newDraggable = newItem.AddComponent<DraggableItem>();
            newDraggable.canCombine = false; // Result should not be combinable again
        }
    }


    private Sprite GetComboResultSprite(string itemID)
    {
        Dictionary<string, Sprite> comboResults = new Dictionary<string, Sprite>
    {
        { "itemA", Resources.Load<Sprite>("ComboResult1") },
        { "itemB", Resources.Load<Sprite>("ComboResult2") }
    };

        return comboResults.ContainsKey(itemID) ? comboResults[itemID] : null;
    }


    public void ClearInventory()
    {
        foreach (Transform child in itemPuzzleContent)
        {
            Destroy(child.gameObject);
        }
        pickedEvidences.Clear();
    }
}
