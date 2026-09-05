using Godot;

// 下落：空中向下运动的阶段，落地后回到待机或跑动
public class FallState : PlayStates
{
    public FallState(Player player) : base(player)
    {
    }

    public override void init()
    {
        // 下落状态默认去往待机状态
        nextStates = player.idleState;
    }

    public override void enter()
    {
        GD.Print("状态 -> Fall");
    }

    public override PlayStates PhysicsProcess(double delta)
    {
        player.ApplyGravity(delta);

        // 空中允许左右微调方向
        float direction = player.GetInputDirection();
        player.ApplyHorizontalMove(direction);

        // 落地后：有方向输入就跑，没有就待机
        if (player.IsOnFloor())
        {
            return direction != 0f ? player.runState : player.idleState;
        }

        return null;
    }
}
