# netconf client is a netconf test tool, it supports OTN equipment, supports the creation of ODU services, SDH services, and packet Ethernet services. It supports performance, TCA, protection groups, notifications, subscriptions, and automated testing.

.NET framework 4.0 

netconf Version record:
2022.12.09 Fix errors such as prompting that there is no "declaration" after subscription
2022.12.08 Repair the writing and reading of log information
2022.12.08 Solve the problem that the automated test log cannot be output
2022.12.05 Optimize the process of saving XML scripts
2022.11.23 Solved the problem that the XML script debugging window was stuck
2022.11.17 Added the alarm suppression function, which can be found by right clicking on the alarm module
2022.11.02 Added pop_push VLAN action
2022.11.02 Update China Unicom's new historical performance full query method
2022.10.27 Optimize business details and add loopback configuration
2022.10.26 Optimize the XML script debugging and return to display double-click support to view and compare the original XML data
2022.10.25 Added log search function
2022.10.25 LLDP, Peer notification modification completed
2022.10.24 Alarms, protection groups, attribute changes, and general notifications have been implemented. New schemes to receive subscription notifications, others to be improved...
2022.10.21 Solve the problem that the automated test notification leads to the wrong judgment of the use case, and also solve the problem of missing notifications
2022.10.20 Added double-click business query to display CTP details
2022.08.30 Solve the problem that the mobile EOS business service mapping mode is not updated
2022.06.26 Attempt to solve the problem of the notification and reply module
2022.06.20 Solve the conflict between notification and response modules
2022.06.16 Add loop execution automation use cases
2022.05.30 Modified the function of the performance module and added all cycles
2022.05.20 Added alarm query module
2022.05.15 Solve the problem of the wrong protection node of the next business package NNI2
2022.05.14 Added the function of sorting out business and deleting all business
2022.05.13 Added Telnet to delete residual configuration function, which may be relatively incomplete
2022.05.11 Added automated test to replace all ip addresses with one click
2022.05.09 Added device protection group query and operation, and added log file recording function to facilitate locating problems
2022.05.08 Added file management function, added time setting query function, added MC-Port configuration and query
2022.05.08 Added board reset and delete functions
2022.05.03 New user password modification
2022.05.02 Create a new telecom MSP and OCH protection group
2022.05.02 Remove the service mapping mode for EoOSU business distribution
2022.04.27 Solve the problem of creating an OSU transparent transmission service cir pir incorrectly
2022.04.26 Solve the problem that the creation of all business selection protection delivery scripts does not take effect
2022.04.24 Solved the problem of md-name delivery error
2022.04.23 Solved the problem of failure to create transparent transmission OSU and SDH services
2022.04.15 Added loopback operation, laser operation, and admin interface shutdown operation
2022.04.14 Add ETH-CTP VLAN and other information modification
2022.04.14 Added NTP creation function, board management supports three major operators
2022.04.10 New Telecom EO-OSU packet service creation
2022.04.10 Added NNI TO NNI scenarios for Telecom, China Unicom, and Mobile ETH services, and Telecom protection group query
2022.04.09 Added Telecom Transparent Transmission OSU Service Creation and Telecom Transparent Transmission ODU Service NNIToNNI Scenario Service Creation
2022.04.07 New Telecom EoO service creation and OAM service creation query
2022.04.02 Added UNI TO NNI and NNI TO NNI scenarios for Telecom, China Unicom, and Mobile SDH services
2022.04.01 Added version update information prompt
2022.04.01 The SDH service was migrated, and no preparations were made for future convergence operators
2022.03.28 Added NNIToNNI service of China Unicom ODU
2022.02.10 New Telecom SDH service creation
2021.12.21 Optimized time slot selection, changed the non-popup window, and supported the identification of occupied time slots
2021.12.16 Modify the automated test module to support the selection of whether to execute and the waiting time
2021.12.07 Modify the service creation and specify the payload type according to the private namespace
2021.12.03 Added user login authentication
2021.12.03 Automation function Support matching namespace
2021.12.02 New EOS service VCG time slot adjustment
2021.11.26 Add up to 32 network elements to go online at the same time
2021.11.21 Added TCA parameter setting
2021.11.18 Add ETH-to-ETH service creation, support CTP speed limit adjustment
2021.11.14 Add multiple network elements to log in at the same time to create and use
2021.11.13 Added oduk online delay measurement
2021.11.12 Added the ODUFlex time slot adjustment function of China Unicom and China Mobile
2021.11.10 Added the service created under China Unicom Yang to query, support layer protocol modification and port information query
2021.11.04 Added automated test function, import and export, view detailed information
2021.10.13 Added Ethernet OAM configuration
2021.10.12 Solve the problem of including Chinese calculation byte count error in version 1.1 of automatic mode
2021.09.25 Added NetConf protocol version 1.1 and 1.0 automatic mode
2021.09.23 Modified the login bug and abnormal error reporting
2021.09.20 Add SDH service creation, protection group query and operation
2021.09.19 Added EOS business creation (VC4, VC3, VC12)
2021.09.17 Added performance statistics
2021.09.16 Added automatic loading of online XML scripts from the Internet (users no longer need to find scripts)
2021.09.15 New EOO business creation
2021.09.14 Create a new transparent transmission service
2021.09.10 Add notification classification processing function
2021.09.05 New subscription function
2021.09.03 Added netconf client tool debugging window
版本记录：
2022.12.09 修复订阅后提示未“声明”等错误
2022.12.08 修复日志信息的写入和读取
2022.12.08 解决自动化测试日志无法输出的问题
2022.12.05 优化保存XML脚本流程
2022.11.23 解决了XML脚本调试编写窗口卡死的问题
2022.11.17 新增告警抑制功能，告警模块右键可找到
2022.11.02 新增pop_push的VLAN动作
2022.11.02 更新联通新的历史性能全量查询方式
2022.10.27 优化业务详细信息，增加环回配置
2022.10.26 优化XML脚本调试返回显示双击支持查看对比原始XML数据
2022.10.25 新增log查找功能
2022.10.25 LLDP、Peer通知修改完成
2022.10.24 告警、保护组、属性变更、通用通知已经实现新方案接收订阅通知，其他的待完善...
2022.10.21 解决自动化测试通知导致判断用例通过错误的问题，也解决了通知漏报的问题
2022.10.20 新增双击业务查询显示CTP详细信息
2022.08.30 解决移动EOS业务服务映射模式没有更新问题
2022.06.26 尝试解决通知与回复模块的问题
2022.06.20 解决通知与应答模块冲突问题
2022.06.16 增加循环执行自动化用例
2022.05.30 修改了性能模块的功能增加了全部周期
2022.05.20 新增告警查询模块
2022.05.15 解决下业务包NNI2保护节点错误的问题
2022.05.14 新增梳理业务与全部删除业务功能
2022.05.13 新增Telnet 删除残余配置功能,可能还相对不完善
2022.05.11 新增自动化测试一键替换所有ip地址
2022.05.09 新增设备保护组查询和操作,新增日志文件记录功能，方便定位问题
2022.05.08 新增文件管理功能,新增时间设置查询功能,新增MC-Port配置和查询
2022.05.08 新增板卡复位和删除功能
2022.05.03 新增用户密码修改
2022.05.02 新增电信MSP和OCH保护组创建
2022.05.02 去掉EoOSU业务下发服务映射模式
2022.04.27 解决创建OSU 透传业务cir pir不对问题
2022.04.26 解决创建所有业务选择保护下发脚本不生效问题
2022.04.24 解决了md-name下发错误问题
2022.04.23 解决了透传OSU和SDH业务创建失败的问题
2022.04.15 新增环回操作、激光器操作、admin接口关闭操作
2022.04.14 新增ETH-CTP VLAN等信息修改
2022.04.14 新增NTP创建功能，板卡管理支持三大运营商
2022.04.10 新增电信 EO-OSU分组业务创建
2022.04.10 新增电信、联通、移动ETH业务的 NNI TO NNI 俩种场景业务创建，以及电信保护组查询
2022.04.09 新增电信 透传OSU业务创建 和 电信透传ODU业务NNIToNNI场景的业务创建
2022.04.07 新增电信 EoO业务创建和OAM业务创建查询
2022.04.02 新增电信、联通、移动SDH业务的UNI TO NNI 和NNI TO NNI俩种场景业务创建
2022.04.01 新增版本更新信息提示
2022.04.01 迁移了SDH业务，未后面融合运营商做准备
2022.03.28 新增联通ODU的NNIToNNI业务
2022.02.10 新增电信的SDH业务创建
2021.12.21 优化了时隙选择，更改未弹窗，支持已被占用的时隙标识
2021.12.16 修改自动化测试模块，支持选择是否执行和等待时间
2021.12.07 修改业务创建，根据私有命名空间指定净荷类型
2021.12.03 新增用户登录认证
2021.12.03 自动化功能 支持匹配命名空间
2021.12.02 新增EOS业务VCG时隙调整
2021.11.26 新增最多32个网元同时上线
2021.11.21 新增TCA参数设置
2021.11.18 新增ETH-to-ETH业务创建，支持CTP限速调整
2021.11.14 新增多个网元同时登录创建使用
2021.11.13 新增oduk在线时延测量
2021.11.12 新增联通、移动的ODUFlex时隙调整功能
2021.11.10 新增联通yang下的业务创建于查询，支持层协议修改和端口信息查询
2021.11.04 新增自动化测试功能，导入导出，详细信息查看
2021.10.13 新增以太网OAM配置
2021.10.12 解决自动模式1.1版本下包含中文计算字节数错误问题
2021.09.25 新增NetConf协议版本1.1与1.0自动模式
2021.09.23 修改了登录BUG以及异常报错
2021.09.20 新增SDH业务创建、保护组查询和操作
2021.09.19 新增EOS业务创建（VC4、VC3、VC12）
2021.09.17 新增性能统计
2021.09.16 新增自动加载互联网在线XML脚本（用户再也不用去找脚本啦）
2021.09.15 新增EOO业务创建
2021.09.14 新增透传业务创建
2021.09.10 新增通知分类处理功能
2021.09.05 新增订阅功能
2021.09.03 新增netconf客户端工具调试窗口


前往下方链接下载体验
http://hunan128.com/

本程序加载了Renci.SshNet库文件，由于该库对netconf的支持功能上有些问题，在此库基础上更改并新增了订阅等功能。
请不要作为商业用途，此程序仅仅提供交流学习使用。
如果出现其他商业问题，概不负责。
