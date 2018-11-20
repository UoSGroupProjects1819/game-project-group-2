using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatManager : MonoBehaviour {

    public static StatManager Instance;

    [System.Serializable]
    public struct StatRank
    {
        public string name;
        public Sprite sprite;
        public int amt;
        public int minAmtPerLevel;
        public int maxAmtPerLevel;
        public int chance;
    }

    public List<StatRank> Ranks;

    public GameObject targetCreature;

    [Space(10)]
    public GameObject statsPanel;
    public Text nameText;
    public Image happinessImage;
    public GameObject breedNote;
    public Transform StarPanel;
    public Image IntelligenceImage;
    public Image AgilityImage;
    public Image StrengthImage;
    public Image StyleImage;
    public Image StaminaImage;

    [Space(10)]
    public Transform intelligenceUpgradeMeter;
    public Transform agilityUpgradeMeter;
    public Transform strengthUpgradeMeter;
    public Transform styleUpgradeMeter;
    public Transform staminaUpgradeMeter;

    [Space(10)]
    public Text intelligenceLevel;
    public Text agilityLevel;
    public Text strengthLevel;
    public Text styleLevel;
    public Text staminaLevel;

    private void Start()
    {
        Instance = this;
    }

    public StatRank FindRank(string rankLetter)
    {
        foreach (var rank in Ranks)
        {
            if(rank.name == rankLetter)
            {
                return rank;
            }
        }

        Debug.Log("Couldn't Find Rank");

        return new StatRank();
    } 

    public void BreedButton()
    {
        targetCreature.GetComponent<CreatureScript>().StartBreed();
        statsPanel.SetActive(false);
    }

}
