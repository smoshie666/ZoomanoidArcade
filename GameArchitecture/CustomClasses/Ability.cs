using ScriptableObjectArchitecture;
using System.Collections.Generic;
using UnityEngine;


public enum AbilityType
{
    Shooter,
    Extender,
    SlowBall,
    ExtraLife,
    Catcher,
    ScoreBonus,
    MultiBall,
    Flying
}

public enum AbilityMode
{
    Timed,      // Run instantly for a duration, then expire
    Triggered,  // Wait for some in-game event (collision, button, etc.)
    Passive     // Constant effect while active (e.g., passive score boost)
}

[System.Serializable]
public class Ability
{
    public Sprite transformation;
    public List<Shooter> shooters = new List<Shooter>();
    public Ball[] _balls;

    private int _unlockedCannons;

    public void BallsCheck()
    {

        if (_balls == null)
            return;

        if (_balls.Length > 0)
        {
            AddBalls();        
        }
    
    }

    public void AddBalls()
    {
        //add balls to controller
        //add 1, 2 or 3
        for (int i = 0; i < _balls.Length; i++)
        {
          
        }
    }

   

    public void ShooterCheck()
    {

        if (shooters == null)
            return;
        
        if (shooters.Count > 0)
        AddShooter(_unlockedCannons); //could make this a bool method to be checked in ability manager whic then calls add shooter??
        
    }
    public void AddShooter(int numberofguns)
    {
        if (shooters == null)
            return;

        if (shooters.Count > 0)
            _unlockedCannons = numberofguns;
    }


    public void OnShoot()
    {
        if (shooters == null)
            return;
        if (shooters.Count > 0)
        {
            for (int i = 0; i < shooters.Count; i++)
            {
                var shooter = shooters[i];
                shooter.Shoot();
            }
        }
    }
}
