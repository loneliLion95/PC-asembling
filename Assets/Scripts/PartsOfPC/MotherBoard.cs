/// <summary>
/// Материнская плата
/// </summary>
public class MotherBoard : PartOfPC
{
    private string _socket, _ramType, _vcInterface, _storageInterface;

    public MotherBoard() { }
    public MotherBoard(string name, Types type, PCTypes pcType, string socket, string ramType, string vcInterface, string storageInterface)
    {
        Name = name;
        Type = type;
        PcType = pcType;
        Socket = socket;
        RamType = ramType;
        VcInterface = vcInterface;
        StorageInterface = storageInterface;
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
    public string VcInterface
    {
        get
        {
            return _vcInterface;
        }
        set
        {
            _vcInterface = value;
        }
    }
    public string StorageInterface
    {
        get
        {
            return _storageInterface;
        }
        set
        {
            _storageInterface = value;
        }
    }
}
