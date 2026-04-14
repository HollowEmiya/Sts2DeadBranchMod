
using BaseLib.Abstracts;

namespace DeadBranch.Scripts.Relics;

public abstract class DRModelIroncladRelicModel : CustomRelicModel
{
    
    // 小图标
    public override string PackedIconPath => $"res://DeadBranch/images/Relics/IroncladRelics/{GetType().Name}.png";
    // 轮廓图标
    protected override string PackedIconOutlinePath => $"res://DeadBranch/images/Relics/IroncladRelics/Outline/{GetType().Name}.png";
    // 大图标
    protected override string BigIconPath => $"res://DeadBranch/images/Relics/IroncladRelics/{GetType().Name}.png";
}