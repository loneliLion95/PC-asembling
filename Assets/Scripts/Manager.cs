using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Главный класс, управляющий всеми операциями в программе
/// </summary>
public class Manager : MonoBehaviour
{
    /// <summary>
    /// Таблица формата .csv с информацией о комплектующих
    /// </summary>
    private TextAsset _sheetData;
    /// <summary>
    /// Контейнер, содержащий текст задания
    /// </summary>
    public Text taskText;
    /// <summary>
    /// Тип ПК, соответствующий выбранному заданию
    /// </summary>
    private PCTypes _selectedTask;
    /// <summary>
    /// Список всех комплектующих
    /// </summary>
    private List<PartOfPC> _partsOfPC = new List<PartOfPC>();
    /// <summary>
    /// Префаб объекта-кнопки, копия которого ассоциируется с определённым комплектующим
    /// и появляется на панели _allPartsPanel
    /// </summary>
    private GameObject _partPrefab;
    /// <summary>
    /// Панель, на которой появляются все комплектующие выбранного типа
    /// </summary>
    private GameObject _allPartsPanel;
    /// <summary>
    /// Индекс предыдущего выбранного типа комплектующего
    /// </summary>
    private int _prevTypeIndex;
    /// <summary>
    /// Флаг, определяющий выбран ли какой-либо тип комплектующего
    /// </summary>
    private bool _typeIsSelected;
    /// <summary>
    /// Список объектов, отображающихся при выборе определённого типа комплектующего
    /// </summary>
    private List<GameObject> _allPartsOfType = new List<GameObject>();
    /// <summary>
    /// Префаб объекта, копия которого ассоциируется с выбранным комплектующим 
    /// и появляется на панели _selectedPartsPanel
    /// </summary>
    private GameObject _selectedPartPrefab;
    /// <summary>
    /// Панель, на которой появляются все выбранные комплектующие
    /// </summary>
    private GameObject _selectedPartsPanel;
    /// <summary>
    /// Список-словарь объектов, ассоциирующихся с выбранными комплектующими
    /// </summary>
    private Dictionary<Types, GameObject> _selectedPartsDic = new Dictionary<Types, GameObject>();
    /// <summary>
    /// Делегат для методов-обработчиков события нажатия на кнопки, генерируемые в реальном времени.
    /// Необходим для передачи этих методов в качестве параметров
    /// </summary>
    /// <param name="data">Данные текущего события</param>
    /// <param name="partType">Тип выбранного комплектующего</param>
    public delegate void OnPointerDownDelegate(PointerEventData data, Types partType);

    void Start()
    {
        _sheetData = Resources.Load<TextAsset>("PartsOfPC");
        _partPrefab = Resources.Load<GameObject>("Prefabs/Part");
        _allPartsPanel = GameObject.Find("All parts");
        _selectedPartPrefab = Resources.Load<GameObject>("Prefabs/SelectedPart");
        _selectedPartsPanel = GameObject.Find("Selected parts");

        ReadTheSheet(_sheetData);
    }

    /// <summary>
    /// Метод для прочтения информации из .csv таблицы и занесения её в созданные классы соответствующих комплектующих
    /// </summary>
    /// <param name="sheetData">Таблица формата .csv с информацией о комплектующих</param>
    public void ReadTheSheet(TextAsset sheetData)
    {
        string[] data = sheetData.text.Split('\n');

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(',');
            string name = row[0];
            Types type = (Types)Enum.Parse(typeof(Types), row[1], true);
            PCTypes pcType = (PCTypes)Enum.Parse(typeof(PCTypes), row[2], true);
            string socket = row[3];
            string ramType = row[4];
            string vcInterface = row[5];
            string storageInterface = row[6];
            PartOfPC tempPart = null;

            switch (type)
            {
                case Types.MotherBoard:
                    tempPart = new MotherBoard(name, type, pcType, socket, ramType, vcInterface, storageInterface);
                    break;
                case Types.CPU:
                    tempPart = new CPU(name, type, pcType, socket);
                    break;
                case Types.VC:
                    tempPart = new VideoCard(name, type, pcType, vcInterface);
                    break;
                case Types.Storage:
                    tempPart = new StorageDevice(name, type, pcType, storageInterface);
                    break;
                case Types.RAM:
                    tempPart = new RAM(name, type, pcType, ramType);
                    break;
            }

            if (tempPart != null)
            {
                _partsOfPC.Add(tempPart);
            }
        }
    }

    /// <summary>
    /// Метод-обработчик события нажатия на кнопку выбора задания
    /// </summary>
    /// <param name="taskIndex">Индекс, необходимый для определения типа ПК</param>
    public void ChooseTheTaskHandler(int taskIndex)
    {
        _selectedTask = (PCTypes)taskIndex;

        switch (_selectedTask)
        {
            case PCTypes.Gaming:
                taskText.text = Resources.Load<TextAsset>("TaskTexts/GamingPC").text;
                break;
            case PCTypes.Office:
                taskText.text = Resources.Load<TextAsset>("TaskTexts/OfficePC").text;
                break;
        }

        GameObject.Find("ChooseTheTask").SetActive(false);
    }

    /// <summary>
    /// Метод-обработчик события нажатия на кнопку выбора типа комплектующего.
    /// Генерирует копии префаба _partPrefab на панели _allPartsPanel
    /// </summary>
    /// <param name="typeIndex">Индекс выбранного типа комплектующего</param>
    public void ChooseTheTypeHandler(int typeIndex)
    {
        if (_prevTypeIndex != typeIndex || !_typeIsSelected)
        {
            if (_typeIsSelected)
            {
                foreach (var part in _allPartsOfType)
                {
                    Destroy(part);
                }
                _allPartsOfType.Clear();
            }
            Types selectedType = (Types)typeIndex;
            foreach (PartOfPC part in _partsOfPC)
            {
                if (part.Type == selectedType)
                {
                    GameObject partClone = Instantiate(_partPrefab);
                    partClone.GetComponentInChildren<Text>().text = part.Name;
                    partClone.transform.SetParent(_allPartsPanel.transform, false);
                    AddEventTriggerBehavior(partClone, selectedType, SelectPartOnPointerDown);
                    _allPartsOfType.Add(partClone);
                }
            }
            _prevTypeIndex = typeIndex;
            _typeIsSelected = true;
        }
    }

    /// <summary>
    /// Метод-обработчик события выбора определённого комплектующего.
    /// Переносит выбранное комплектущее на панель _selectedPartsPanel
    /// </summary>
    /// <param name="data">Данные текущего события</param>
    /// <param name="selPartType">Тип выбранного комплектующего</param>
    public void SelectPartOnPointerDown(PointerEventData data, Types selPartType)
    {
        // Произойдёт, только если не будет выбранного комплектующего с таким же типом
        if (!_selectedPartsDic.ContainsKey(selPartType))
        {
            string partName = data.selectedObject.GetComponentInChildren<Text>().text;
            GameObject selectedPartClone = Instantiate(_selectedPartPrefab);
            _selectedPartsDic.Add(selPartType, selectedPartClone);
            selectedPartClone.GetComponentInChildren<Text>().text = partName;
            selectedPartClone.transform.SetParent(_selectedPartsPanel.transform, false);
            GameObject deletePartButton = selectedPartClone.GetComponentInChildren<Button>().gameObject;
            AddEventTriggerBehavior(deletePartButton, selPartType, DeletePartOnPointerDown);
        }
    }

    /// <summary>
    /// Метод-обработчик события нажатия на кнопку удаления выбранного комплектующего.
    /// </summary>
    /// <param name="data">Данные текущего события</param>
    /// <param name="delPartType">Тип удаляемого комплектующего</param>
    public void DeletePartOnPointerDown(PointerEventData data, Types delPartType)
    {
        Destroy(_selectedPartsDic[delPartType]);
        _selectedPartsDic.Remove(delPartType);
    }

    /// <summary>
    /// Метод добавления обработчика события нажатия на кнопку, генерируемую в реальном времени
    /// </summary>
    /// <param name="objWithEventTrigger">Кнопка с компонентом EventTrigger</param>
    /// <param name="partType">Тип комплектующего</param>
    /// <param name="onPointerDown">Метод-обработчик события нажатия на кнопку</param>
    public void AddEventTriggerBehavior(GameObject objWithEventTrigger, Types partType, OnPointerDownDelegate onPointerDown)
    {
        EventTrigger trigger = objWithEventTrigger.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { onPointerDown((PointerEventData)data, partType); });
        trigger.triggers.Add(entry);
    }

    /// <summary>
    /// Метод оценки сборки ПК соответственно заданию и соответствию комплектующих к материнской плате
    /// </summary>
    public void Evaluate()
    {
        int numberOfTypes = Enum.GetNames(typeof(Types)).Length;
        int maxPoints = numberOfTypes + (numberOfTypes - 1);
        int earnedPoints = 0;
        int finalGrade;

        List<PartOfPC> selectedParts = new List<PartOfPC>();

        foreach (var selPart in _selectedPartsDic.Values)
        {
            foreach (var part in _partsOfPC)
            {
                if (part.Name == selPart.GetComponentInChildren<Text>().text)
                {
                    selectedParts.Add(part);
                }
            }
        }

        Table table = FillTheTable(selectedParts);
        MotherBoard mB = new MotherBoard();

        foreach (var part in selectedParts)
        {
            if (part.PcType == _selectedTask)
            {
                earnedPoints++;
                table.Rows[part.Type].Points++;
                table.Rows[part.Type].PcType.color = Color.green;
            }

            switch (part.Type)
            {
                case Types.MotherBoard:
                    mB = part as MotherBoard;
                    break;
                case Types.CPU:
                    CPU cPU = part as CPU;
                    if (cPU.Socket == mB.Socket)
                    {
                        earnedPoints++;
                        table.Rows[part.Type].Points++;
                        table.Rows[part.Type].Socket.color = Color.green;
                    }
                    break;
                case Types.VC:
                    VideoCard vC = part as VideoCard;
                    if (vC.VcInterface == mB.VcInterface)
                    {
                        earnedPoints++;
                        table.Rows[part.Type].Points++;
                        table.Rows[part.Type].VcInterface.color = Color.green;
                    }
                    break;
                case Types.Storage:
                    StorageDevice storage = part as StorageDevice;
                    if (storage.StorageInterface == mB.StorageInterface)
                    {
                        earnedPoints++;
                        table.Rows[part.Type].Points++;
                        table.Rows[part.Type].StorageInterface.color = Color.green;
                    }
                    break;
                case Types.RAM:
                    RAM rAM = part as RAM;
                    if (rAM.RamType == mB.RamType)
                    {
                        earnedPoints++;
                        table.Rows[part.Type].Points++;
                        table.Rows[part.Type].RamType.color = Color.green;
                    }
                    break;
            }
        }
        
        finalGrade = Mathf.RoundToInt((float)earnedPoints / maxPoints * 10);
        Text finalGradeText = GameObject.Find("finalGrade").GetComponent<Text>();

        finalGradeText.text = string.Format($"Получено {earnedPoints} баллов из {maxPoints} возможных\n\n" +
                                            $"Итоговая оценка: ");
        if (finalGrade > 8)
            finalGradeText.text += string.Format($"<color=green>{finalGrade}</color>");
        else if (finalGrade > 6)
            finalGradeText.text += string.Format($"<color=orange>{finalGrade}</color>");
        else
            finalGradeText.text += string.Format($"<color=red>{finalGrade}</color>");

        GameObject.Find("SolveTheTask").SetActive(false);
    }

    /// <summary>
    /// Метод наполнения таблицы информацией, взятой из списка выбранных комплектующих
    /// </summary>
    /// <param name="selectedParts">Список выбранных комплектующих</param>
    /// <returns>Ссылка на заполенную таблицу</returns>
    public Table FillTheTable(List<PartOfPC> selectedParts)
    {
        Table table = new Table();
        foreach (var part in selectedParts)
        {
            GameObject rowClone = Instantiate(table.RowPrefab);
            rowClone.transform.SetParent(table.TableObj.transform, false);
            Row row = new Row();
            table.Rows.Add(part.Type, row);

            foreach (Text cell in rowClone.GetComponentsInChildren<Text>())
            {
                switch (cell.gameObject.name)
                {
                    case "Name":
                        row.Name = cell;
                        break;
                    case "Type":
                        row.Type = cell;
                        break;
                    case "PCType":
                        row.PcType = cell;
                        break;
                    case "Socket":
                        row.Socket = cell;
                        break;
                    case "RamType":
                        row.RamType = cell;
                        break;
                    case "VcInterface":
                        row.VcInterface = cell;
                        break;
                    case "StorageInterface":
                        row.StorageInterface = cell;
                        break;
                    case "Points":
                        row.PointsCell = cell;
                        break;
                }
            }

            row.Name.text = part.Name;
            row.Type.text = part.Type.ToString();
            row.PcType.text = part.PcType.ToString();
            row.PcType.color = Color.red;

            if (part is MotherBoard)
            {
                MotherBoard mB = part as MotherBoard;
                row.Socket.text = mB.Socket;
                row.RamType.text = mB.RamType;
                row.VcInterface.text = mB.VcInterface;
                row.StorageInterface.text = mB.StorageInterface;
            }
            if (part is CPU)
            {
                row.Socket.text = (part as CPU).Socket;
                row.Socket.color = Color.red;
            }
            if (part is VideoCard)
            {
                row.VcInterface.text = (part as VideoCard).VcInterface;
                row.VcInterface.color = Color.red;
            }
            if (part is StorageDevice)
            {
                row.StorageInterface.text = (part as StorageDevice).StorageInterface;
                row.StorageInterface.color = Color.red;
            }
            if (part is RAM)
            {
                row.RamType.text = (part as RAM).RamType;
                row.RamType.color = Color.red;
            }
        }

        return table;
    }
}
