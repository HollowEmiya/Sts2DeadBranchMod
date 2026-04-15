
using System.Reflection;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;

namespace DeadBranch.Scripts.Cards;

[Pool(typeof(ColorlessCardPool))]
public sealed class Unprepared : DColorlessCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => new []{CardKeyword.Exhaust};

    protected override IEnumerable<DynamicVar> CanonicalVars => new []{new PowerVar<WeakPower>(2m)};

	protected override IEnumerable<IHoverTip> ExtraHoverTips => new []{HoverTipFactory.FromPower<WeakPower>()};
    
	public Unprepared()
		: base(1, CardType.Skill, CardRarity.Ancient, TargetType.AnyEnemy)
	{
	}

    
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		// await Unprepared.Stun(cardPlay.Target,(IReadOnlyList<Creature> _) => Task.CompletedTask,null);
        Log.Info("[DeadBranch]Used JumpToNextState 1!");
        JumpToNextState(cardPlay.Target);
        Log.Info("[DeadBranch]Used JumpToNextState 2!");
        await PowerCmd.Apply<WeakPower>(cardPlay.Target, base.DynamicVars.Weak.BaseValue, base.Owner.Creature, this);
		// cardPlay.Target.Monster.RollMove(new[]{base.Owner.Creature});
	}

    public bool JumpToNextState(Creature creature, Rng? rng = null)
    {
        Log.Info("Enter JumpToNextState");
        var monster = creature.Monster;
        if (monster == null)
        {
            Log.Info("No Monster. Get out of JumpToNextState");
            return false;
        }

        FieldInfo? MachineField = typeof(MonsterModel).GetField("_moveStateMachine",
        BindingFlags.NonPublic | BindingFlags.Instance);
        var machine = MachineField?.GetValue(monster) as MonsterMoveStateMachine;;
        if (machine == null)
        {
            Log.Info("No MachineField. Get out of JumpToNextState");
            return false;
        }
        
        // 1. 获取当前状态
        var currentState = GetCurrentState(machine);
        if (currentState == null)
        {
            Log.Info("No currentState. Get out of JumpToNextState");
            return false;
        }
        
        // 2. 获取随机数生成器
        if (rng == null)
        {
            rng = creature.CombatState?.RunState?.Rng?.MonsterAi;
            if (rng == null) 
            {
                Log.Info("No rng. Get out of JumpToNextState");
                return false;
            }
        }
        
        // 3. 计算下一个状态ID（调用当前状态的GetNextState）
        var nextStateId = GetNextStateId(currentState, creature, rng);
        if (string.IsNullOrEmpty(nextStateId))
        {
            Log.Info("No nextStateId. Get out of JumpToNextState");
            return false;
        }
        
        // 4. 递归查找最终的MoveState（因为中间可能有决策状态）
        var nextMoveState = FindNextMoveState(machine, nextStateId, creature, rng);
        if (nextMoveState == null)
        {
            Log.Info("No nextMoveState. Get out of JumpToNextState");
            return false;
        }
        
        // 5. 使用SetMoveImmediate跳转（关键！）
        //    注意：forceTransition = true 会绕过 CanTransitionAway 检查
        monster.SetMoveImmediate(nextMoveState, forceTransition: true);
        Log.Info("[DeadBranch]Used JumpToNextState!");
        return true;
    }

    // 辅助方法（与之前类似）
    private MonsterState? GetCurrentState(MonsterMoveStateMachine machine)
    {
        var field = machine.GetType().GetField("_currentState", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        return field?.GetValue(machine) as MonsterState;
    }

    private string? GetNextStateId(MonsterState currentState, Creature owner, Rng rng)
    {
        var method = currentState.GetType().GetMethod("GetNextState", 
            BindingFlags.NonPublic | BindingFlags.Instance, 
            null, 
            new[] { typeof(Creature), typeof(Rng) }, 
            null);
        
        if (method == null) return null;
        return method.Invoke(currentState, new object[] { owner, rng }) as string;
    }

    private MoveState? FindNextMoveState(
        MonsterMoveStateMachine machine, 
        string startStateId, 
        Creature owner, 
        Rng rng, 
        int depth = 0)
    {
        const int maxDepth = 20;
        if (depth > maxDepth) return null;
        
        var statesField = machine.GetType().GetField("_states", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        var states = statesField?.GetValue(machine) as System.Collections.IDictionary;
        if (states == null || !states.Contains(startStateId)) return null;
        
        var state = states[startStateId] as MonsterState;
        if (state == null) return null;
        
        if (state is MoveState moveState) return moveState;
        
        // 递归获取下一个状态ID
        var nextStateId = GetNextStateId(state, owner, rng);
        if (string.IsNullOrEmpty(nextStateId)) return null;
        
        return FindNextMoveState(machine, nextStateId, owner, rng, depth + 1);
    }
	
	protected override void OnUpgrade()
	{
		base.DynamicVars.Weak.UpgradeValueBy(1m);
	}
}