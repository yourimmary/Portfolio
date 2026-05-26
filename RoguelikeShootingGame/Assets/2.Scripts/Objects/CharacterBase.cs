using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnum;

abstract public class CharacterBase : MonoBehaviour
{
    protected int _level;
    protected int _maxhp;
    protected int _hp;
    protected int _att;
    protected int _def;
    protected float _cri;
    protected float _criDmg;

    public float HpRate { get { return (float)_hp / _maxhp; } }
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

    abstract public int CalculateDamage(int enermyDef, float attPercent = 1);

    abstract public void OnHitting(int damage);
}
