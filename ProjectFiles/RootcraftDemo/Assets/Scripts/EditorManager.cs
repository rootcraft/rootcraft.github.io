﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorManager : MonoBehaviour
{
    #region variables
    private int rowCount = 3;   //default
    private int maxRowCount = 6;    //maximum
    private int minRowCount = 3;    //minimum  // bu durumda default ile ayni, kullanilmayablirdi.

    private int coloumCount = 3;    //default
    private int maxColoumCount = 6; //maximum
    private int minColoumCount = 3; //minimum  // bu durumda default ile ayni, kullanilmayabilirdi.

    private float gridCellSize = 1500;  //grid cell size'i icin referans

    public GridLayoutGroup gridLayout;
    public GameObject prefab;   //olusturulacak 'round' prefabi

    private List<GameObject> createdRoundList = new List<GameObject>(); //Pooling icin kullanilacak

    #endregion

    #region MonoMethods
    void Start()
    {
        Pool();
    }

    #endregion

    #region Private Methods
    /// <summary>
    /// Maximum olusacak daire sayisi kadar daire yaratip disable et.
    /// Pooliling
    /// </summary>
    private void Pool()
    {
        for (int i = 0; i < maxColoumCount * maxRowCount; i++)
        {
            GameObject temp = Instantiate(prefab, gridLayout.transform);
            createdRoundList.Add(temp);
        }
        UpdateView();
    }

    /// <summary>
    /// Row veya coloum sayisi degisirse grid elemanlarini güncelle
    /// </summary>
    private void UpdateView()
    {
        int i;
        for (i = 0; i < rowCount * coloumCount; i++)
        {
            createdRoundList[i].SetActive(true);
        }

        for (; i < maxColoumCount * maxRowCount; i++)
        {
            createdRoundList[i].SetActive(false); 
        }

    }

    /// <summary>
    /// Grid'i ekrana sigacak sekilde ayarlamak icin mat islemleri
    /// </summary>
    void UpdateCellSize()
    {
        gridLayout.constraintCount = coloumCount;
        gridLayout.cellSize = Vector2.one * (gridCellSize / coloumCount);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Row sayisini artir
    /// Grid'i guncelle
    /// </summary>
    public void IncreaseRow()
    {
        if (rowCount < maxRowCount)
        {
            rowCount++;
            UpdateView();
        }
    }
    /// <summary>
    /// Row sayisini azalt
    /// Grid'i guncelle
    /// </summary>
    public void DecreaseRow()
    {
        if (rowCount > minRowCount)
        {
            rowCount--;
            UpdateView();
        }
    }
    /// <summary>
    /// Coluom sayisini artir
    /// Grid'i guncelle
    /// </summary>
    public void IncreaseColoum()
    {
        if (coloumCount < maxColoumCount) { 
        coloumCount++;
        UpdateCellSize();
        UpdateView();
        }
    }
    /// <summary>
    /// Coloum sayisini azalt
    /// Grid'i guncelle
    /// </summary>
    public void DecreaseColoum()
    {
        if (coloumCount > minColoumCount)
        {
            coloumCount--;
            UpdateCellSize();
            UpdateView();
        }
    }
    #endregion

}
