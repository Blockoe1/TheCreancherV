/// <summary>
/// Stores types of actions
/// </summary>
[System.Serializable]
public abstract class DiceAction
{
    protected int faceValue;

    public abstract int PriorityValue { get; }
    public abstract void PerformAction(Combatant);

    public int FaceValue
    {
        get { return faceValue; }
        set {  faceValue = value; }
    }

    //public enum ActionTypes
    //{
    //    ATTACK,
    //    HEAL,
    //    POSION,
    //    CORRUPTION
    //}
    //public Action(ActionTypes type, int value)
    //{
    //    Type = type;
    //    Value = value;
    //}

    //public ActionTypes Type;
    //public int Value;
}
