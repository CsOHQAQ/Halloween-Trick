using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using QxFramework.Core;

public class EnergySlideUI : UIBase
{
    private Image cover;
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        cover = _gos["Cover"].GetComponent<Image>();

    }
    private void Update()
    {

        cover.fillAmount = EntityManager.Instance.CurEnergy / EntityManager.Instance.MaxEnergy;

    }
    protected override void OnClose()
    {
        base.OnClose();
    }
}
