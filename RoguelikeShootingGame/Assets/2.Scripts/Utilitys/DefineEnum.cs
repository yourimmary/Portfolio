namespace DefineEnum
{
    public enum PLAYERSTATE
    {
        IDLE,
        WALK,
        ATTACKREADY,
        ATTACK,
        DEATH = 99
    }

    public enum MONSTERSTATE
    {
        IDLE,
        WALK,
        ATTACK,

        DEATH = 99
    }

    public enum CHARACTERDIR
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public enum GAMESTATE
    {
        GAMETITLE,
        GAMEINIT,
        GAMEREADY,
        GAMESTART,
        GAMEPLAY,
        GAMEEND,
        GAMERESULT,
    }

    public enum ENHANCETYPE
    {
        ATTUP,
        DEFUP,
        HPUP,
        ATTUPPERCENT,
        DEFUPPERCENT,
        HPUPPERCENT,
        CRITICALUP,
        CRITICALDAMAGEUP,
        ADDITIONALATTACK,
        MAX
    }

    public enum UIENUM
    {
        MAPSELECTWINDOW,
        ENHANCESELECTWINDOW,
        PLAYERWINDOW,
        RESULTWINDOW,
        TITLEWINDOW
    }
}
