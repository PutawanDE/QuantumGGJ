using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUIUpdater : MonoBehaviour
{
    [SerializeField] protected Slider HPSlider;
    [SerializeField] protected TextMeshProUGUI nameTextMesh;
    [SerializeField] private GameController gameController;
    [SerializeField] private bool isBoss;
    [SerializeField] private Character character;

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
        if (isBoss) character = gameController.enemy.GetComponent<Character>();
        else character = gameController.player.GetComponent<Character>();

        if (character != null)
        {
            if (nameTextMesh != null)
                nameTextMesh.text = character.name;

            if (HPSlider != null)
            {
                if (character.GetHP() <= 0)
                    HPSlider.fillRect.gameObject.SetActive(false);
                else
                {
                    HPSlider.value = character.GetHP() / character.GetMaxHP();
                    HPSlider.fillRect.gameObject.SetActive(true);
                }
            }

        }
    }
}
