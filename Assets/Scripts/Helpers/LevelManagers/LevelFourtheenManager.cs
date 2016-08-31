using UnityEngine;
using System.Collections;

public class LevelFourtheenManager : ILevel {

    public GameObject[] objectToDisable;

    public override void InitLevel()
    {
        base.InitLevel();
    }

    public override void EndLevel()
    {
        base.EndLevel();
        for (int i = 0; i < objectToDisable.Length; i++)
        {
            objectToDisable[i].SetActive(false);
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void UpdateLevel()
    {

    }


    #region Difficulty setting
    protected override void PrepareEasyLevel()
    {
        base.PrepareEasyLevel();
    }
    protected override void PrepareMediumLevel()
    {
        base.PrepareMediumLevel();
    }
    protected override void PrepareHardLevel()
    {
        base.PrepareHardLevel();
    }
    #endregion Difficulty setting
}
