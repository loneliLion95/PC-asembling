using UnityEngine.UI;

/// <summary>
/// Класс строки таблицы с полями, содержащими текстовые данные её ячеек
/// </summary>
public class Row
{
    private Text _name;
    private Text _type;
    private Text _pcType;
    private Text _socket;
    private Text _ramType;
    private Text _vcInterface;
    private Text _storageInterface;
    private Text _pointsCell;
    private int _points = 0;

    public Text Name
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
    public Text Type
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
    public Text PcType
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
    public Text Socket
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
    public Text RamType
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
    public Text VcInterface
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
    public Text StorageInterface
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
    public Text PointsCell
    {
        get
        {
            return _pointsCell;
        }
        set
        {
            _pointsCell = value;
        }
    }
    /// <summary>
    /// Числовое представление заработанных очков
    /// </summary>
    public int Points
    {
        get
        {
            return _points;
        }
        set
        {
            _points = value;
            _pointsCell.text = value.ToString();
        }
    }
}
