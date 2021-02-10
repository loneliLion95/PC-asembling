/// <summary>
/// Накопитель
/// </summary>
public class StorageDevice : PartOfPC
{
    private string _storageInterface;

    public StorageDevice(string name, Types type, PCTypes pcType, string storageInterface)
    {
        Name = name;
        Type = type;
        PcType = pcType;
        StorageInterface = storageInterface;
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
