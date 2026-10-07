using Fusion;

// Published only by state authority after the guarded lethal transition.
public readonly struct KillConfirmedRecord
{
    public readonly PlayerRef Killer, Victim;
    public readonly int KillerTeam, VictimTeam, DeathSequence, ShotSequence;
    public readonly bool Headshot;
    public KillConfirmedRecord(PlayerRef killer, PlayerRef victim, int killerTeam, int victimTeam,
        bool headshot, int deathSequence, int shotSequence)
    {
        Killer = killer; Victim = victim; KillerTeam = killerTeam; VictimTeam = victimTeam;
        Headshot = headshot; DeathSequence = deathSequence; ShotSequence = shotSequence;
    }
}
