## Status: Developing  

### 当前版本的各种隐藏设定  
* Windows 8 or above required... （`IDesktopWallpaper` interface  
* 提取间隔为 6 小时，暂未提供设置  
* 使用 aspect ratio 过滤宽高比会使实际获取到的图片数量比预期少（没什么好办法  
* 自动换壁纸仅在提取操作成功完成后生效，  
  如因网络问题中断…… 抱歉 ╮(￣▽￣)╭ （Fork you，中国移动，说的就是你  
* 如果未指定存放文件的绝对路径，那么启动时将不会自动提取（以避免未经允许修改当前目录  
  
#### 其实正确的使用方式是…… 把中意的图片复制到另一个文件夹内，然后使用 Windows 的壁纸幻灯片功能 ( ´_ゝ｀)  
  
### 接下来的打算。。。？  
* 支持系统代理设置  
* 历史图片清理功能（只适用于绝对路径，筛选扩展名（感觉一旦出了 bug 的话会不得了呢…… （`rm -rf /`什么的  
* 支持分类（如果主站打算分类的话  
* 重构代码。。。（你信吗？反正我不信 233  
