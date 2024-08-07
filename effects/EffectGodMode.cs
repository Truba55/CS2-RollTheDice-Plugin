
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using Preach.CS2.Plugins.RollTheDiceV2.Core.BaseEffect;
using Preach.CS2.Plugins.RollTheDiceV2.Utilities;

namespace Preach.CS2.Plugins.RollTheDiceV2.Effects;

public class EffectGodMode : EffectBaseRegular, IEffectParameter, IEffectTimer
{
    public override bool Enabled { get; set; } = true;
    public override string PrettyName { get; set; } = "GodMode";
    public override string TranslationName { get; set; } = "godmode";
    public override double Probability { get; set; } = 1;
    public override bool ShowDescriptionOnRoll { get; set; } = false;
    public Dictionary<string, string> RawParameters { get; set; } = new();
    public Dictionary<IntPtr, CounterStrikeSharp.API.Modules.Timers.Timer> Timers { get; set; } = new();

    public override void Initialize()
    {
        RawParameters.Add("durationSeconds", "10");
    }

    public override void OnApply(CCSPlayerController? playerController)
    {
        if (playerController == null || playerController.PlayerPawn.Value is null)  return;

        if(Timers.ContainsKey(playerController!.Handle))
        {
            playerController.LogChat(GetEffectPrefix() + Log.GetLocalizedText("effect_already_applied"));
            return;
        }

        if(!RawParameters.TryGetValue("durationSeconds", out var durationStr))
            return;

        if(!float.TryParse(durationStr, out var durationFl))
            return;

        playerController!.PlayerPawn.Value.Health = (int)10e8;
        playerController.RefreshUI();
        PrintDescription(playerController, TranslationName, durationStr);

        var timerRef = Timers;
        StartTimer(ref timerRef, playerController, durationFl);
    }

    public override void OnRemove(CCSPlayerController? playerController)
    {
        if(Timers.TryGetValue(playerController!.Handle, out var timer))
        {
            timer.Kill();
            Timers.Remove(playerController!.Handle);
        }
    }

    public void OnTimerEnd(CCSPlayerController playerController)
    {
        if (playerController == null || playerController.PlayerPawn.Value is null)  return;

        if(!playerController!.IsValidPly() || !playerController!.IsAlive())
            return;

        var plyHealth = playerController!.PlayerPawn.Value.Health;
        var plyMaxHealth = playerController!.PlayerPawn.Value.MaxHealth;

        if(plyHealth > plyMaxHealth)
        {
            playerController!.PlayerPawn.Value.Health = plyMaxHealth;
        }

        playerController.LogChat(GetEffectPrefix() + Log.GetLocalizedText(Log.GetEffectLocale(TranslationName, "end")));
        playerController.RefreshUI();
    }

}