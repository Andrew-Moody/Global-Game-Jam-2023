using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public int Index { get; private set; }

    private Inventory _inventory;

    [SerializeField]
    private TextMeshProUGUI _count;

    [SerializeField]
    private Image _icon;

    [SerializeField]
    private Sprite _defaultSprite;


    private Image _background;

    void Awake()
    {
        _background = GetComponent<Image>();
    }

    public void Initialize(Inventory inventory, int slotIndex)
	{
        _inventory = inventory;
        Index = slotIndex;

        _icon.sprite = null;
        _count.text = "";

        ShowIcon(false);
	}


    public void ShowIcon(bool show)
	{
        _icon.enabled = show;
        _count.enabled = show;
	}


    public Sprite GetSprite()
	{
        return _icon.sprite;
	}


    public void SetCount(int count)
	{
        if (count == 0 || count == 1)
        {
            _count.text = "";

        }
        else
		{
            _count.text = count.ToString();
        }
        
	}


    public void SetSprite(Sprite sprite)
	{
        _icon.sprite = sprite;
	}
}
