/// <summary>
/// Типы комплектующих
/// </summary>
public enum Types
{
    MotherBoard,
    CPU,
    VC,
    Storage,
    RAM
}

/// <summary>
/// Типы ПК
/// </summary>
public enum PCTypes
{
    Gaming,
    Office
}

/// <summary>
/// Родительский класс всех комплектующих
/// </summary>
public abstract class PartOfPC
{
    private string _name;
    private Types _type;
    private PCTypes _pcType;

    public string Name 
    {
        get 
        { 
            return _name; 
        } 
        set 
        { 
            _name = value; 
        } 
    }
    public Types Type
    {
        get
        {
            return _type;
        }
        set
        {
            _type = value;
        }
    }
    public PCTypes PcType 
    { 
        get 
        { 
            return _pcType; 
        } 
        set 
        { 
            _pcType = value; 
        } 
    }
}
