/// <summary>
/// Оперативная память
/// </summary>
public class RAM : PartOfPC
{
    private string _ramType;

    public RAM(string name, Types type, PCTypes pcType, string ramType)
    {
        Name = name;
        Type = type;
        PcType = pcType;
        RamType = ramType;
    }

    public string RamType
    {
        get
        {
            return _ramType;
        }
        set
        {
            _ramType = value;
        }
    }
}
