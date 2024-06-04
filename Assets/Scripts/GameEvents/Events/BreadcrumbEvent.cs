using UnityEngine;

[CreateAssetMenu(fileName = "BreadcrumbEvent", menuName = "GameEvents/BreadcrumbEvent")]
public class BreadcrumbEvent : GameEvent
{    
    public override string GetName()
    {
        return "Breadcrumb Event!";
    }

    public override void OnEnter(GameEventsManager mgr)
    {
        mgr.Mika.StartBreadcrumbEvent();
    }
}
