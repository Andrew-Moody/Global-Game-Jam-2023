using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    public float Health;

    public Boss ParentBoss;

    public SpriteRenderer SpriteRenderer;

    public float Speed;


    public void TakeDamage(float damage)
	{
        Health -= damage;

        StopCoroutine(ShowDamage());
        StartCoroutine(ShowDamage());

        AudioManager.Instance.PlaySound(2);

        if (Health <= 0f)
		{
            ParentBoss.RootDied();

            AudioManager.Instance.PlaySound(3);

            Destroy(gameObject);
		}
	}


    private IEnumerator ShowDamage()
	{
        Color color = Color.white;

        for (float bg = 1f; bg >= 0; bg -= (Speed * Time.deltaTime))
		{
            color.b = bg;
            color.g = bg;
            SpriteRenderer.color = color;
            yield return null;
		}

        for (float bg = 0f; bg <= 1f; bg += (Speed * Time.deltaTime))
        {
            Debug.Log("test");
            color.b = bg;
            color.g = bg;
            SpriteRenderer.color = color;
            yield return null;
        }

        SpriteRenderer.color = Color.white;
    }
}
