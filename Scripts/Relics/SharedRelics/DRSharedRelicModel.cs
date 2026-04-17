using BaseLib.Abstracts;

namespace DeadBranch.Scripts.Relics;

public abstract class DRSharedRelicModel : CustomRelicModel
{
    // 小图标
    public override string PackedIconPath => $"res://DeadBranch/images/Relics/SharedRelics/{GetType().Name}.png";
    // 轮廓图标
    protected override string PackedIconOutlinePath => $"res://DeadBranch/images/Relics/SharedRelics/Outline/{GetType().Name}.png";
    // 大图标
    protected override string BigIconPath => $"res://DeadBranch/images/Relics/SharedRelics/LargeRelic/{GetType().Name}.png";
}