using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EvidenceCheck
{
    public Image evidence;
    public Sprite uncheckedSprite; 
    public Sprite checkedSprite;
}

public class EvidenceManager : MonoBehaviour
{
    public static EvidenceManager instance;
    public List<EvidenceCheck> evidenceSpriteList = new List<EvidenceCheck>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
