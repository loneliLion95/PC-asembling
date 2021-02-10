/// <summary>
/// Видеокарта
/// </summary>
public class VideoCard : PartOfPC
{
    private string _vcInterface;

    public VideoCard(string name, Types type, PCTypes pcType, string vcInterface)
    {
        Name = name;
        Type = type;
        PcType = pcType;
        VcInterface = vcInterface;
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
}
