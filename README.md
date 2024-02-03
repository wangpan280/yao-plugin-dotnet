# yao-plugin-dotnet
发布成单文件：dotnet publish -r win-x64 --self-contained=false /p:PublishSingleFile=true
然后把dotnetplugin.exe改成dotnetplugin.dll，置于plugins目录下。
运行yao run plugins.dotnetplugin.hello "world"，可测试效果。
