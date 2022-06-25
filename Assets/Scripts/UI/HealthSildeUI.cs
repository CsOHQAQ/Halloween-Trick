using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
using UnityEngine.UI;
public class HealthSildeUI : UIBase
{
    public Entity ent;
    private Image cover;
    public override void OnDisplay(object args)
    {
        base.OnDisplay(args);
        cover = _gos["Cover"].GetComponent<Image>();
    }
    private void Update()
    {
        if (ent != null)
        {
            transform.position = Camera.main.WorldToScreenPoint((Vector2)ent.transform.position + new Vector2(0, 1));
            cover.fillAmount = ent.CurHealth / ent.MaxHealth;

        }
        else
        {
            UIManager.Instance.Close(this);

        }
    }
}
