using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public float Speed;
	private SpriteRenderer _spriteRenderer;

	private bool flipped;

	private void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update()
    {
        
    }

    public void FlipX(bool flipX)
	{
		_spriteRenderer.flipX = flipX;

		flipped = flipX;

		if (flipX)
		{
			transform.localPosition = new Vector3(-0.2f, -0.25f, 0f);
		}
		else
		{
			transform.localPosition = new Vector3(0.2f, -0.25f, 0f);
		}
	}


	public void SetSprite(Sprite sprite)
	{
		_spriteRenderer.sprite = sprite;
	}


    public void SwingWeapon()
	{
        StopCoroutine(Swing());

		transform.rotation = Quaternion.identity;

		StartCoroutine(Swing());
	}


    private IEnumerator Swing()
	{
        for (float angle = 0f; angle < 30f; angle += (Speed * Time.deltaTime))
		{
			if (flipped)
			{
				transform.rotation = Quaternion.Euler(0f, 0f, angle);
			}
			else
			{
				transform.rotation = Quaternion.Euler(0f, 0f, -angle);
			}

			yield return null;
		}

		transform.rotation = Quaternion.identity;
	}
}
