using TMPro;
using UnityEngine;

public class ScoreboardUI : MonoBehaviour
{
    public TextMeshProUGUI teamANameText;
    public TextMeshProUGUI teamAScoreText;
    public TextMeshProUGUI teamBNameText;
    public TextMeshProUGUI teamBScoreText;

    private int teamAScore = 0;
    private int teamBScore = 0;

    // Set names on scoreboard
    public void SetTeamNames(string teamA, string teamB)
    {
        teamANameText.text = teamA;
        teamBNameText.text = teamB;
    }

    // Overwrite both scores manually
    public void UpdateScore(int newTeamAScore, int newTeamBScore)
    {
        teamAScore = newTeamAScore;
        teamBScore = newTeamBScore;

        RefreshScoreUI();
    }

    // Add points to a team (0 = Team A, 1 = Team B)
    public void AddScore(int teamIndex, int points)
    {
        if (teamIndex == 0)
        {
            teamAScore += points;
        }
        else if (teamIndex == 1)
        {
            teamBScore += points;
        }

        RefreshScoreUI();
    }

    // Helper method to update the visual score text
    private void RefreshScoreUI()
    {
        teamAScoreText.text = teamAScore.ToString();
        teamBScoreText.text = teamBScore.ToString();
    }
}
