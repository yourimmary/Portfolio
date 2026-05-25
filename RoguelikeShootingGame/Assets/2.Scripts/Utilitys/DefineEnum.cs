namespace DefineEnum
{
    public enum SOUNDENUM
    {
        TitleBGM,           //아직 못 정함
        GamePlayBGM,        //아직 못 정함

        UIButtonClick,
        SubMapClick,
        SubEnhanceClick,    //아직 못 정함

        PickUpJam,          //아직 못 정함

        ArrowDraw,
        ArrowRelease,

        MonsterArrowHit,
        PlayerHit           //아직 못 정함
    }

    public enum MAPCLEARTYPE
    {
        AllMonsterDestroy,
        ReachDestination,
        GetherAllJam
    }

    public enum PLAYERSTATE
    {
        IDLE,
        WALK,
        ATTACKREADY,
        ATTACK,
        HIT,
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
        GAMERESULT
    }

    public enum ENHANCEGRADE
    {
        GOLD,
        SILVER,
        BRONZE
    }

    public enum ENHANCETYPE
    {
        ATTUP,
        ATTUPPERCENT,
        DEFUP,
        DEFUPPERCENT,
        HPUP,
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
        GETITEMWINDOW,
        RESULTWINDOW,
        TITLEWINDOW
    }
}
