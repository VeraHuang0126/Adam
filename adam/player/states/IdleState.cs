using Godot;

// 待机：站在地面上不动
public class IdleState : PlayStates
{
    public IdleState(Player player) : base(player)
    {
    }

    public override void init()
    {
        // 待机状态默认去往跑动状态
        nextStates = player.runState;
    }

    public override void enter()
    {
        // 进入待机时立刻停住，避免从跑动切过来时还在滑
        Vector2 velocity = player.Velocity;
        velocity.X = 0f;
        player.Velocity = velocity;

        GD.Print("状态 -> Idle");
    }

    public override PlayStates handleInput(InputEvent @event)
    {
        // 站在地面上按跳跃键，切到跳跃状态
        if (@event.IsActionPressed("jump") && player.IsOnFloor())
        {
            return player.jumpState;
        }

        return null;
    }

    public override PlayStates PhysicsProcess(double delta)
    {
        // 离开地面了（比如地面消失、被推下去），切到下落
        if (!player.IsOnFloor())
        {
            return player.fallState;
        }

        float direction = player.GetInputDirection();
        player.ApplyHorizontalMove(direction);

        // 有水平输入就切到跑动
        if (direction != 0f)
        {
            return player.runState;
        }

        return null;
    }
}
