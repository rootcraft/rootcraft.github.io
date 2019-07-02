using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorManager_v3 : MonoBehaviour
{
    #region variables
    private int rowCount = 3;   //default
    private int maxRowCount = 8;    //maximum
    private int minRowCount = 3;    //minimum  // bu durumda default ile ayni, kullanilmayablirdi.

    private int coloumCount = 3;    //default
    private int maxColoumCount = 8; //maximum
    private int minColoumCount = 3; //minimum  // bu durumda default ile ayni, kullanilmayabilirdi.

    private float gridCellSize = 1500;  //grid cell size'i icin referans

    public GridLayoutGroup gridLayout;
    public GameObject prefab;   //olusturulacak 'round' prefabi
    public int maxSize = 150;
    private List<GameObject> createdRoundList = new List<GameObject>(); //Pooling icin kullanilacak
     
    RectTransform first, last;
    float paddingLeft; 
    float paddingTop;
    #endregion

    #region MonoMethods
    void Start()
    {
        GetScreenSize();
        //float paddingR = Screen.width - Screen.width / 10;
        //float paddingB = Screen.height - Screen.height / 5;
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
        int i=0;
        first = createdRoundList[i].GetComponent<RectTransform>();
        for (; i < rowCount * coloumCount; i++)
        {
            createdRoundList[i].SetActive(true);
        }
        last = createdRoundList[i-1].GetComponent<RectTransform>();

        for (; i < maxColoumCount * maxRowCount; i++)
        {
            createdRoundList[i].SetActive(false); 
        }

    }
     
    void UpdateConstraint()
    { 
        gridLayout.constraintCount = coloumCount;
        //gridLayout.cellSize = Vector2.one * (gridCellSize / coloumCount); 
    }

    /// <summary>
    /// Grid'ler ekranin yatayina sigiyor mu?
    /// </summary>
    private void CheckXSize()
    { 
        float _fX = first.position.x - first.sizeDelta.x / 2;

        if (_fX < paddingLeft)
        {
            StopAllCoroutines();
            StartCoroutine(DecreaseWidth());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(IncreaseWidth());
        }
         
    }
    
    /// <summary>
    /// Grid'ler ekranin dikeyine sigiyor mu?
    /// </summary>
    private void CheckYSize()
    { 
        float _lY = last.position.y - last.sizeDelta.y / 2;

        if (_lY < paddingTop)
        {
            StopAllCoroutines();
            StartCoroutine(DecreaseHeight());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(IncreaseHeight());
        }
         
    } 
    #endregion
     
    #region Coroutines 
    IEnumerator DecreaseWidth() {
        float _fX;
        while (true)
        { 
             _fX = first.position.x - first.sizeDelta.x / 2;

            if (_fX < paddingLeft)
            {
                gridLayout.cellSize -= Vector2.one * 1f;
                yield return null;
            }
            else
            { break; }
        }

    }
    IEnumerator IncreaseWidth()
    {
        float _fX; 
        float _lY;
        while (true)
        { 
             _fX = first.position.x - first.sizeDelta.x / 2;
            _lY = last.position.y - last.sizeDelta.y / 2;

            if (_fX > paddingLeft && _lY > paddingTop && gridLayout.cellSize.x < maxSize)
            {
                gridLayout.cellSize += Vector2.one * 1f;
                yield return null;
            }
            else
            { break; }
        }

    }

    IEnumerator DecreaseHeight() {

        float _lY;
        while (true)
        {
             _lY = last.position.y - last.sizeDelta.y / 2;

            if (_lY < paddingTop)
            {
                gridLayout.cellSize -= Vector2.one * 1f;
                yield return null;
            }
            else
            { break; }
            }

    }
    IEnumerator IncreaseHeight()
    {
        float _fX;
        float _lY;
        while (true)
        {
            _fX = first.position.x - first.sizeDelta.x / 2;
            _lY = last.position.y - last.sizeDelta.y / 2;

            if (_lY > paddingTop && _fX > paddingLeft && gridLayout.cellSize.x < maxSize)
            {
                gridLayout.cellSize += Vector2.one * 1f;
                yield return null;
            }
            else
            { break; }
        }

    } 
    #endregion
     
    #region Public Methods

    public void GetScreenSize()
    {
        paddingLeft = Screen.width / 10;
        paddingTop = Screen.height / 10;
    }
  
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
            Invoke("CheckYSize", 0.1f);
                
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
            Invoke("CheckYSize", 0.1f);
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
        UpdateConstraint();
        UpdateView();
        Invoke("CheckXSize", 0.1f);
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
            UpdateConstraint();
            UpdateView();
            Invoke("CheckXSize", 0.1f);
        }

        //Debug.Log(GetComponent<RectTransform>().position.x);
        //Debug.LogWarning(Screen.width);
    }
    #endregion

}
