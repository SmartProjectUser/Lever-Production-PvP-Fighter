using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Joystick joystick;
    public PlayerController player;
    public Button fireBtn;
    public Text gameOver;
    public Text youWin;
    public Button restartBtn;

    private bool gameEnd = false;
    void Start()
    {
        joystick.OnMoveJoystick += JoystickMove;
        player.OnCharacterLeaveField += GameOver;
    }

    void JoystickMove(object sender, Vector3 direction)
    {
        player.MoveCharacter(direction);
    }

    void GameOver(object sender, PlayerController player)
    {
        if (!gameEnd)
        {
            gameEnd = true;
            gameOver.gameObject.SetActive(true);
            restartBtn.gameObject.SetActive(true);
            fireBtn.gameObject.SetActive(false);
            joystick.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        { FireBtnPressed(); }
    }

    public void FireBtnPressed()
    {
        player.Fire();
    }

    public void RestartBtnPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Win()
    {
        if (!gameEnd)
        {
            gameEnd = true;
            youWin.gameObject.SetActive(true);
            restartBtn.gameObject.SetActive(true);
            fireBtn.gameObject.SetActive(false);
            joystick.gameObject.SetActive(false);
        }
    }
}
