using Godot;

public partial class Player : CharacterBody2D
{
    // ---- 移动参数（俯视角四方向）----
    [Export] public float MoveSpeed = 320f;

    // 当前状态
    public PlayStates currentState;

    // 各个具体状态的引用，方便状态之间互相跳转
    public IdleState idleState;
    public RunState runState;

    public override void _Ready()
    {
        // 1. 创建所有状态，并把玩家自身传进去
        idleState = new IdleState(this);
        runState = new RunState(this);

        // 2. 让每个状态认识其它状态（填充自己的 nextStates）
        idleState.init();
        runState.init();

        // 3. 进入初始状态
        ChangeState(idleState);
    }

    // 统一的状态切换入口：旧状态 exit -> 新状态 enter
    public void ChangeState(PlayStates newState)
    {
        if (newState == null || newState == currentState)
        {
            return;
        }

        currentState?.exit();
        currentState = newState;
        currentState.enter();
    }

    // 输入事件交给当前状态处理
    public override void _UnhandledInput(InputEvent @event)
    {
        if (currentState == null)
        {
            return;
        }

        ChangeState(currentState.handleInput(@event));
    }

    public override void _Process(double delta)
    {
        if (currentState == null)
        {
            return;
        }

        ChangeState(currentState.Process(delta));
    }

    public override void _PhysicsProcess(double delta)
    {
        if (currentState == null)
        {
            return;
        }

        ChangeState(currentState.PhysicsProcess(delta));

        // 物理计算完统一移动，速度由各状态自己设置
        MoveAndSlide();
    }

    // 读取四方向输入，返回方向向量（已归一化，避免斜向移动更快）
    public Vector2 GetInputDirection()
    {
        return Input.GetVector("left", "right", "up", "down");
    }
}
