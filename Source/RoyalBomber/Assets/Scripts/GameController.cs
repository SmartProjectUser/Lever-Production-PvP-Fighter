using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public UIController uIController;
    private List<PlayerController> bots;
    private PlayerController player;

    private void Start()
    {
        player = uIController.player;
        InitializeBots();
    }

    private void InitializeBots()
    {
        bots = FindObjectsOfType<PlayerController>().ToList();
        bots.Remove(bots.Find(x => x.Equals(player)));
        foreach (var bot in bots)
        {
            bot.OnCharacterLeaveField += BotDied;
        }
    }

    private void BotDied(object sender, PlayerController bot)
    {
        bots.Remove(bots.Find(x => x.Equals(bot)));
        CheckWin();
    }

    private void CheckWin()
    {
        if (bots.Count < 1)
        {
            uIController.Win();
        }
    }
}
