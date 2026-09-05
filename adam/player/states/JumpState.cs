using Godot;

// 跳跃：起跳后向上运动的阶段，到最高点转为下落
public class JumpState : PlayStates
{
    public JumpState(Player player) : base(player)
    {
    }

    public override void init()
    {
        // 跳跃状态默认去往下落状态
        nextStates = player.fallState;
    }

    public override void enter()
    {
        // 进入的瞬间给一个向上的初速度
        Vector2 velocity = player.Velocity;
        velocity.Y = player.JumpVelocity;
        player.Velocity = velocity;

        GD.Print("状态 -> Jump");
    }

    public override PlayStates PhysicsProcess(double delta)
    {
        player.ApplyGravity(delta);

        // 空中允许左右微调方向
        player.ApplyHorizontalMove(player.GetInputDirection());

        // 垂直速度不再向上，说明到最高点了，转入下落
        if (player.Velocity.Y >= 0f)
        {
            return player.fallState;
        }

        return null;
    }
}
