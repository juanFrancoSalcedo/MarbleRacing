using Newronizer;
using Newronizer.SceneLoader;

public class ButtonQuickApplication: BaseButtonAttendant
{
    private void Start() => buttonComponent.onClick.AddListener(()=>SceneLoader.Instance.Quit());
}
