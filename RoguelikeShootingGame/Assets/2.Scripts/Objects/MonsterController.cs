using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnum;

public class MonsterController : CharacterBase
{
    [SerializeField] GameObject _hpPrefab;
    [SerializeField] float _speed = 2;

    Transform _player;
    MonsterHpBar _hpBar;
    Animation _findMark;

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
    bool _isQuitChase = false;
    float _checkMoveTime = 0;
    float _distance = 0;
    Vector2 _destination;
    MONSTERSTATE _state = MONSTERSTATE.IDLE;
    CHARACTERDIR _dir = CHARACTERDIR.DOWN;

    Animator _aniControl;

    private void Update()
    {
        ExMonterMoveFuction();

        if (_hpBar != null)
            _hpBar.SetPosition(transform.position + Vector3.up);
        if (_findMark != null)
            _findMark.transform.position = transform.position + new Vector3(-0.7f, 1, 0);

        if (GameManager.Instance.GameState == GAMESTATE.GAMERESULT)
            Destroy(gameObject);
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

    void MonterMoveFuction()
    {
        if (_state == MONSTERSTATE.WALK)
        {
            if (!IsCollisionEnterWall(transform.GetChild(1).GetChild((int)_dir), 0.3f))
            {
                Vector2 monSight = transform.GetChild(1).GetChild((int)_dir).up;
                Vector2 pDir = (_player.position - transform.position).normalized;
                if (Vector2.Distance(transform.position, _player.position) <= _sightDis)
                {
                    if (Vector2.Dot(monSight, pDir) >= Mathf.Cos(_sightAngle * Mathf.Deg2Rad))
                    {
                        if (!_isChase)
                            _findMark.Play();
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
                        ChangeMonsterAni(_state, _dir);
                    }
                    else
                    {
                        if (Vector2.Distance(transform.position, _destination) < 0.1f)
                        {
                            _state = MONSTERSTATE.IDLE;
                            _checkMoveTime = Random.Range(_minTimeNotMove, _maxtimeNotMove);
                            ChangeMonsterAni(_state, _dir);
                        }
                        transform.position += _speed * Time.deltaTime * new Vector3(GetDir(_dir).x, GetDir(_dir).y, 0);
                    }
                }
                else
                {
                    if (_isChase)
                    {
                        _isChase = false;
                        _state = MONSTERSTATE.IDLE;
                        _checkMoveTime = Random.Range(_minTimeNotMove, _maxtimeNotMove);
                        ChangeMonsterAni(_state, _dir);
                    }
                    else
                    {
                        if (Vector2.Distance(transform.position, _destination) < 0.1f)
                        {
                            _state = MONSTERSTATE.IDLE;
                            _checkMoveTime = Random.Range(_minTimeNotMove, _maxtimeNotMove);
                            ChangeMonsterAni(_state, _dir);
                        }
                        transform.position += _speed * Time.deltaTime * new Vector3(GetDir(_dir).x, GetDir(_dir).y, 0);
                    }
                }
            }
            else
            {
                _state = MONSTERSTATE.IDLE;
                _checkMoveTime = Random.Range(_minTimeNotMove, _maxtimeNotMove);
                ChangeMonsterAni(_state, _dir);
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
                    ChangeMonsterAni(_state, _dir);
                }
                _checkMoveTime -= Time.deltaTime;
            }
        }
    }

    void ExMonterMoveFuction()
    {
        switch (_state)
        {
            case MONSTERSTATE.IDLE:
                if (_checkMoveTime <= 0)
                {
                    _state = MONSTERSTATE.WALK;
                    _distance = Random.Range(1, _moveLength);
                    _dir = (CHARACTERDIR)Random.Range(0, 4);
                    _destination = transform.position + (Vector3)GetDir(_dir) * _distance;
                    ChangeMonsterAni(_state, _dir);
                }
                _checkMoveTime -= Time.deltaTime;
                break;
            case MONSTERSTATE.WALK:
                Vector2 monSight = transform.GetChild(1).GetChild((int)_dir).up;
                Vector2 pDir = (_player.position - transform.position).normalized;
                
                if (!IsCollisionEnterWall(transform.GetChild(1).GetChild((int)_dir), 0.3f))
                {
                    if (_isChase)
                    {
                        if (_isQuitChase && Vector2.Distance(transform.position, _player.position) > _sightDis)
                        {
                            _isChase = false;
                            _isQuitChase = false;
                            _state = MONSTERSTATE.IDLE;
                        }
                        else
                        {
                            if (Vector2.Distance(transform.position, _player.position) <= _sightDis)
                                _isQuitChase = true;

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
                            ChangeMonsterAni(_state, _dir);
                        }
                    }
                    else
                    {
                        float dis = Vector2.Distance(transform.position, _player.position);
                        if (dis <= _sightDis)
                        {
                            if (Vector2.Dot(monSight, pDir) >= Mathf.Cos(_sightAngle * Mathf.Deg2Rad))
                            {
                                //RaycastHit2D rHit = Physics2D.Raycast(transform.position, pDir, dis);
                                //if (rHit.collider != null && rHit.collider.CompareTag("Player"))
                                //{
                                //    _findMark.Play();
                                //    _isChase = true;
                                //    _isQuitChase = true;
                                //}
                                _findMark.Play();
                                _isChase = true;
                                _isQuitChase = true;
                            }
                        }

                        if (Vector2.Distance(transform.position, _destination) < 0.1f)
                        {
                            _state = MONSTERSTATE.IDLE;
                            _checkMoveTime = Random.Range(_minTimeNotMove, _maxtimeNotMove);
                            ChangeMonsterAni(_state, _dir);
                        }
                        else
                            transform.position += _speed * Time.deltaTime * new Vector3(GetDir(_dir).x, GetDir(_dir).y, 0);
                    }
                }
                else
                {
                    _state = MONSTERSTATE.IDLE;
                    _checkMoveTime = Random.Range(_minTimeNotMove, _maxtimeNotMove);
                    ChangeMonsterAni(_state, _dir);
                }
                break;
        }
    }

    public void InitSet(Transform playerTrans, int mapCnt, int curDepth, Transform hpBar, Transform mark)
    {
        _player = playerTrans;
        _baseHp += Mathf.RoundToInt(_baseHp * 0.1f * (mapCnt + curDepth / 100));
        InitStatus(_baseHp, _baseAtt, _baseDef, _baseLevel);

        _hpBar = Instantiate(_hpPrefab, hpBar).GetComponent<MonsterHpBar>();
        _hpBar.InitSet();

        _findMark = mark.GetComponent<Animation>();

        _checkMoveTime = Random.Range(_minTimeNotMove, _maxtimeNotMove);
        _aniControl = GetComponent<Animator>();
    }
    
    public override void OnHitting(int damage)
    {
        if (_hp > 0)
        {
            if (!_isChase)
            {
                _findMark.Play();
                _isChase = true;
                _state = MONSTERSTATE.WALK;
            }

            if (_hp > damage)
            {
                _hp -= damage;
                _hpBar.SetFillHpBar(HpRate);
            }
            else if (_hp <= damage)
            {
                _hp = 0;
                _hpBar.SetFillHpBar(HpRate);
                _state = MONSTERSTATE.DEATH;
                GameManager.Instance.DecreaseMonsterCount();
                _player.GetComponent<PlayerController>().GetExp(GetGiveExpPerLevel());
                Destroy(_findMark.gameObject);
                Destroy(_hpBar.gameObject);
                Destroy(gameObject);
            }
            
            Vector3 pos = Vector3.up + transform.position;
            int angle = Random.Range(-90, 91);
            float len = Random.Range(0.5f, 1);
            float x = Mathf.Cos(angle * Mathf.Rad2Deg);
            float y = Mathf.Cos(angle * Mathf.Rad2Deg);
            Vector3 exPos = new Vector3(x, y, 0) * len;
            UIManager.Instance.CreateDamageWindow(damage, pos + exPos);
        }
    }

    public override int CalculateDamage(int enermyDef, float attPercent = 1)
    {
        int damage = (int)(_att * attPercent);
        int ran = Random.Range(1, 101);
        if (ran <= _cri)
            damage += (int)(_att * (_criDmg / 100.0f));
        damage -= enermyDef;
        damage = (damage < 1) ? 1 : damage;
        return damage;
    }
}
