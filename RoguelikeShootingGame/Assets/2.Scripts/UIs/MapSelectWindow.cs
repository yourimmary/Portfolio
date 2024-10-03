using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelectWindow : MonoBehaviour
{
    [SerializeField] GameObject _subWindow;

    List<SubMapSelectWindow> _subWndList;
    List<int> _mapIndexs;

    const int WIDTH = 1920;
    int _selectCount = 0;

    IEnumerator EachCardAnimation(Transform trans, int index)
    {
        trans.GetComponent<Animation>().Play();

        yield return new WaitForSeconds(0.2f);

        transform.GetChild(0).GetChild(1).GetChild(index).GetComponent<Animation>().Play();
    }

    IEnumerator MapCardsAnimation()
    {
        for (int i = 0; i < _mapIndexs.Count; i++)
        {
            Transform carTrans = transform.GetChild(0).GetChild(0).GetChild(_mapIndexs[i]);
            StartCoroutine(EachCardAnimation(carTrans, i));

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void InitWindow()
    {
        _subWndList = new List<SubMapSelectWindow>();
        _mapIndexs = new List<int>();
    }

    public void OpenWindow()
    {
        _selectCount = Random.Range(2, 6);

        int cnt = 0;

        while (cnt < _selectCount)
        {
            int idx = Random.Range(0, 10);
            if (!_mapIndexs.Contains(idx))
            {
                _mapIndexs.Add(idx);
                Debug.Log(idx);
                cnt++;
            }
        }

        for (int i = 0; i < _selectCount; i++)
        {
            float x = (WIDTH / _selectCount * i) - (WIDTH / 2) + WIDTH / (_selectCount * 2);
            SubMapSelectWindow smw = Instantiate(_subWindow, transform.GetChild(0).GetChild(1)).GetComponent<SubMapSelectWindow>();
            RectTransform trans = smw.GetComponent<RectTransform>();
            trans.anchoredPosition = new Vector2(x, 0);
            smw.InitSet(_mapIndexs[i]);
            smw.SetDistance(GameManager.Instance.SetDistance());
            _subWndList.Add(smw);
        }

        gameObject.SetActive(true);

        StartCoroutine(MapCardsAnimation());
    }

    public void CloseWindow()
    {
        for (int i = 0; i < _subWndList.Count; i++)
            Destroy(_subWndList[i].gameObject);
        _subWndList.Clear();

        for (int i = 0; i < transform.GetChild(0).GetChild(0).childCount; i++)
        {
            RectTransform rect = transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetComponent<RectTransform>();
            rect.anchoredPosition = Vector2.zero;
        }
        _mapIndexs.Clear();
        gameObject.SetActive(false);
    }
}
