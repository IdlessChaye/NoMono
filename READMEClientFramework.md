# NoMono Client Framework

### 思想

- 根据印象总结一个客户端框架，完成阶段性成果
- 尽力减少MonoBehaviour的使用，尤其指游戏对象的组件
- 将面向Unity3D游戏引擎的游戏制作过程，改成面向代码的游戏制作过程
- 主要以代码完成对Manager、UI、GameObject的初始化及生命周期管理

### 主要角色

0、GameManager

- 游戏管理器
- 负责管理GlobalManager和LocalManager的生命周期
- Tick之源

1、GlobalManager

- 全局跨场景管理器
- 游戏一开始就持续存在
- 单例
- 跨场景不会被销毁，但是有回调函数会执行

2、LocalManager

- 局部单场景管理器
- 在场景加载之初存在，直至场景被卸载
- 单场景的单例
- 只在一个场景中存在，跨场景会先销毁，再创建

3、Context

- 一个UI Prefab
- 由UIManager管理，UIManager是个LocalManager
- 有两类：Page和Widget。Page是大UI，比如主界面；Widget是轻量的UI，比如小事件提示
- Prefab --- MonoContext --- Context，三位一体

4、Entity

- 除了Manager和UI，剩下的GameObject统称为Entity
- 由EntityManager管理，EntityManager是个LocalManager
- Entity上可以挂载Component
- Entity可以有对应的Prefab
- Prefab(可无) --- MonoPlugin --- Entity，三位一体。(类似Context的处理方式)

5、Component

- Entity上挂载的组件
- 由Entity管理，Entity在场景中随时可以被创建
- 分为LogicComponent和VisualComponent，只是Logic放逻辑相关的组件，Visual放视觉相关的，具体的没那么严格
- 对于Entity来说，自己是单例

### 设计思路

1、needTick

- needTick为true的才会被Tick
- Manager、Context、Entity、Component都有自己的needTick，要在new()、Init()中设置一下，默认为false

2、_prefabPath

- 有_prefabPath的才会加载对应的prefab
- protected abstract string _prefabPath { get; }
- context必须要实现，Entity可以返回空字符

3、Tick树

- GameObject Tick GlobalManagers 和 LocalManagers
- UIManager Tick contexts; EntityManager Tick entities
- Entity Tick components

4、三层初始化、动态添加

- Manager、Context、Entity、Component有三层初始化，分为是

    0、abstract属性
    1、new()
    2、Init()
    3、Setup()

- 支持在Init()中嵌套Add同级或下级的Tick对象，比如Tick对象：Manager>Entity>Component
- new()由管理者执行，可以传入一些需要的参数
- Init()和Setup()实现了分层初始化，简单来说就是在一帧中，所有Tick对象的Init()都执行完了才执行它们的Setup()。这样的好处在于Entity可以在它的Setup()中访问别的Entity的Component
- 为了实现上边那个功能，对象的初始化在逻辑上是“递归”完成的。比如A Entity的Init()中new了B Entity，并通过EntityManager来Add了B Entity，那对于A Entity来说，执行到A的Setup()的时刻，B Entity对于A已经是完全初始化完成的了，即B Entity的所有Component都执行完毕Init()和Setup()了，且B Entity也执行完Setup()了
- Setup()执行时刻为LateUpdate。这样一帧中随时都可以进行Add Tick对象，被Add的Tick 对象会在这帧的末尾完成初始化，并在下一帧开始正常Tick
- Add Tick对象应该在Tick对象的Init()函数中完成。如果不得已写到了Setup()中，则Tick对象会在下一帧的末尾进行初始化，并在下下帧开始正常Tick

### 写代码的思路

1、对于GlobalManager

- 在GameManager的AddGlobalManagers()中添加即可
- 继承BaseGlobalManager

2、对于LocalManager

- 在GameManager的AddLocalManagers()中添加即可
- 继承BaseLocalManager

3、对于Context

- 做好Prefab，Prefab上要带上MonoContext
- Context继承BaseXXXContext；MonoContext继承BaseMonoXXXContext
- 在UIManager的ShowPages()中添加即可

4、对于Entity

- 可以做Prefab，Prefab带上Plugin
- Entity继承BaseEntity；Plugin继承BaseMonoPlugin
- 可以通过EntityManager的AddEntities()中new出来，或者通过AddEntity(new BaseEntity())整出来
- 可以在Init()中嵌套new别的Entity
- 自己的组件要在Init()中通过AddComponent(new BaseComponent())整出来
- Setup()是来访问别的同级或次级的Tick对象的

5、对于Component

- 看情况继承BaseLogicComponent或者BaseVisualComponent
- 通过BaseEntity的AddComponent来挂载到Entity上
- Init()和Setup()的规则与Entity类似