using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthControl : MonoBehaviour
{
    public static HealthControl instance;
    private void Awake(){ instance = this; }

    public List<Side> Sides;
    public Sprite Win;
    public Sprite Lose;
    public Sprite Empty;


    [System.Serializable]
    public class Side
    {
        public List<GameObject> Dots;
        public Image Fill;
        public Image Icon;
        public void SetFillSize(float Set)
        {
            Fill.fillAmount = Set;
        }
    }

    public void DisplayWin(int Side)
    {
        for(int i = 0; i > Sides[Side].Dots.Count; i++)
            if (Sides[Side].Dots[i] != Empty)
            {
                Sides[Side].Dots[i].GetComponent<Image>().sprite = Win;
                return;
            }
    }
}
