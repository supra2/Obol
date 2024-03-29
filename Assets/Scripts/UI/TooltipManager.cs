using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : Singleton<TooltipManager>
{
    #region Members
    [SerializeField]
    protected Tooltip _tooltipReference;
    #endregion

    #regionĘPublic Method

    public void ShowTooltip(string content , Vector2 position)
    {
        _tooltipReference.SetContent( content );
        _tooltipReference.SetPosition(position );
            _tooltipReference.Show();
    }

    public void HideTooltip()
    {
        _tooltipReference.Hide();
    }

    #endregion
}
