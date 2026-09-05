using Godot;

// 跑动：在地面上左右移动
public class RunState : PlayStates
{
    public RunState(Player player) : base(player)
    {
    }

    public override void init()
    {
        // 跑动状态默认去往待机状态
        nextStates = player.idleState;
    }

    public override void enter()
    {
        GD.Print("状态 -> Run");
    }

    public override PlayStates handleInput(InputEvent @event)
    {
        // 跑动中也可以直接起跳
        if (@event.IsActionPressed("jump") && player.IsOnFloor())
        {
            return player.jumpState;
        }

        return null;
    }

    public override PlayStates PhysicsProcess(double delta)
    {
        // 跑动途中掉下地面，切到下落
        if (!player.IsOnFloor())
        {
            return player.fallState;
        }

        float direction = player.GetInputDirection();
        player.ApplyHorizontalMove(direction);

        // 松开方向键就回到待机
        if (direction == 0f)
        {
            return player.idleState;
        }

        return null;
    }
}
