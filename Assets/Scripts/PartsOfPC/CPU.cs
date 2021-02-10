/// <summary>
/// Центральный процессор
/// </summary>
public class CPU : PartOfPC
{
    private string _socket;

    public CPU(string name, Types type, PCTypes pcType, string socket)
    {
        Name = name;
        Type = type;
        PcType = pcType;
        Socket = socket;
    }

    public string Socket
    {
        get
        {
            return _socket;
        }
        set
        {
            _socket = value;
        }
    }
}
