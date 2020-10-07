using UnityEngine.UI;
using UnityEngine;

public class BattlePanelControllor : MonoBehaviour
{
    public GameObject BattleBtn;
    public Text BattleText;

    public void OnClickStartBattle()
    {
        BattleControllor.Instance.SetupBattle();
        BattleBtn.SetActive(false);
    }

    public void SetupBattleText(string text)
    {
        BattleText.text = text;
    }
}
