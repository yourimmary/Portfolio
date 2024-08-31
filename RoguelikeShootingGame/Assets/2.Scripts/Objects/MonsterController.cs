using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnum;

public class MonsterController : CharacterBase
{
    [SerializeField] float _speed = 2;

    Transform _player;

    [Header("MonsterMove")]
    [SerializeField] float _minTimeNotMove = 0.5f;
    [SerializeField] float _maxtimeNotMove = 2;
    [SerializeField] float _moveLength = 1.5f;
    [SerializeField] float _sightDis = 3;
    [SerializeField] float _sightAngle = 30;

    [Header("Base Status")]
    [SerializeField] int _baseHp;
    [SerializeField] int _baseAtt;
    [SerializeField] int _baseDef;
    [SerializeField] int _baseLevel;
    [SerializeField] int _giveExp = 20;

    bool _isChase = false;
    float _checkMoveTime = 0;
    float _distance = 0;
    Vector2 _destination;
    MONSTERSTATE _state = MONSTERSTATE.IDLE;
    CHARACTERDIR _dir = CHARACTERDIR.DOWN;

    Animator _aniControl;

    private void Update()
    {
        if (_state == MONSTERSTATE.WALK)
        {
            if (!IsCollisionEnterWall(transform.GetChild(1).GetChild((int)_dir), 0.3f))
            {
                Vector2 monSight = transform.GetChild(1).GetChild((int)_dir).up;
                Vector2 pDir = (_player.position - transform.position).normalized;
                if (Vector2.Distance(transform.position, _player.position) <= _sightDis)
                {
                    if (Vector2.Dot(monSight, pDir) >= Mathf.Cos(_sightAngle))
                    {
                        _isChase = true;
                    }

                    if (_isChase)
                    {
                        Vector2 xAxis = new Vector2(pDir.x, 0).normalized;
                        Vector2 yAxis = new Vector2(0, pDir.y).normalized;

                        if (Vector2.Dot(GetDir(_dir), xAxis) > Vector2.Dot(GetDir(_dir), yAxis))
                        {
                            _dir = (xAxis.x < 0) ? CHARACTERDIR.LEFT : CHARACTERDIR.RIGHT;
                            transform.position += _speed * Time.deltaTime * (Vector3)xAxis;
                        }
                        else
                        {
                            _dir = (yAxis.y < 0) ? CHARACTERDIR.DOWN : CHARACTERDIR.UP;
                            transform.position += _speed * Time.deltaTime * (Vector3)yAxis;
                        }
                    }
                }
                else
                {
                    if (_isChase)
                    {
                        _isChase = false;
                        _state = MONSTERSTATE.IDLE;
                        _checkMoveTime = Random.Range(_minTimeNotMove, _maxtimeNotMove);
                    }
                    else
                    {
                        if (Vector2.Distance(transform.position, _destination) < 0.05f)
                        {
                            _state = MONSTERSTATE.IDLE;
                            _checkMoveTime = Random.Range(_minTimeNotMove, _maxtimeNotMove);
                        }
                        transform.position += _speed * Time.deltaTime * new Vector3(GetDir(_dir).x, GetDir(_dir).y, 0);
                    }
                }
            }
            else
            {
                _state = MONSTERSTATE.IDLE;
                _checkMoveTime = Random.Range(_minTimeNotMove, _maxtimeNotMove);
            }
        }
        else
        {
            if (_state == MONSTERSTATE.IDLE)
            {
                if (_checkMoveTime <= 0)
                {
                    _state = MONSTERSTATE.WALK;
                    _distance = Random.Range(1, _moveLength);
                    _dir = (CHARACTERDIR)Random.Range(0, 4);
                    _destination = transform.position + (Vector3)GetDir(_dir) * _distance;
                }
                _checkMoveTime -= Time.deltaTime;
            }
        }
        ChangeMonsterAni(_state, _dir);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _state = MONSTERSTATE.ATTACK;
            ChangeMonsterAni(_state, _dir);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _state = MONSTERSTATE.WALK;
            ChangeMonsterAni(_state, _dir);
        }
    }

    public void InitSet(Transform playerTrans, int mapCnt, int curDepth)
    {
        _player = playerTrans;
        _baseHp += Mathf.RoundToInt(_baseHp * 0.1f * (mapCnt + curDepth / 100));
        InitStatus(_baseHp, _baseAtt, _baseDef, _baseLevel);
        _checkMoveTime = Random.Range(_minTimeNotMove, _maxtimeNotMove);
        _aniControl = GetComponent<Animator>();
    }

    int GetGiveExpPerLevel()
    {
        int exp = _giveExp;
        exp += Mathf.RoundToInt(_giveExp * 0.5f * _level);
        return exp;
    }

    Vector2 GetDir(CHARACTERDIR dir)
    {
        switch(dir)
        {
            case CHARACTERDIR.UP:
                return Vector2.up;
            case CHARACTERDIR.DOWN:
                return Vector2.down;
            case CHARACTERDIR.LEFT:
                return Vector2.left;
            case CHARACTERDIR.RIGHT:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }

    void ChangeMonsterAni(MONSTERSTATE state, CHARACTERDIR dir)
    {
        _aniControl.SetInteger("State", (int)state);
        _aniControl.SetInteger("Dir", (int)dir);
    }

    void GiveDamagePlayer()
    {
        PlayerController pc = _player.GetComponent<PlayerController>();
        pc.OnHitting(CalculateDamage(pc.Def));
    }

    public override void OnHitting(int damage)
    {
        _hp -= damage;
        _hp -= (int)(damage * (EnhanceUtility.GetDegree(ENHANCETYPE.ADDITIONALATTACK) / 100));
        if (_hp <= 0)
        {
            _state = MONSTERSTATE.DEATH;
            GameManager.Instance.DecreaseMonsterCount();
            _player.GetComponent<PlayerController>().GetExp(GetGiveExpPerLevel());
            Destroy(gameObject);
        }
    }

    public override int CalculateDamage(int enermyDef)
    {
        int damage = _att;
        int ran = Random.Range(1, 101);
        if (ran <= _cri)
            damage += Mathf.RoundToInt(_att * (_criDmg / 100.0f));
        damage -= (int)((enermyDef + (int)EnhanceUtility.GetDegree(ENHANCETYPE.DEFUP)) * 
            ((1f + EnhanceUtility.GetDegree(ENHANCETYPE.DEFUPPERCENT)) / 100));
        damage = (damage < 1) ? 1 : damage;
        return damage;
    }
}
