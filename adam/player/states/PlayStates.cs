using Godot;

// 状态机：所有玩家状态的基类
// 注意这里故意【不】继承 Node。状态只是普通 C# 对象，由 Player 统一创建和驱动，
// 这样 init/enter/exit 才能用 virtual + override 正常重写，也不用往场景树里挂节点。
public abstract class PlayStates
{
    // 持有玩家引用，子类靠它读写速度、位置等
    public Player player;

    // 本状态默认要切换到的下一个状态，在 init() 里赋值
    public PlayStates nextStates;

    public PlayStates(Player player)
    {
        this.player = player;
    }

    // 状态机启动时调用一次（只调用一次），用来认识其它状态、填充 nextStates
    public virtual void init()
    {
    }

    // 每次进入该状态时调用
    public virtual void enter()
    {
    }

    // 每次离开该状态时调用
    public virtual void exit()
    {
    }

    // 处理输入事件。返回一个状态表示要切换过去，返回 null 表示保持当前状态
    public virtual PlayStates handleInput(InputEvent @event)
    {
        return null;
    }

    // 对应 Player._Process。返回 null 表示保持当前状态
    public virtual PlayStates Process(double delta)
    {
        return null;
    }

    // 对应 Player._PhysicsProcess。返回 null 表示保持当前状态
    public virtual PlayStates PhysicsProcess(double delta)
    {
        return null;
    }
}
