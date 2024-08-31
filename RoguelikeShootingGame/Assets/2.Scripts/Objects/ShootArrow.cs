using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnum;

public class ShootArrow : MonoBehaviour
{
    Rigidbody2D _rb;
    PlayerController _player;
    [SerializeField] float _power = 200;

    public void InitSet(PlayerController pc)
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = pc;
    }

    public void Shoot(Vector2 dir)
    {
        _rb.AddForce(dir * _power);
    }
    public void Shoot(Vector3 dir)
    {
        Vector2 exDir = new Vector2(dir.x, dir.y);
        _rb.AddForce(exDir * _power);
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GAMESTATE.GAMEREADY)
            Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
            Destroy(gameObject);
        else if (collision.CompareTag("Monster"))
        {
            MonsterController mc = collision.GetComponent<MonsterController>();
            mc.OnHitting(_player.CalculateDamage(mc.Def));
            Destroy(gameObject);
        }
    }
}
