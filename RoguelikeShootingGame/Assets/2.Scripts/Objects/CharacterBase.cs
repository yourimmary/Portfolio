using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class CharacterBase : MonoBehaviour
{
    protected int _level;
    protected int _maxhp;
    protected int _hp;
    protected int _att;
    protected int _def;
    protected float _cri;
    protected float _criDmg;

    public float _hpRate { get { return (float)_hp / _maxhp; } }
    public int Def { get { return _def; } }

    protected void InitStatus(int hp, int att, int def, int level = 1)
    {
        _maxhp = _hp = hp;
        _att = att;
        _def = def;
        _cri = 10;
        _criDmg = 50;

        _level = level;
    }

    protected bool IsCollisionEnterWall(Transform originTrans, float length)
    {
        bool isEnter = false;
        Vector2 origin = transform.position + originTrans.localPosition + originTrans.up * length;
        RaycastHit2D rHit = Physics2D.Raycast(origin, transform.up * -1, length, LayerMask.GetMask("Wall"));
        Debug.DrawRay(origin, originTrans.up * length, Color.red);
        if (rHit.collider != null && rHit.transform.CompareTag("Wall")) isEnter = true;
        return isEnter;
    }

    abstract public int CalculateDamage(int enermyDef);

    abstract public void OnHitting(int damage);
}
