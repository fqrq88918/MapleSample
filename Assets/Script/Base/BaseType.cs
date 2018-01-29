
/// <summary>
/// 窗口类型
/// </summary>
public enum WindowType
{
    MainWin=1,
    MessageWindow=2,
    FloatMessageWindow=3,
}

/// <summary>
/// UI事件类型
/// </summary>
public enum UIEventType
{
    #region MessageWindow
    TypeOpenMessageWin=1,
    TypeShowFloatMessageWin=2,
    #endregion

    #region MainView 5-10
    TypeOpenMainView =5,    
    #endregion
}
public enum ResourceType
{
    Resource=1,
    AssetBundle=2,
}

public enum MessageWndType
{
    TypeConfirm=1,
    TypeOKAndCancle=2,
}