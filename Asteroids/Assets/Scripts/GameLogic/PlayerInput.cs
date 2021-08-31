using System.Collections;
using System.Collections.Generic;

public enum Rotation {
    None,
    Left,
    Right
}

public class PlayerInput {
    #region Singleton
    private static PlayerInput instance;

    private PlayerInput()
    { }

    public static PlayerInput GetInstance()
    {
        if (instance == null)
            instance = new PlayerInput();
        return instance;
    }
    #endregion

    public Rotation Rotation { get; set; }
    public bool Acceleration { get; set; }
    public bool Shoot { get; set; }
    public bool LaserAttack { get; set; }
}
