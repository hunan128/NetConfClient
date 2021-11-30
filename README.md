# netconf client is a netconf test tool, it supports OTN equipment, supports the creation of ODU services, SDH services, and packet Ethernet services. It supports performance, TCA, protection groups, notifications, subscriptions, and automated testing.

netconf Version record:

2021.11.26 supports up to 32 network elements to go online at the same time

2021.11.21 Support TCA parameter setting

2021.11.18 Support ETH-to-ETH business creation, support CTP rate limit adjustment

2021.11.14 supports simultaneous login and creation of multiple network elements

2021.11.13 Support oduk online delay measurement

2021.11.12 Supports ODUFlex time slot adjustment function of China Unicom and China Mobile

2021.11.10 Supports the creation and query of services under Unicom Yang, and supports layer protocol modification and port information query

2021.11.04 Support automated test function, import and export, detailed information view

2021.10.13 supports Ethernet OAM configuration

2021.09.25 Support NetConf version auto-negotiation (1.0 and 1.1)

2021.09.19 Support EOS business creation (VC4, VC3, VC12)

2021.09.17 Support performance statistics

2021.09.16 supports automatic loading of Internet online XML scripts (users no longer need to find scripts)

2021.09.15 Support EOO business creation

2021.09.14 supports the creation of transparent transmission services

2021.09.10 Support notification classification processing function

2021.09.05 supports subscription function

2021.09.03 Support netconf client tool debugging window

版本记录：

2021.11.26 支持最多32个网元同时上线

2021.11.21 支持TCA参数设置

2021.11.18 支持ETH-to-ETH业务创建，支持CTP限速调整

2021.11.14 支持多个网元同时登录创建使用

2021.11.13 支持oduk在线时延测量

2021.11.12 支持联通、移动的ODUFlex时隙调整功能

2021.11.10 支持联通yang下的业务创建于查询，支持层协议修改和端口信息查询

2021.11.04 支持自动化测试功能，导入导出，详细信息查看

2021.10.13 支持以太网OAM配置

2021.09.25 支持NetConf版本自动协商(1.0与1.1)

2021.09.19 支持EOS业务创建（VC4、VC3、VC12）

2021.09.17 支持性能统计

2021.09.16 支持自动加载互联网在线XML脚本（用户再也不用去找脚本啦）

2021.09.15 支持EOO业务创建

2021.09.14 支持透传业务创建

2021.09.10 支持通知分类处理功能

2021.09.05 支持订阅功能

2021.09.03 支持netconf客户端工具调试窗口

![image](https://user-images.githubusercontent.com/59459264/144022899-1b9dd707-963e-49de-8767-691ca49d9689.png)

连接页面：
![image](https://user-images.githubusercontent.com/59459264/134663857-d1178673-a923-4d78-9786-5728aa74d0fd.png)
脚本执行页面：
![image](https://user-images.githubusercontent.com/59459264/134770811-24f7a836-3d17-41c7-b4e2-344793088d14.png)

保护倒换通知页面：
![image](https://user-images.githubusercontent.com/59459264/134770450-328ea051-5add-467d-918a-a5199f48cec7.png)
LOG信息日志页面：
![image](https://user-images.githubusercontent.com/59459264/134770684-7a52fd53-e8b8-45fd-8aec-3ef793144346.png)
性能页面：

前往下方链接下载体验
http://hunan128.com/index.php/2021/09/12/netconf%e6%94%af%e6%8c%81%e4%b8%9a%e5%8a%a1%e5%88%9b%e5%bb%ba/

本程序加载了Renci.SshNet库文件，由于该库对netconf的支持功能上有些问题，在此库基础上更改并新增了订阅等功能。
请不要作为商业用途，此程序仅仅提供交流学习使用。
如果出现其他商业问题，概不负责。
