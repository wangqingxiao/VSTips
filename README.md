# VSTips
Visual Studio 上的一个Addin 插件，可以连接数据库，在VS中增加代码助记位置，并能检索和双击自动跳转到代码位置。

使用方式：
将 BIN 目录的这些文件放到 VS 的addins目录下重启VS即可使用。
config.ini              --- 配置文件，主要是数据库链接字符串
Mono.Security.dll       --- 链接 Postgres 数据使用的
Npgsql.dll              --- 链接 Postgres 数据使用的
ToolsX.dll              --- 插件主体
ToolsX.AddIn            --- VS 加载插件的引导文件
