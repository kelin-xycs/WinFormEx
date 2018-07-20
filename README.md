# WinFormEx
一个 用 C# 写的 WinForm 扩展库 ， 用于改善 WinForm 的 界面效果

WinFormEx 可以实现 仿 360 窗口 ， 可以实现 窗口 控件 透明 半透明 效果 ， 可以实现 事件冒泡 。

可在 Demo 项目中查看 Demo 。

WinFormEx.dll 中包含了 FormEx ControlEx Form360 3 个 类 ， 继承 Form360 就可以实现 仿 360 窗口效果 。 仿 360 窗口 包括 可拖拽 Header 移动窗口 ， 可将鼠标放到 窗口边缘 边角 拉动 改变窗口大小 ， 窗口边框阴影 ， ImageButton 。 Form360 继承 FormEx ， FormEx ControlEx 是 WinFormEx 提供的 窗体 和 控件 基类 ， 继承 FormEX 和 ControlEx 可以实现事件冒泡 。

事件冒泡 有什么意义呢 ？ WinForm 是没有事件冒泡的， 这样就给编写一些 界面效果 带来 困难 。 没有事件冒泡， 表现就像 父控件 的 事件 会被 子控件 “吃掉” 。

有了 事件冒泡 ， 我们就可以 编写 像 WPF 示例 里的 “播放按钮” 这样的效果 。 WPF 播放按钮 是 将一个 Play图标 嵌入 一个按钮（或者 Panel） 来实现丰富的界面效果 。

接下来， 透明 半透明 效果 怎么实现呢， 首先 ， WinForm 控件 提供了 BackColor = Color.Transparent 的 方法， 这是 背景透明 ， 也可以通过 Color.FromArgb() 方法来获得一个 半透明 的背景色 。 还可以通过 Form.Opacity 属性来设置整个 窗体 的 透明度 。 还可以 通过 窗体 的 BackColor 和 TransparencyKey 属性 设置相同的值 。

如果这些还不能满足要求， 那么可以自己 override OnPaintBackground() 方法， 对 窗体 和 控件 都可以 重写 OnPaintBackground() 方法 。 在 OnPaintBackground() 方法中什么都不做， 窗体 和 控件 的 背景 就是透明的 。

比如 Form360 的 边框阴影 就是 override OnPaintBackground() 方法 实现的 。

实现 窗口 的 边框阴影 ， 通用的做法有 2 种（包括 C++ 和 C#） ， 第一种是 调用 Win32 函数 GetClassLong() SetClassLong() ， 可以获得 系统 提供的 默认边框阴影效果 。 第二种是 使用 双层窗口， 除了 主窗口 外 ， 再创建一个 窗口 ， 用来显示 阴影， 主窗口 “盖在” 阴影窗口 的 上面 ， 阴影窗口 的 位置 会 跟随 主窗口 ， 可以 用 GDI+ 或者 PNG 图片 在 阴影窗口 上显示 阴影效果 。

第一种方法 效果 比较 单一 ， 可控性不高 。 第二种方法 又 太麻烦 。

我觉得 可以 用 GDI+ （System.Drawing.Graphics 类） 来 绘制 阴影效果 。 这种做法跟上面 第二种 做法 的 区别 是 ， 上面的 第二种 做法 的 阴影 可以用 鼠标 “点穿” ， 可以点到 阴影 下面的 其它窗口或者桌面 。 用 Graphics 绘图 的话 ， 不能用鼠标 “点穿” 阴影 ， 阴影 是 窗口 的 一部分 。 不过 我想 这不是问题 。

这里要提到 .Net Winform Graphics 的 1 个 Bug 。 如果在 窗口背景 上 画空心图形（如 空心矩形）， 或者用 半透明颜色（如 Color.FromArgb(125,Color.LightGreen)）画实心图形， 在 空心 或者 半透明 的 部分， 会留下 残影 。 残影 是指 空心 和 半透明 会 显示出窗口下面的内容 ， 比如 其它窗口 或者 桌面 ， 但是 会 一直保持在 第一次 显示的 内容 。 所以为了避免这个问题 ， 我现在的 边框阴影 是简单的 画了 2 组直线 来 实现的 。 窗口右边 2 根直线 ， 下边 2 根直线 ， 这样 2 组 。 但是这样 又有 1 个 Bug 。 就是在用鼠标放在窗口边缘边角拉动改变窗口大小时， 直线 会 出现 “擦花” 的现象 ， 擦花的效果 随着 拉动 而变化 。 拉过去 花一点， 再拉过来 又不花 一点 。 这样好像也不太坏 ， 阴影效果 更 明显了 。 不知道用 逐点绘制 和 PNG 图片 能不能 避免 这个问题 。

FormEx 和 ControlEx 可以 单独使用 ， 可以和 一般的 WinForm 程序 混合使用 ，
用 ControlEx 编写 的 控件 可以 放在 一般的 WinForm 程序里使用 。

编写 WinForm 控件 ， 主要是 2 个 技术点， 1 是 Windows 窗口消息模型 ， 2 是 GDI+ 绘图 。 把这些了解清楚了 ， 写 Winform 控件 也不难 ， 跟 写 Web 控件 差不多 。 啊哈哈哈

不过 Windows 窗口消息模型 这块 蛋糕 很大 ， 所以也有一点不容易 。 有时候觉得 如果 自己是一个 有多年经验的 Windows 开发人员就好了 。 事情就好办了。

Windows 提供的 消息模型 比较 庞大， 也 比较 原始 ， .Net Winform 对其进行了 封装 和 加工 ， 我们看到的 .Net Winform 的 事件模型， 会 显得 更 直观 和 易用 。

WinForm 对 Windows 消息 的 封装 ， 体现在， 有些效果 不是 接收一条 消息 就 搞定 的 。 比如 MouseEnter 事件 ， Windows 消息里并没有 “MOUSEENTER” 这样的 消息 ， 在 WinForm 里 ， MouseEnter 事件 是 通过 Win32 函数 TraceMouseEvent() 实现的 。

比如 MouseLeave 事件 ， 接收到 WM_MOUSELEAVE 消息时会触发 MouseLeave 事件 ， Wm_Destroy 时也会触发 MouseLeave 事件 ， 窗口弹出模态对话框 时 也会触发 MouseLeave 事件 。 窗口弹出模态对话框 时的 MouseLeave 事件 我估计不是通过消息的方式来实现的 ， 而是在 发起 模态对话框 的 代码 中 直接触发 MouseLeave 事件 。 发起 模态对话框 的 代码 是 Win32 的东西 ， 这部分大概是 .Net 和 Win32 的 内部约定 。

比如 Form.Activated 事件 ， 不是由 .Net 触发的， 是由 Win32 触发的 ， 在 .Net 代码里找不到触发 Form.Activated 的代码 。这也是 .Net 和 Win32 的 内部约定 。 在 研究 Form.Activated 的过程中 ， 我们发现 ， 有很多 Windows 消息 并不会 发送到 .Net WinForm 这个层面 。 比如 跟 Form.Activated 可能有关的一些消息 如 WM_ACTIVATEAPP WM_ENABLE WM_NCACTIVATE 都不会发送给 WinForm ， 具体的说就是 MessageFilter 接收不到这些消息 。 MessageFilter 是在 Form Control 之前处理消息的类，实现 IMessageFilter 接口 。 可以推测 NativeWindow 也接收不到这些消息 。











