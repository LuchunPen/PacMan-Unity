public class PersonaSpeed
{
    private float _ghostNormalSpeed;
    private float _ghostFrightSpeed;
    private float _ghostTunnelSpeed;

    private float _pacmanNormalSpeed;
    private float _pacmanFrightSpeed;
    private float _pacmanNormalDotaSpeed;
    private float _pacmanFrightDotaSpeed;

    public float GhostSpeedNormal
    {
        get { return _ghostNormalSpeed; }
    }
    public float GhostSpeedFright
    {
        get { return _ghostFrightSpeed; }
    }
    public float GhostTunnelSpeed
    {
        get { return _ghostTunnelSpeed; }
    }

    public float PacManSpeedNormal
    {
        get { return _pacmanNormalSpeed; }
    }
    public float PacManSpeedFright
    {
        get { return _pacmanFrightSpeed; }
    }
    public float PacManSpeedDotaNormal
    {
        get { return _pacmanNormalDotaSpeed; }
    }
    public float PacManSpeedDotaFright
    {
        get { return _pacmanFrightDotaSpeed; }
    }

    public PersonaSpeed(float ghNormalSpeed, float ghFrightSpeed, float ghTunnelSpeed, float pmNormalSpeed, float pmFrightSpeed, float pmNormalDota, float pmFrightDota)
    {
        _ghostNormalSpeed = ghNormalSpeed;
        _ghostFrightSpeed = ghFrightSpeed;
        _ghostTunnelSpeed = ghTunnelSpeed;
        _pacmanNormalSpeed = pmNormalSpeed;
        _pacmanFrightSpeed = pmFrightSpeed;
        _pacmanNormalDotaSpeed = pmNormalDota;
        _pacmanFrightDotaSpeed = pmFrightDota;
    }
}
public static class ClassicLevelTable
{
    private static PersonaSpeed[] SpeedTable = new PersonaSpeed[]
    {
        new PersonaSpeed(0.75f, 0.5f, 0.4f, 0.8f, 0.9f, 0.71f, 0.79f),
        new PersonaSpeed(0.85f, 0.55f, 0.45f, 0.9f, 0.95f, 0.79f, 0.83f),
        new PersonaSpeed(0.85f, 0.6f, 0.5f, 1.0f, 1.0f, 0.87f, 0.87f),
        new PersonaSpeed(0.95f, 0.95f, 0.5f, 0.9f, 0.9f, 0.79f, 0.79f)
    };

    public static PersonaSpeed GetSpeedTable(int level)
    {
        if (level <= 1) return SpeedTable[0];
        else if (level >= 2 && level <= 4) return SpeedTable[1];
        else if (level >= 5 && level <= 20) return SpeedTable[2];
        else return SpeedTable[3];
    }
}
