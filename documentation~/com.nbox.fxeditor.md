# FxEditor使用文档


##Shader参考
###Droplet

###Pixelate

###RadiusBlur

###ZoomBlur

###DirectionalBlur

###TextureColorMask

###TextureColorMaskMultiply

###TextureColorMaskAdditive

###Spherical

###BumpMap

###Wave

###ColorDistanceRGB

###Smoothstep

###Inverse

###PageCurl


##FXEditor菜单项目参考说明
###FxEditor/创建物体/画布
创建空的画布对象

###FxEditor/创建物体/效果画布
创建一个画布对象和一个空的效果画面



##定义
###画布(Canvas)
用于保存绘制内容的空间，为保存的内容提供重用的方法。如可以把ZoomBlur模糊的画面用于Wave扭曲。



##物体属性
###Tag
如果物体的Tag设置为EditorOnly将不会进入素材文件。不过要注意的是Tag设置在parent上无效。