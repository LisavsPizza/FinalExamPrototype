using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoisonDice : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnStopped()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        int diceRoll = Random.Range(1, 7);

        GameObject.Find("ClickTheDice").GetComponent<Text>().text = "DICE VALUES: ";

        FreezeOnRoll(diceRoll);

        if (DiceManager.PPFound() == false)
        {

            GameObject pp = DiceManager.FindEmptyTypeTotal();
            pp.GetComponent<Text>().text = "PP: ";
            pp.transform.GetChild(0).GetComponent<Text>().enabled = true;

            if (!TurnManager.GetCurrTurnCharacter().tag.Contains("Enemy"))
            {
                GameObject.Find("ConfirmAttackButton").GetComponent<CustomButton>().Enable();
            }

            DiceManager.SetPPFound(true);
        }

        string rollValueName = "RollValue" + Dice.LastDiceClicked().name.ToCharArray()[Dice.LastDiceClicked().name.Length - 1];
        GameObject.Find(rollValueName).GetComponent<Text>().text = diceRoll.ToString();

        int poisonTotal = int.Parse(DiceManager.FindTypeTotalGameObject("PP").transform.GetChild(0).GetComponent<Text>().text);
        poisonTotal += diceRoll;
        DiceManager.FindTypeTotalGameObject("PP").transform.GetChild(0).GetComponent<Text>().text = poisonTotal.ToString();
    }

    public void OnAttack()
    {
        if (DiceManager.FindTypeTotalGameObject("PP") != null)
        {
            foreach (Character c in DiceManager.GetCurrTargets())
            {
                GameObject poisonCanvas = Utilities.SearchChild("PoisonCanvas", c.gameObject);
                GameObject poison = Utilities.SearchChild("PoisonImage", poisonCanvas);

                int poisonTotal = int.Parse(DiceManager.FindTypeTotalGameObject("PP").transform.GetChild(0).GetComponent<Text>().text);

                poison.GetComponent<Poison>().AddPoison(poisonTotal);
            }
        }
    }

    private void FreezeOnRoll(int num)
    {
        Dice.LastDiceClicked().GetComponent<Animator>().enabled = false;

        DiceType myDiceType = DiceManager.SearchDiceType("Poison");

        List<Sprite> freezeSprites = myDiceType.GetFreezeSprites();

        Sprite freezeSprite = null;

        foreach (Sprite s in freezeSprites)
        {
            if (s.name.Contains(num.ToString()))
            {
                freezeSprite = s;
            }
        }

        Dice.LastDiceClicked().GetComponent<Image>().sprite = freezeSprite;
    }

}
