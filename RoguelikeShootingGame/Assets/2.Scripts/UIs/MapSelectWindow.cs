using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelectWindow : MonoBehaviour
{
    [SerializeField] GameObject _subWindow;
    [SerializeField] bool _isExtra; //юс╫ц

    List<SubMapSelectWindow> _subWndList;
    List<int> _mapIndexs;

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
            //carTrans.GetComponent<Animation>().Play();
            StartCoroutine(EachCardAnimation(carTrans, i));

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void InitWindow()
    {
        _subWndList = new List<SubMapSelectWindow>();
        _mapIndexs = new List<int>();

        //for (int i = 1; i <= 10; i++)
        //{
        //    SubMapSelectWindow smw = 
        //        Instantiate(_subWindow, transform.GetChild(0).GetChild(0)).GetComponent<SubMapSelectWindow>();
        //    smw.InitSet(i);
        //    _subWndList.Add(smw);
        //}
    }

    public void OpenWindow()
    {
        if (_isExtra)
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
                SubMapSelectWindow smw = Instantiate(_subWindow, transform.GetChild(0).GetChild(1)).GetComponent<SubMapSelectWindow>();
                smw.InitSet(_mapIndexs[i]);
                smw.SetDistance(GameManager.Instance.SetDistance());
                _subWndList.Add(smw);
            }

            gameObject.SetActive(true);

            StartCoroutine(MapCardsAnimation());
        }
        else
        {
            _selectCount = Random.Range(2, 6);

            int cnt = 0;

            while (cnt < _selectCount)
            {
                int idx = Random.Range(0, 10);
                if (!_mapIndexs.Contains(idx))
                {
                    _mapIndexs.Add(idx);
                    cnt++;
                }
            }

            for (int i = 0; i < _selectCount; i++)
            {
                //_subWndList[_mapIndexs[i]].gameObject.SetActive(true);
                //_subWndList[_mapIndexs[i]].SetDistance(GameManager.Instance.SetDistance());
                SubMapSelectWindow smw = Instantiate(_subWindow, transform.GetChild(0).GetChild(0)).GetComponent<SubMapSelectWindow>();
                smw.InitSet(_mapIndexs[i]);
                smw.SetDistance(GameManager.Instance.SetDistance());
                _subWndList.Add(smw);
            }

            gameObject.SetActive(true);
        }
    }

    public void CloseWindow()
    {
        if (_isExtra)
        {
        }
        else
        {
            for (int i = 0; i < _subWndList.Count; i++)
                Destroy(_subWndList[i].gameObject);
            //_subWndList[_mapIndexs[i]].gameObject.SetActive(false);
            _subWndList.Clear();
            _mapIndexs.Clear();
            gameObject.SetActive(false);
        }
    }
}
