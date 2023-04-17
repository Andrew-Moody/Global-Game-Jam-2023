using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private List<Canvas> _canvases;

    [SerializeField]
    private TextMeshProUGUI _endText;

	private void Awake()
	{
		if (Instance == null)
		{
            Instance = this;
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


    public void ShowCanvas(int id, bool show)
	{
        _canvases[id].gameObject.SetActive(show);
	}

    public void SetEndMessage(string message)
	{
        _endText.text = message;
	}
}
