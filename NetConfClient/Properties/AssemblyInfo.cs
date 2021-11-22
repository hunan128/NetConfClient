using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// 有关程序集的一般信息由以下
// 控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle("Netconf")]
[assembly: AssemblyDescription("" +
   
    "2021.11.21 支持TCA参数设置" + "\r\n" +
    "2021.11.14 支持多个网元同时登录创建使用" + "\r\n" +
    "2021.11.12 支持联通、移动的ODUFlex时隙调整功能" + "\r\n" +
    "2021.11.10 支持联通yang下的业务创建于查询，支持层协议修改和端口信息查询" + "\r\n" +
    "2021.11.04 支持自动化测试功能，导入导出，详细信息查看" + "\r\n" +
    "2021.10.13 支持以太网OAM配置" + "\r\n"+
    "2021.10.12 解决自动模式1.1版本下包含中文计算字节数错误问题" + "\r\n" +
    "2021.09.25 支持NetConf协议版本1.1与1.0自动模式" + "\r\n" +
    "2021.09.23 修改了登录BUG以及异常报错" + "\r\n" +
    "2021.09.20 支持SDH业务创建、保护组查询和操作" + "\r\n" +
    "2021.09.19 支持EOS业务创建（VC4、VC3、VC12）" + "\r\n" +
    "2021.09.17 支持性能统计" + "\r\n"+
    "2021.09.16 支持自动加载互联网在线XML脚本（用户再也不用去找脚本啦）" + "\r\n" +
    "2021.09.15 支持EOO业务创建" + "\r\n" +
    "2021.09.14 支持透传业务创建" + "\r\n" +
    "2021.09.10 支持通知分类处理功能" + "\r\n" +
    "2021.09.05 支持订阅功能" + "\r\n" +
    "2021.09.03 支持netconf客户端工具调试窗口" + "\r\n" +
"")]
[assembly: AssemblyConfiguration("小兔兔配置")]
[assembly: AssemblyCompany("公司网址：hunan128.com")]
[assembly: AssemblyProduct("产品名称：netconf客户端")]
[assembly: AssemblyCopyright("版权所有  ©  胡楠 2021.09.02")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 会使此程序集中的类型
//对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型
//请将此类型的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("23e2d8c4-4d81-4dd6-9ad3-169c877955bd")]

// 程序集的版本信息由下列四个值组成: 
//
//      主版本
//      次版本
//      生成号
//      修订号
//
// 可以指定所有值，也可以使用以下所示的 "*" 预置版本号和修订号
// 方法是按如下所示使用“*”: :
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("2021.11.10.15")]
[assembly: AssemblyFileVersion("2021.11.10.15")]
