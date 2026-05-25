using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnum;

public class Map : MonoBehaviour
{
    // 맵 오브젝트 이름
    string _mapObjectName;

    // 맵 정보
    string _name;
    int _depth;
    bool _isClear;

    public bool IsClear { get { return _isClear; } }
    public int Depth { get { return _depth; } }
    public int JamCount { get { return _jamParent.childCount; } }
    public Gride MapGrid { get { return GetComponent<Gride>(); } }

    // 클리어를 위해 필요한 변수
    int _monsterNum;
    Vector3 _destination;
    Collider2D _collider;
    Transform _jamParent;
    List<MonsterController> _monList;

    [SerializeField] MAPCLEARTYPE _clearType;

    //null 방지용
    bool _isInit = false;

    bool IsInit()
    {
        if (!_isInit) Debug.Log("맵 정보가 초기화되지 않았습니다.");
        return _isInit;
    }

    //맵 초기화 함수
    public void InitMap(string name, int depth)
    {
        _monList = new List<MonsterController>();

        _mapObjectName = gameObject.name;
        _name = name;
        _depth = depth;
        _isClear = false;

        _monsterNum = transform.GetChild(1).childCount;
        _destination = transform.GetChild(3).position;
        _jamParent = transform.GetChild(4);

        _isInit = true;
    }

    public void CreateMonster()
    {
        GameObject mPrefab = Resources.Load("Characters/Monster1") as GameObject;

        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            Transform mark = UIManager.Instance.CreateFindMark();
            MonsterController mc = Instantiate(mPrefab, transform.GetChild(1).GetChild(i).position,
                Quaternion.identity).GetComponent<MonsterController>();

            mc.InitSet(GameManager.Instance.Player.transform, GameManager.Instance.MapSelectCount,
                GameManager.Instance.CurDepth, UIManager.Instance.HpBarParent, mark);
            mc.PathFind.GetGrid(GetComponent<Gride>());
            _monList.Add(mc);
        }
    }

    public void DecreaseMonsterCount()
    {
        _monsterNum -= 1;
    }

    public void ClearMonsterList()
    {
        for (int i = 0; i < _monList.Count; i++)
        {
            if (_monList[i] != null)
            {
                _monList[i].DestroyMonster();
                Destroy(_monList[i].gameObject);
            }
        }

        _monList.Clear();
    }

    private void Update()
    {
        if (IsInit())
        {
            switch (_clearType)
            {
                case MAPCLEARTYPE.AllMonsterDestroy:
                    if (_monsterNum == 0)
                        _isClear = true;
                    break;
                case MAPCLEARTYPE.ReachDestination:
                    _collider = Physics2D.OverlapCircle(_destination, 0.25f, LayerMask.GetMask("Player"));
                    if (_collider != null)
                    {
                        _isClear = true;
                        ClearMonsterList();
                    }
                    break;
                case MAPCLEARTYPE.GetherAllJam:
                    if (_jamParent.childCount == 0)
                    {
                        _isClear = true;
                        UIManager.Instance.CloseWindow(UIENUM.GETITEMWINDOW);
                        ClearMonsterList();
                    }
                    break;
            }
        }
    }
}
