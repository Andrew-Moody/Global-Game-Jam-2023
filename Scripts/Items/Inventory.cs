using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField]
	private MouseSlot _mouseSlot;

	[SerializeField]
    private InventorySlot[] _slots;

	private ItemData[] _items;

	private int _mouseIndex;

	private bool _dragging;

	void Start()
    {
        InitializeSlots();
    }

   
    void Update()
    {
        
    }


    public void InitializeSlots()
	{
		_items = new ItemData[_slots.Length];

		for (int i = 0; i < _slots.Length; i++)
		{
            _slots[i].Initialize(this, i);

			_items[i] = new ItemData();
		}

		Debug.Log(_items.Length);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		InventorySlot slot = eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>();

		if (slot == null)
		{
			return;
		}

	}


	public void OnPointerUp(PointerEventData eventData)
	{
		if (!_dragging)
		{
			InventorySlot slot = eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>();

			if (slot == null)
			{
				_dragging = false;
				return;
			}

			if (_items[slot.Index].Stats.IsConsumable && _items[slot.Index].Quantity > 0)
			{
				Player.Instance.ConsumeItem(_items[slot.Index]);

				_items[slot.Index].Quantity -= 1;

				UpdateSlot(slot.Index);
			}
		}
		else
		{
			_dragging = false;
		}
	}


	public void OnBeginDrag(PointerEventData eventData)
	{
		_dragging = true;

		InventorySlot slot = eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>();

		if (slot == null)
		{
			return;
		}

		if (_items[slot.Index].Quantity != 0)
		{
			_mouseIndex = slot.Index;
			_mouseSlot.SetSprite(slot.GetSprite());
			slot.ShowIcon(false);
		}
		else
		{
			_mouseIndex = -1;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (_mouseIndex == -1)
		{
			return;
		}

		GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;

		InventorySlot slot = null;

		if (hitObject != null)
		{
			slot = hitObject.GetComponent<InventorySlot>();
		}

		_mouseSlot.ClearSprite();

		
		if (slot == null)
		{
			// Delete Item
			if (!EventSystem.current.IsPointerOverGameObject() && _mouseIndex > 1)
			{
				_items[_mouseIndex].ID = 0;
				_items[_mouseIndex].Quantity = 0;
				UpdateSlot(_mouseIndex);
			}
			else
			{
				// Return to previous
				_slots[_mouseIndex].ShowIcon(true);
			}
			return;
		}

		// Moving to or from equipment
		if (_mouseIndex < 2 || slot.Index < 2)
		{
			if (_mouseIndex < 2 && _items[slot.Index].EquipSlot != _mouseIndex && _items[slot.Index].Quantity != 0)
			{
				_slots[_mouseIndex].ShowIcon(true);
				return;
			}

			if (slot.Index < 2 && _items[_mouseIndex].EquipSlot != slot.Index)
			{
				_slots[_mouseIndex].ShowIcon(true);
				return;
			}
		}

		ItemData tempItem = _items[slot.Index];
		_items[slot.Index] = _items[_mouseIndex];
		_items[_mouseIndex] = tempItem;

		UpdateSlot(slot.Index);
		UpdateSlot(_mouseIndex);
	}

	public void OnDrag(PointerEventData eventData)
	{
		// Required to handle for other Drag events but nothing needs to be done
	}


	/// <summary>
	/// Attempt to pickup item, first buy splitting the quantity across any existing stacks of the item, then in the first available empty slot
	/// </summary>
	/// <param name="itemData"></param>
	/// <returns>Returns true if the item is comepletely picked</returns>
	public bool TryTakeItem(ref ItemData itemData)
	{
		int firstEmpty = -1;

		// See if stack of the item already exists
		for (int i = 0; i < _items.Length; i++)
		{
			if (firstEmpty == -1 && _items[i].ID == 0)
			{
				if (itemData.EquipSlot == i || i > 1)
					firstEmpty = i;
			}

			// Transfer some or all of the item to existing stack
			if (itemData.ID == _items[i].ID)
			{
				if (_items[i].Quantity < _items[i].StackLimit)
				{
					int spaceInStack = _items[i].StackLimit - _items[i].Quantity;

					// Transfer all
					if (itemData.Quantity <= spaceInStack)
					{
						_items[i].Quantity += itemData.Quantity;
						itemData.Quantity = 0;
						itemData.ID = 0;

						//_slots[i].SetCount(_items[i].Quantity);
						UpdateSlot(i);

						return true;
					}
					else
					{
						// Transfer some
						itemData.Quantity -= spaceInStack;
						_items[i].Quantity += spaceInStack;

						UpdateSlot(i);
					}
				}
			}

			
		}

		// At this point there should be no more existing stacks with space but maybe empty slots
		if (firstEmpty != -1)
		{
			_items[firstEmpty] = itemData; // Since this is a struct this is a copy? IE cant be altered by the caller
			UpdateSlot(firstEmpty);
			return true;
		}

		return false;
	}


	private void UpdateSlot(int index)
	{
		_slots[index].SetCount(_items[index].Quantity);
		_slots[index].SetSprite(_items[index].Sprite);

		if (_items[index].Quantity != 0)
		{
			_slots[index].ShowIcon(true);
		}
		else
		{
			_slots[index].ShowIcon(false);
			_items[index].ID = 0;
		}

		if (index < 2)
		{
			Player.Instance.EquipItem(_items[index]);
		}
	}
}
