/// <summary>
/// 唤出答题界面处理器
/// </summary>
class CallQuestionPanelCommand : Controller
{
    public override void Execute(object data)
    {
        UIAnswer m_UIAnswer = GetView<UIAnswer>();
        UIBoard m_UIBoard = GetView<UIBoard>();

        m_UIBoard.PauseGame(true);
        m_UIBoard.HideBoardItem();

        m_UIAnswer.CallSelf();
    }
}

