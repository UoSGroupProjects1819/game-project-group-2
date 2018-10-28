using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatManager : MonoBehaviour {

    [System.Serializable]
    public struct StatRank
    {
        public string name;
        public Sprite sprite;
        public int amt;
        public int chance;
    }

    public List<StatRank> Ranks;

    [Space(10)]
    public GameObject breedNote;
    public Transform StarPanel;
    public Image IntelligenceImage;
    public Image AgilityImage;
    public Image StrengthImage;
    public Image StyleImage;
    public Image StaminaImage;

}
