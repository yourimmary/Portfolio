using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnum;

public class MonsterController : CharacterBase
{
    [SerializeField] GameObject _hpPrefab;
    [SerializeField] float _walkSpeed = 2;
    [SerializeField] float _chaseSpeed = 4;

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
    float _speed = 0;
    Vector2 _destination;
    MONSTERSTATE _state = MONSTERSTATE.IDLE;
    CHARACTERDIR _dir = CHARACTERDIR.DOWN;

    Animator _aniControl;
    PathFinding _pathFinding;
    public PathFinding PathFind { get { return _pathFinding; } }

    private void Update()
    {
        MonsterMove();

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

    void MonsterMove()
    {
        Vector2 monSight = transform.GetChild(1).GetChild((int)_dir).up;
        Vector2 pDir = (_player.position - transform.position).normalized;

        if (_isChase)
        {
            _speed = _chaseSpeed;

            if (_isQuitChase)
            {
                if (Vector2.Distance(transform.position, _player.position) >= _sightDis)
                {
                    _isQuitChase = false;
                    _isChase = false;
                    _state = MONSTERSTATE.IDLE;
                }
            }
            else
            {
                if (Vector2.Distance(transform.position, _player.position) < _sightDis)
                    _isQuitChase = true;
            }

            if (_state != MONSTERSTATE.ATTACK)
            {
                _pathFinding.FindPath(transform.position, _player.position);

                if (_pathFinding._path.Count != 0)
                {
                    if (Vector3.Distance(_pathFinding._path[0]._worldPosition, transform.position) > 0.1f)
                    {
                        Vector3 pathDir = (_pathFinding._path[0]._worldPosition - transform.position).normalized;

                        float minAngle = 360;
                        CHARACTERDIR minDir = CHARACTERDIR.UP;
                        for (int n = 0; n < 4; n++)
                        {
                            CHARACTERDIR cDir = (CHARACTERDIR)n;
                            float angle = Vector3.Angle(pathDir, GetDir(cDir));
                            if (minAngle > angle)
                            {
                                minAngle = angle;
                                minDir = cDir;
                            }
                        }
                        _dir = minDir;
                        Vector3 dir = GetDir(minDir);
                        transform.position += _speed * Time.deltaTime * dir;
                    }
                }

                ChangeMonsterAni(_state, _dir);
            }
        }
        else
        {
            _speed = _walkSpeed;

            if (Vector2.Distance(transform.position, _player.position) <= _sightDis &&
                Vector2.Dot(monSight, pDir) >= Mathf.Cos(_sightAngle * Mathf.Deg2Rad))
            {
                _findMark.Play();
                _isChase = true;
                _isQuitChase = true;
                _state = MONSTERSTATE.WALK;
            }
            else
            {
                //if (!IsCollisionEnterWall(transform.GetChild(1).GetChild((int)_dir), 0.3f))
                //{
                    switch (_state)
                    {
                        case MONSTERSTATE.IDLE:
                            if (_checkMoveTime <= 0)
                            {
                                do
                                {
                                    _state = MONSTERSTATE.WALK;
                                    _distance = Random.Range(1, _moveLength);
                                    _dir = (CHARACTERDIR)Random.Range(0, 4);
                                    _destination = transform.position + (Vector3)GetDir(_dir) * _distance;
                                } while (!GameManager.Instance.IsWalkableNodePos(_destination));
                                ChangeMonsterAni(_state, _dir);
                            }
                            _checkMoveTime -= Time.deltaTime;
                            break;
                        case MONSTERSTATE.WALK:
                            if (Vector2.Distance(transform.position, _destination) < 0.1f)
                            {
                                _state = MONSTERSTATE.IDLE;
                                _checkMoveTime = Random.Range(_minTimeNotMove, _maxtimeNotMove);
                                ChangeMonsterAni(_state, _dir);
                            }
                            else
                                transform.position += _speed * Time.deltaTime * new Vector3(GetDir(_dir).x, GetDir(_dir).y, 0);
                            break;
                    }
                //}
                //else
                //{
                //    _state = MONSTERSTATE.IDLE;
                //    _checkMoveTime = Random.Range(_minTimeNotMove, _maxtimeNotMove);
                //    ChangeMonsterAni(_state, _dir);
                //}
            }
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

        _pathFinding = GetComponent<PathFinding>();
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
