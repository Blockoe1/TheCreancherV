/// <summary>
/// Stores types of actions
/// </summary>
public struct Action
{
    public enum ActionTypes
    {
        ATTACK,
        HEAL,
        POSION,
        CORRUPTION
    }
    public Action(ActionTypes type, int value)
    {
        Type = type;
        Value = value;
    }

    public ActionTypes Type;
    public int Value;
}
