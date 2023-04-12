using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{

    private SpriteRenderer _spriteRenderer;

    private ItemData _itemData;
    
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public ItemData GetItemData()
	{
        return _itemData;
	}

    public void SetItemData(ItemData itemData)
	{
        _itemData = itemData;

        _spriteRenderer.sprite = itemData.Sprite;
	}

}
