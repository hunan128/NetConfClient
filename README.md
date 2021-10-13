# NetConfClient 参考中国移动yang模型语言制作的一个客户端工具，可用于连接OTN设备，创建业务，删除，查询等操作

未来：

告警模块、TCA模块、接口模块、业务关联查询
会支持脚本自动化测试功能

版本记录：
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
