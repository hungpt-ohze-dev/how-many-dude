using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILevelCube : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private TMP_Text levelTxt;
    [SerializeField] private GameObject lockObj;
    [SerializeField] private GameObject hardObj;

    private LevelSave levelSave;
    private int level;

    public void Set(int level)
    {
        levelSave = DataManager.Save.Level;
        this.level = level;

        levelTxt.text = $"{level}";
        lockObj.SetActive(levelSave.levelId < level);
    }
}
