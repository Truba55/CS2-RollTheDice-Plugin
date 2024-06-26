using CounterStrikeSharp.API.Core;
using Preach.CS2.Plugins.RollTheDiceV2.Core.BaseEffect;
using Preach.CS2.Plugins.RollTheDiceV2.Utilities;

namespace Preach.CS2.Plugins.RollTheDiceV2.Effects;

public class EffectOneHealth : EffectBaseRegular, IEffectParameter
{
    public override bool Enabled { get; set; } = true;
    public override string PrettyName { get; set; } = "1 HP".__("effect_name_one_health");
    public override string Description { get; set; } = "Your health is 1".__("effect_description_one_health");
    public override double Probability { get; set; }  = 3;
    public override bool ShowDescriptionOnRoll { get; set; } = false;
    public Dictionary<string, string> RawParameters { get; set; } = new();

    public override void Initialize()
    {
    }

    public override void OnApply(CCSPlayerController? playerController)
    {
        if (playerController == null || playerController.PlayerPawn.Value is null)  return;

        // Health can't be less than 1 otherwise server crashes
        var plyHealth = playerController!.PlayerPawn.Value.Health;
        playerController!.PlayerPawn.Value.Health = 1;

        playerController.RefreshUI();
        PrintDescription(playerController, "effect_description_one_health");
    }

    public override void OnRemove(CCSPlayerController? playerController)
    {
    }
}