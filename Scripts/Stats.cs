using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    [SerializeField]
    private Image _healthBar;

    [SerializeField]
    private Image _manaBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdateHealth(float health)
	{
        _healthBar.rectTransform.localScale = new Vector3(health, 1f, 1f);
	}


    public void UpdateMana(float mana)
    {
        _manaBar.rectTransform.localScale = new Vector3(mana, 1f, 1f);
    }
}
