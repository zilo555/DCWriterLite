# DCWriterLite 
August 18, 2025

<br />Author : yongfu-yuan(袁永福,MS MVP <img src="https://raw.githubusercontent.com/dcsoft-yyf/DCWriterLite/refs/heads/main/screensnapshort/msmvpsmall.png"/>) from CHINA . 
<br />Email:28348092@qq.com,Site:[https://dcwriter.cn/](https://dcwriter.cn/).
<br />Online demo [https://dcsoft-yyf.github.io/DCWriterLite/index.html](https://dcsoft-yyf.github.io/DCWriterLite/index.html).
# update log
<br/> 2025-8-18 : First publish.
<br/> 2025-8-26 : Update readme.
# For C# developers
&nbsp; &nbsp; This open-source project contains 160,000 lines of C# code. These codes demonstrate the following amazing features:  
1. Using **System.Drawing.Graphics** in Blazor WebAssembly projects.  
2. Porting **WinForm .NET** code to Blazor WebAssembly.  
3. Quickly loading and parsing XML text in Blazor WebAssembly. The use of the native **XmlTextReader** is simply too slow in comparison.  
4. This project also utilizes a trimmed version of the Blazor WebAssembly framework, which reduces the file size of the final **dotnet.wasm** and improves the software loading speed.  

# Introduction
<img src="https://raw.githubusercontent.com/dcsoft-yyf/DCWriterLite/refs/heads/main/screensnapshort/dcwl-1.png"/>
<br/><img src="https://raw.githubusercontent.com/dcsoft-yyf/DCWriterLite/refs/heads/main/screensnapshort/dcwl-2.png"/>
<br/><img src="https://raw.githubusercontent.com/dcsoft-yyf/DCWriterLite/refs/heads/main/screensnapshort/dcwl-3.png"/>
<br/><img src="https://raw.githubusercontent.com/dcsoft-yyf/DCWriterLite/refs/heads/main/screensnapshort/dcwl-4.png"/>

<br/>&nbsp; &nbsp; **DCWriterLite** is a powerful open-source online RTF rich text editor component just like MS WORD. It designed for enterprise-level applications.
<br/>&nbsp; &nbsp; Unlike other software such as TinyMCE and CKEditor, which are developed based on the browser's HTML DOM, DCWriterLite builds its own DOM structure, uses XML to store documents, and employs Canvas for document rendering and SVG for document printing. This enables DCWriterLite to overcome the limitations of the HTML DOM and achieve highly advanced document editing features, such as real-time pagination, reliable redo/undo functionality, and robust form fields.
<br/>&nbsp; &nbsp; It also offers unique secondary development capabilities, allowing developers to extend existing document element types and create their own, thus meeting more complex business requirements. 
<br/>&nbsp; &nbsp; DCWriterLite is developed based on Blazor Webassembly 9.0 and has been modified at the WASM low-level framework level. Compared to other Blazor WASM programs, it significantly reduces the size of the executable file, which enhances software loading speed and reduces memory usage. <br/>&nbsp; &nbsp; DCWriterLite is a pure front-end component with no restrictions on the server side, making it convenient to adapt to various operating environments. 
<br/>&nbsp; &nbsp; DCWriterLite is an open-source software. We also offer a corresponding commercial version, DCWriter, which implements all the functions of DCWriterLite and provides more abundant software features as well as reliable technical support and services. 
# DCWriterLite Open Source Edition Feature List
## Operating Environment
- Supports Windows, Linux, MacOS, iOS, Android, UOS, Kylin, Fde, and other operating systems.
<img src="https://raw.githubusercontent.com/dcsoft-yyf/DCWriterLite/refs/heads/main/screensnapshort/uos.jpg"/>
<br/><img src="https://raw.githubusercontent.com/dcsoft-yyf/DCWriterLite/refs/heads/main/screensnapshort/kylin.jpg"/>
<br/><img src="https://raw.githubusercontent.com/dcsoft-yyf/DCWriterLite/refs/heads/main/screensnapshort/fde.jpg"/>

- Supports Chrome, Firefox, and other browsers.
- Pure front-end components, does not rely on server-side, and does not depend on any third-party components.
## File Operations:
- Create new file
- Open files, supports XML formats.
- Save files, supports XML formats.
- Print settings
- Paper type
- Paper orientation
- Margin settings for top, bottom, left, and right
- Global document grid lines
- Print preview
## Copy and Paste
- Supports copying and pasting of plain text and private format document content
## Redo/Undo
- Supports unlimited redo and undo operations
## Text Styles
- Supports bold, italic, underline, strikethrough, font name, and font size settings
- Borders and background colors for text, input fields, and cells
## Rulers
- Supports horizontal and vertical rulers.
- Supports setting margins by dragging the ruler scales.
- Supports setting paragraph margins by dragging the ruler scales.
## Paragraph Styles
- Supports line spacing and paragraph spacing
## Programmable DOM API Interface
Provides a tree-like DOM model to represent all the content in the document. And provides the following DOM document element types.
- XTextDocument Document element
  - Represents the root node of the document and is the entry point for DOM tree operations.
- XTextContainerElement Container element, an abstract class, the base class for all container elements.
  - Can contain other elements.
- XTextImageElement Image element
  - Can freely drag to set the size of the image
  - Only supports JPG, PNG, BMP formats.
- XTextInputFieldElement Input field element
  - Input field highlighted display
  - Set background text
  - Content validation, including required fields, maximum value, minimum value, etc.
- XTextLineBreakElement Line break element
  - Text performs a soft return but does not apply paragraph spacing settings.
- XTextPageBreakElement Page break element
  - Can cause forced page breaks
- XTextPageInfoElement Page number element
  - Can display page numbers or total page numbers
- XTextParagraphFlagElement Paragraph flag element
  - Can be set as the first-line indent of a paragraph
  - Can be set as the hanging indent of a paragraph
  - Can be set as the alignment of a paragraph (left, center, right, justified)
  - Can set the line spacing and paragraph spacing of a paragraph
  - Can set numbered lists and bullet lists
- XTextTableElement Table element
  - Table header row
  - Merge and split cells
  - Set the height of table rows and the width of table columns by dragging with the mouse
  - Set forced page breaks for table rows
  - Set table rows as non-cross-page (cannot be split by page breaks)
- XTextCheckBoxElement/XTextRadioElement Check box/Radio button element
  - Can be set as selected state - Can be set to non-editable status
  - Can be set to required status
  - Multi-line text 

  If you want more features and services, please learn about [[the DCWriter Business Edition]](https://github.com/dcsoft-yyf/DCWriterLite/blob/main/README-be.md)) . 

---


# 对于C#程序员
&nbsp; &nbsp; 这个开源项目包含了16万行的C#代码。这些代码展示了以下神奇的功能：
1. 在Blazor WebAssembly项目中使用System.Drawing.Graphcis.
2. 将WinForm.NET的代码移植到Blazor WebAssembly.
3. 在Blazor WebAssembly中快速的加载和解析Xml文本。而使用原生的XmlTextReader实在是太慢了。
4. 这个项目还使用了一个剪裁版的Blazor WebAssembly框架，减少最终的dotnet.wasm的文件大小，提高软件加载速度。
# DCWriterLite简介
<br/>&nbsp;&nbsp;**DCWriterLite**是一个面向企业级应用的强大的开源的在线RTF富文本编辑器组件。
<br/>相对于TinyMCE/CKEditor等其他软件是基于浏览器的HTML DOM开发的，DCWriterLite构建
<br/>了自己的DOM结构，使用XML来存储文档，使用Canvas绘制文档，使用SVG来打印文档。这使得
<br/>DCWriterLite能突破HTML DOM的限制，实现了非常强大的文档编辑器功能，比如实时分页、
<br/>可靠的重做/撤销功能、强大的表单域等等。还提供独有的的二次开发能力，开发者可以
<br/>扩展已有的文档元素类型，并创建自己的文档元素类型，使得软件能满足更复杂的业务需求。
<br/>DCWriterLite是基于Blazor Webassembly9.0开发的。并魔改了WASM底层框架，相对于其他
<br/>的Blazor WASM程序，大大减少了可执行文件的大小，这提高了软件加载速度，减少了内存占用。
<br/>&nbsp;&nbsp;DCWriterLite是一个纯前端的组件，对应服务器端没有任何限制，方便适应各种运行环境。
<br/>&nbsp;&nbsp;DCWriterLite是一个开源软件，我们还提供一个对应的商业版软件DCWriter，DCWriter实现
<br/>了DCWriterLite的全部功能，并提供更加丰富的软件功能以及可靠的技术支持和服务。

# DCWriterLite 开源版功能清单
## 运行环境
  - 支持Windows、Linux、MacOS、iOS、安卓、统信、麒麟、方德等操作系统。
  - 支持Chrome、Firefox等浏览器。
  - 纯前端组件，不依赖服务器端，不依赖任何第三方组件。
## 文件操作：
  - 新建文件
  - 打开文件，支持XML格式。
  - 保存文件，支持XML格式。
  - 打印设置
     - 纸张类型
     - 纸张方向
     - 上下左右的边距设置
     - 全局性文档网格线
  - 打印预览
## 复制粘贴
  - 支持纯文本、私有格式的文档内容的复制粘贴
## 重做/撤销
  - 支持不限制次数的重做和撤销操作
## 文本样式
  - 支持粗体、斜体、下划线、删除线、字体名称、字体大小的设置
  - 文本、输入域、单元格的边框和背景色。
## 标尺
  - 支持横向和纵向标尺。
  - 支持拖拽标尺的刻度来设置页边距。
  - 支持拖拽标尺的刻度来设置段落边距。
## 段落样式
  - 支持行间距和段落间距
## 可编程DOM API接口
提供树状的DOM模型，用于表示文档中所有的内容。并提供以下DOM文档元素类型。
 - XTextDocument 文档元素
      - 表示文档根节点，是DOM树的操作的入口点。
 - XTextContainerElement 容器元素，是一个抽象类，是所有容器元素的基类。
      - 可以包含其他元素。
 - XTextImageElement 图片元素
      - 可以自由拖拽设置图片的大小
      - 只支持JPG\PNG\BMP格式。
 - XTextInputFieldElement 输入域元素
      - 输入域高亮度显示
      - 设置背景文本
      - 内容校验，包括必填项、最大值、最小值等。
 - XTextLineBreakElement 换行元素
      - 文本进行软回车，但不应用段落间距的设置。
 - XTextPageBreakElement 分页元素
      - 可以导致强制分页
 - XTextPageInfoElement 页码元素
      - 可以显示页码或者总页数
 - XTextParagraphFlagElement 段落标记元素
      - 可以设置为段落的首行缩进
      - 可以设置为段落的悬挂缩进
      - 可以设置为段落的对齐方式（靠左、居中、靠右、两端对齐）
      - 可以设置段落的行间距和段落间距
      - 可以设置数字列表和项目符号列表
 - XTextTableElement表格元素
      - 表格标题行
      - 合并拆分单元格
      - 鼠标拖拽来设置表格行的高度和表格列的宽度
      - 表格行设置强制分页
      - 表格行设置为不可跨页（不能被分页线分割）
 - XTextCheckBoxElement/XTextRadioElement 单选框/单选按钮元素
      - 可以设置为选中状态
      - 可以设置为不可编辑状态
      - 可以设置为必填状态
      - 文本多行

  您若想要更多的功能和服务，请了解[[DCWriter商业版]](https://github.com/dcsoft-yyf/DCWriterLite/blob/main/README-be.md)).
 
# Donate 捐助
You can donate by <a href="https://www.paypal.com/paypalme/yuanyongfu">paypal</a> , by <a href="https://raw.githubusercontent.com/dcsoft-yyf/DCNETProtector/main/alipay.jpg">alipay</a> , by <a href="https://raw.githubusercontent.com/dcsoft-yyf/DCNETProtector/main/wechat_pay.png">Wechat</a>,help author to feed twins born in 2020.
