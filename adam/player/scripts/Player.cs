using Godot;

public partial class Player : CharacterBody2D
{
    // ---- 横版跳跃移动参数，都暴露到检查器里方便调 ----
    [Export] public float MoveSpeed = 320f;      // 水平移动速度
    [Export] public float Gravity = 1200f;       // 重力加速度（像素/秒²）
    [Export] public float JumpVelocity = -520f;  // 起跳初速度，负号表示向上

    // 当前状态
    public PlayStates currentState;

    // 缓存精灵节点，避免每帧 GetNode 查找
    private Sprite2D _sprite;

    // 各个具体状态的引用，状态之间靠它们互相跳转
    public IdleState idleState;
    public RunState runState;
    public JumpState jumpState;
    public FallState fallState;

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite2D");

        // 1. 创建所有状态，并把玩家自身传进去
        idleState = new IdleState(this);
        runState = new RunState(this);
        jumpState = new JumpState(this);
        fallState = new FallState(this);

        // 2. 让每个状态认识其它状态（填充各自的 nextStates）
        idleState.init();
        runState.init();
        jumpState.init();
        fallState.init();

        // 3. 进入初始状态
        ChangeState(idleState);
    }

    // 统一的状态切换入口：旧状态 exit -> 新状态 enter
    // 传 null 或同一个状态时什么都不做，所以各方法可以放心 return null
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

    // 读取水平输入，返回 -1（左）/ 0（无）/ 1（右）
    public float GetInputDirection()
    {
        return Input.GetAxis("left", "right");
    }

    // 按输入方向设置水平速度，同时让精灵翻转朝向
    public void ApplyHorizontalMove(float direction)
    {
        Vector2 velocity = Velocity;
        velocity.X = direction * MoveSpeed;
        Velocity = velocity;

        if (direction != 0f && _sprite != null)
        {
            // direction 为 -1 时 FlipH = true，即朝左时水平翻转
            _sprite.FlipH = direction < 0f;
        }
    }

    // 累加重力到垂直速度。
    // 站在地面上时只清除【向下】的残留速度（避免落地后每帧拿速度去撞地面造成抖动）；
    // 向上的速度必须放过——起跳那一帧 IsOnFloor() 仍为 true（MoveAndSlide 还没执行），
    // 如果一并清零，刚设好的起跳初速度会被抹掉，角色就跳不起来了。
    public void ApplyGravity(double delta)
    {
        Vector2 velocity = Velocity;

        if (IsOnFloor())
        {
            if (velocity.Y > 0f)
            {
                velocity.Y = 0f;
                Velocity = velocity;
            }

            return;
        }

        velocity.Y += Gravity * (float)delta;
        Velocity = velocity;
    }
}
