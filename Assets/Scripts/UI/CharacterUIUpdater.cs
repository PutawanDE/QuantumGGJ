using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUIUpdater : MonoBehaviour
{
    [SerializeField] protected Slider HPSlider;
    [SerializeField] protected TextMeshProUGUI nameTextMesh;
    [SerializeField] protected Character character;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    protected void UpdateUI()
    {
        if (character != null)
        {
            if (nameTextMesh != null)
                nameTextMesh.text = character.name;
        }
    }
}
