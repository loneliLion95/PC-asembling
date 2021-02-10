using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Таблица для отображения результата сборки ПК
/// </summary>
public class Table
{
    private GameObject _tableObj; 
    private GameObject _rowPrefab;
    private Dictionary<Types, Row> _rows;

    public Table()
    {
        _tableObj = GameObject.Find("Body");
        _rowPrefab = Resources.Load<GameObject>("Prefabs/Row");
        _rows = new Dictionary<Types, Row>();
    }

    /// <summary>
    /// Фактический объект таблицы на сцене
    /// </summary>
    public GameObject TableObj
    {
        get
        {
            return _tableObj;
        }
    }
    /// <summary>
    /// Префаб строки таблицы
    /// </summary>
    public GameObject RowPrefab
    {
        get
        {
            return _rowPrefab;
        }
    }
    /// <summary>
    /// Список-словарь всех строк таблицы
    /// </summary>
    public Dictionary<Types, Row> Rows
    {
        get
        {
            return _rows;
        }
    }
}
