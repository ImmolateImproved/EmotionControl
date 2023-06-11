using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public TextMeshProUGUI[] howToPlayTooltip;

    public GameObject startLevelPanel;

    HashSet<string> done = new HashSet<string>(3);

    private void OnEnable()
    {
        PlanningStage.OnSingSpawnAndDestroy += PlanningStage_OnSingSpawnAndDestroy;
        PlanningStage.OnSingChangeDirection += PlanningStage_OnSingChangeDirection;
    }

    private void OnDisable()
    {
        PlanningStage.OnSingSpawnAndDestroy -= PlanningStage_OnSingSpawnAndDestroy;
        PlanningStage.OnSingChangeDirection -= PlanningStage_OnSingChangeDirection;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            startLevelPanel.SetActive(false);
    }

    private void PlanningStage_OnSingSpawnAndDestroy(Vector3Int node, bool isSpawn)
    {
        if (isSpawn)
        {
            if (done.Contains("spawn")) return;
            done.Add("spawn");

            howToPlayTooltip[0].DOFade(0, 1);
            howToPlayTooltip[1].DOFade(1, 1);

            startLevelPanel.SetActive(true);
        }
        else
        {
            if (done.Contains("remove")) return;
            done.Add("remove");

            howToPlayTooltip[2].DOFade(0, 1);
        }
    }

    private void PlanningStage_OnSingChangeDirection(DirectionSign directionSign)
    {
        if (done.Contains("direction")) return;
        done.Add("direction");

        if (directionSign == null) return;

        howToPlayTooltip[1].DOFade(0, 1);
        howToPlayTooltip[2].DOFade(1, 1);
    }
}