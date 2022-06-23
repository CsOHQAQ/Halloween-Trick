
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Common;
using QxFramework.Core;
public class MainDataManager : LogicModuleBase, IMainDataManager
{
    public MainData _mainData;
    public override void Init()
    {
        base.Init();
        if (!RegisterData(out _mainData))
        {
            Debug.Log("!!!!!"+_mainData);
            InitMainData();
        }
    }

    public override void Update()
    {
        base.Update();
    }

    private void InitMainData()
    {
        _mainData.DataNumber = ReadFromCSV();
    }

    private string ReadFromCSV()
    {
        return Data.Instance.TableAgent.GetString("Test","1","Value");
    }

    public void LoadFrom()
    {
        Data.Instance.LoadFromFile("FileName.json");
        Debug.Log(_mainData.GetHashCode());
        _mainData.tsd = new TestSaveData();
        _mainData.tsd.vect = new Vector2(1, 14);
    }
    public void SaveTo()
    {
        Data.Instance.SaveToFile("FileName.json");
        Debug.Log(_mainData.GetHashCode());
        _mainData.tsd = null;
    }

    public void RefreshNum(string Num)
    {
        _mainData.DataNumber = Num;
    }
    public string DisplayNum()
    {
        return _mainData.DataNumber;
    }
}
public class MainData : GameDataBase
{
    public string DataNumber;
    public TestSaveData tsd;

}

public class TestSaveData
{
    public Vector2 vect;
}
