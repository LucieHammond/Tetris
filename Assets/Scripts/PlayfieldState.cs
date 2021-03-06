﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldState : MonoBehaviour {

	public GameObject[][] playGrid { get; set; }
	public FlashingLine[] flashingLines;
	public AudioSource lineCompletedSound;
	public static PlayfieldState instance;

	private LevelDesign levels;
	private Gameplay gameplay;
	private bool isRestored;

	private void Awake()
	{
		instance = this;
	}

	void Start () {
		levels = GetComponent<LevelDesign>();
		gameplay = GetComponent<Gameplay>();
		lineCompletedSound.volume = Settings.soundsVolume;

        if (!isRestored)
		{
			playGrid = new GameObject[22][];
            for (int i = 0; i < playGrid.Length; i++)
            {
                playGrid[i] = new GameObject[10];
            }
		}
	}

	public IEnumerator OnLanding(bool usedSoftDrop)
    {
		if (usedSoftDrop)
			levels.IncreaseScore();
        List<int> lines = CheckLines();
        if (lines.Count > 0)
        {
            foreach (int line in lines)
                flashingLines[line].Flash();
            levels.NewLines(lines);
			lineCompletedSound.Play();
            yield return new WaitForSeconds(0.95f);
            RemoveLines(lines);
        }
		if (CheckGameOver())
			gameplay.LaunchGameOver();
		else
            gameplay.SpawnTetrimino();
    }
	
	private List<int> CheckLines()
    {
        List<int> completedLines = new List<int>();
        for (int i = 0; i < playGrid.Length; i++)
        {
            bool completed = true;
            for (int j = 0; j < playGrid[i].Length; j++)
            {
                if (!playGrid[i][j])
                {
                    completed = false;
                    break;
                }
            }
            if (completed)
                completedLines.Add(i);
        }
        return completedLines;
    }

    private void RemoveLines(List<int> lines)
    {
        int offset = 0;
        for (int i = 0; i < playGrid.Length; i++)
        {
            if (lines.Contains(i))
            {
                for (int j = 0; j < playGrid[i].Length; j++)
                {
                    Destroy(playGrid[i][j]);
                }
                offset++;
            }
            else
            {
                for (int j = 0; j < playGrid[i].Length; j++)
                {
                    if (offset > 0 && playGrid[i][j])
                        playGrid[i][j].transform.Translate(0, -offset, 0, Space.World);
                    playGrid[i - offset][j] = playGrid[i][j];
                }
            }
        }
    }

    private bool CheckGameOver()
	{
		for (int j = 0; j < playGrid[20].Length; j++)
		{
			if (playGrid[20][j])
				return true;
		}
		return false;
	}

	public void Restore(GameObject[][] pPlayGrid)
	{
		playGrid = pPlayGrid;
		isRestored = true;
	}
}
