using TMPro;
using UnityEngine;

public class GameRuleManager : MonoBehaviour
{
    public Transform player;
    public Transform[] startPoints;

    public TextMeshProUGUI pointText;
    public GameObject winText;

    public int totalPointCount = 4;

    private int currentPointCount = 0;
    private bool gameFinished = false;

    void Start()
    {
        SetRandomStartPosition();
        UpdatePointText();

        if (winText != null)
        {
            winText.SetActive(false);
        }
    }

    void SetRandomStartPosition()
    {
        if (player == null || startPoints == null || startPoints.Length == 0)
        {
            Debug.LogWarning("PlayerまたはStartPointsが設定されていません");
            return;
        }

        int index = Random.Range(0, startPoints.Length);
        Transform selectedStartPoint = startPoints[index];

        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
        {
            controller.enabled = false;
            player.position = selectedStartPoint.position;
            player.rotation = selectedStartPoint.rotation;
            controller.enabled = true;
        }
        else
        {
            player.position = selectedStartPoint.position;
            player.rotation = selectedStartPoint.rotation;
        }

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.startPoint = selectedStartPoint;
        }

        Debug.Log("StartPoint_" + (index + 1) + " から開始");
    }

    public void GetPoint(PointItem pointItem)
    {
        if (gameFinished)
        {
            return;
        }

        if (pointItem == null)
        {
            return;
        }

        if (pointItem.IsCollected())
        {
            return;
        }

        pointItem.Collect();

        currentPointCount++;
        UpdatePointText();

        Debug.Log("Point: " + currentPointCount + " / " + totalPointCount);

        if (currentPointCount >= totalPointCount)
        {
            FinishGame();
        }
    }

    void UpdatePointText()
    {
        if (pointText != null)
        {
            pointText.text = "Point : " + currentPointCount + " / " + totalPointCount;
        }
    }

    void FinishGame()
    {
        if (gameFinished)
        {
            return;
        }

        gameFinished = true;

        if (winText != null)
        {
            winText.SetActive(true);
        }

        if (player != null)
        {
            PlayerMove playerMove = player.GetComponent<PlayerMove>();

            if (playerMove != null)
            {
                playerMove.enabled = false;
            }

            SwordAttack swordAttack = player.GetComponent<SwordAttack>();

            if (swordAttack != null)
            {
                swordAttack.enabled = false;
            }

            JoyconSwingAttack joyconSwingAttack = player.GetComponent<JoyconSwingAttack>();

            if (joyconSwingAttack != null)
            {
                joyconSwingAttack.enabled = false;
            }
        }

        Debug.Log("4つのポイントを集めました。ゲーム終了");
    }
}