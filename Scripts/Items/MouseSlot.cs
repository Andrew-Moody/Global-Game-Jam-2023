using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSlot : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;


    [SerializeField]
    private Image _icon;

    [SerializeField]
    private Image _cursor;

    private Vector3 _position;

    
    void Awake()
    {
        ClearSprite();
    }


    public void SetSprite(Sprite sprite)
	{
        _icon.enabled = true;
        _icon.sprite = sprite;
	}


    public void ClearSprite()
	{
        _icon.enabled = false;
	}

    
    void Update()
    {
        bool inScreen = false;

        // LateUpdate doesnt reduce update lag
        if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            inScreen = RectTransformUtility.RectangleContainsScreenPoint((RectTransform)_canvas.transform, Input.mousePosition, _canvas.worldCamera);
            RectTransformUtility.ScreenPointToWorldPointInRectangle((RectTransform)_canvas.transform, Input.mousePosition, _canvas.worldCamera, out _position);
        }
        else if (_canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            inScreen = RectTransformUtility.RectangleContainsScreenPoint((RectTransform)_canvas.transform, Input.mousePosition);
            _position = Input.mousePosition;
        }

        if (inScreen)
		{
            transform.position = _position;
        }
        
    }
}
