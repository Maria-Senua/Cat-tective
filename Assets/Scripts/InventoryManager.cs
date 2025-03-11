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
    public Sprite combinedSprite;
}


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public Transform itemEvidenceContent;
    public Transform itemPuzzleContent;
    public GameObject puzzleItemPrefab;
    public GameObject evidenceItemPrefab;
    public List<EvidenceSprite> evidenceSpriteList = new List<EvidenceSprite>(); 
    public Dictionary<string, Sprite> evidenceSprites = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> combinedSprites = new Dictionary<string, Sprite>();  

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
            if (entry.combinedSprite != null) 
            {
                combinedSprites[entry.evidenceName] = entry.combinedSprite;
            }
        }
    }

    public Sprite GetCombinedSprite(string evidenceName)
    {
        if (combinedSprites.ContainsKey(evidenceName))
        {
            return combinedSprites[evidenceName];
        }
        return null;
    }

    public void AddEvidenceToInventory(string evidenceName, bool isDraggable = false)
    {
        if (!pickedEvidences.Contains(evidenceName))
        {
            pickedEvidences.Add(evidenceName);

            if (evidenceSprites.ContainsKey(evidenceName))
            {
                GameObject prefabToUse = isDraggable ? puzzleItemPrefab : evidenceItemPrefab;
                Transform parentTransform = isDraggable ? itemPuzzleContent : itemEvidenceContent;

                GameObject placeholder = parentTransform.Find("PlaceholderImage")?.gameObject;
                if (placeholder != null)
                {
                    placeholder.transform.SetAsFirstSibling(); 
                }

                GameObject newItem = Instantiate(prefabToUse, parentTransform);
                Image itemImage = newItem.GetComponent<Image>();
                itemImage.sprite = evidenceSprites[evidenceName];

                newItem.name = evidenceName;

                if (isDraggable)
                {
                    DraggableItem draggable = newItem.AddComponent<DraggableItem>();
                    draggable.evidenceName = evidenceName;
                }
                else
                {
                    itemImage.raycastTarget = false;
                }
            }
        }
    }




   


   


    public void ClearInventory()
    {
        foreach (Transform child in itemPuzzleContent)
        {
            if (child.name != "PlaceholderImage") 
            {
                Destroy(child.gameObject);
            }
        }
        pickedEvidences.Clear();
    }
}
