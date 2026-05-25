using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnum;

public class PlayerController : CharacterBase
{
    int _maxExp;
    int _exp;
    bool _isAttack = false;
    float _checkTime = 0;
    float _stopTime = 0;

    [SerializeField] float _speed = 3.0f;
    [SerializeField] float _arrowRate = 0.2f;
    [SerializeField] GameObject _arrowPrefab;
    [SerializeField] Transform _arrowSpawn;

    Animator _aniControl;
    PlayerWindow _pWnd;
    PLAYERSTATE _state = PLAYERSTATE.IDLE;
    CHARACTERDIR _dir = CHARACTERDIR.DOWN;

    public float PosY { get { return transform.position.y; } }
    public CHARACTERDIR DIR { get { return _dir; } }

    private void Update()
    {
        if (GameManager.Instance.GameState == GAMESTATE.GAMEPLAY && _state != PLAYERSTATE.HIT)
        {
            ChangePlayerAni(_state, _dir);

            if (Input.GetMouseButtonDown(0))
            {
                SoundManager.Instance.sfxPlay(SOUNDENUM.ArrowDraw, 0.2f);
                _state = PLAYERSTATE.ATTACKREADY;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                SoundManager.Instance.sfxPlay(SOUNDENUM.ArrowRelease, 0.2f);
                _state = PLAYERSTATE.ATTACK;
                _isAttack = true;
                GameObject go = Instantiate(_arrowPrefab,
                    _arrowSpawn.GetChild((int)_dir).position, _arrowSpawn.GetChild((int)_dir).rotation);
                go.GetComponent<ShootArrow>().InitSet(this);
                go.GetComponent<ShootArrow>().Shoot(_arrowSpawn.GetChild((int)_dir).up);
                _aniControl.SetTrigger("Shoot");
                _checkTime = _arrowRate;
            }
        }

        if (_checkTime <= 0)
        {
            _isAttack = false;
        }
        else
            _checkTime -= Time.deltaTime;

        if (_state == PLAYERSTATE.HIT)
        {
            if (_stopTime <= 0)
            {
                _state = PLAYERSTATE.IDLE;
            }
            _stopTime -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        //여기선 이동만
        if (GameManager.Instance.GameState == GAMESTATE.GAMEPLAY && _state != PLAYERSTATE.HIT)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            Vector3 dir = Vector3.zero;

            if (x != 0)
            {
                dir = new Vector3(x, 0, 0);
                _dir = (x > 0) ? CHARACTERDIR.RIGHT : CHARACTERDIR.LEFT;
            }
            else if (y != 0)
            {
                dir = new Vector3(0, y, 0);
                _dir = (y > 0) ? CHARACTERDIR.UP : CHARACTERDIR.DOWN;
            }

            if (_state != PLAYERSTATE.ATTACKREADY && !_isAttack)
            {
                Vector3 plusPos = _speed * Time.fixedDeltaTime * dir.normalized;
                if (dir.magnitude > 0 && GameManager.Instance.IsWalkableNodePos(transform.position + plusPos) &&
                    IsTriggerMonster(transform.GetChild(2).GetChild((int)_dir), 0.3f))
                {
                    _state = PLAYERSTATE.WALK;
                    transform.position += plusPos;
                    GameManager.Instance.MC.Follow(transform.position);
                }
                else
                    _state = PLAYERSTATE.IDLE;
            }
            ChangePlayerAni(_state, _dir);
        }
    }

    public void InitSet(PlayerWindow wnd)
    {
        InitStatus(300, 30, 20);
        _exp = 0;
        _maxExp = 10;

        _pWnd = wnd;
        _pWnd.InitSet();
        _pWnd.SetValue(HpRate, _maxhp, _hp);

        _state = PLAYERSTATE.IDLE;
        _dir = CHARACTERDIR.DOWN;
        _aniControl = GetComponent<Animator>();
        ChangePlayerAni(_state, _dir);
        Debug.LogFormat("Level : {0}\nMaxExp : {1}\nExp : {2}\nATT : {3}\nDEF : {4}\nCRI : {5}\nCRIDMG : {6}\nHP : {7}",
            _level, _maxExp, _exp, _att, _def, _cri, _criDmg, _hp);
    }

    public void GetExp(int exp)
    {
        _exp += exp;
        if (_exp >= _maxExp)
        {
            _level++;
            _exp -= _maxExp;
            _maxExp += Mathf.RoundToInt(_maxExp * 1.5f);
            IncreasePlayerState(2, 2, 3);
            _pWnd.SetValue(HpRate, _maxhp, _hp);
            Debug.LogFormat("Level : {0}\nMaxExp : {1}\nExp : {2}\nATT : {3}\nDEF : {4}\nCRI : {5}\nCRIDMG : {6}\nHP : {7}",
                _level, _maxExp, _exp, _att, _def, _cri, _criDmg, _hp);
        }
    }

    void ChangePlayerAni(PLAYERSTATE state, CHARACTERDIR dir)
    {
        _aniControl.SetInteger("State", (int)state);
        _aniControl.SetInteger("Dir", (int)dir);
        if (state == PLAYERSTATE.DEATH) _aniControl.SetTrigger("Death");
    }

    void IncreasePlayerState(int iAtt, int iDef, int ihp)
    {
        _maxhp += ihp;
        _hp += ihp;
        _att += iAtt;
        _def += iDef;
    }

    bool IsTriggerMonster(Transform originTrans, float length)
    {
        bool isEnter = true;
        RaycastHit2D rHit = Physics2D.Raycast(originTrans.position, transform.up/* * -1*/,
            length, LayerMask.GetMask("Monster"));
        Debug.DrawRay(originTrans.position, originTrans.up * length, Color.red);
        if (rHit.collider != null)
            isEnter = false;

        return isEnter;
    }

    void ChangeHitMotion()
    {
        _state = PLAYERSTATE.HIT;
        _stopTime = 0.3f;
        ChangePlayerAni(_state, _dir);
    }

    public void IncreasePlayerStatus(ENHANCETYPE type, float value)
    {
        switch (type)
        {
            case ENHANCETYPE.ATTUP:
                _att += (int)value;
                break;
            case ENHANCETYPE.DEFUP:
                _def += (int)value;
                break;
            case ENHANCETYPE.HPUP:
                _maxhp += (int)value;
                _hp += (int)value;
                break;
            case ENHANCETYPE.ATTUPPERCENT:
                int addAtk = (int)(_att * value / 100);
                _att += addAtk;
                break;
            case ENHANCETYPE.DEFUPPERCENT:
                int addDef = (int)(_def * value / 100);
                _def += addDef;
                break;
            case ENHANCETYPE.HPUPPERCENT:
                int addHp = (int)(_maxhp * value / 100);
                _maxhp += addHp;
                _hp += addHp;
                break;
            case ENHANCETYPE.CRITICALUP:
                _cri += value;
                break;
            case ENHANCETYPE.CRITICALDAMAGEUP:
                _criDmg += value;
                break;
        }
        _pWnd.SetValue(HpRate, _maxhp, _hp);
    }

    public void InitStateAndDir()
    {
        _state = PLAYERSTATE.IDLE;
        _dir = CHARACTERDIR.DOWN;
        ChangePlayerAni(_state, _dir);
    }

    public override void OnHitting(int damage)
    {
        _hp -= damage;
        _pWnd.SetValue(HpRate, _maxhp, _hp);
        _state = PLAYERSTATE.HIT;
        _aniControl.SetTrigger("Hit");
        if (gameObject.activeInHierarchy)
            ChangeHitMotion();

        Debug.Log(_hp);
        if (_hp <= 0)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            _state = PLAYERSTATE.DEATH;
            ChangePlayerAni(_state, _dir);
            GameManager.Instance.GameEnd(false);
        }
    }

    public override int CalculateDamage(int enermyDef, float attPercent = 1)
    {
        int damage = (int)(_att * attPercent);
        int ran = Random.Range(1, 101);
        if (ran <= _cri)
            damage += (int)(damage * (_criDmg / 100));
        damage -= enermyDef;
        damage = (damage < 1) ? 1 : damage;
        return damage;
    }
}
