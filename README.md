# DCWriterLite 
August 18, 2025

<br />Author : yongfu-yuan(袁永福,MS MVP <img src="https://raw.githubusercontent.com/dcsoft-yyf/DCWriterLite/refs/heads/main/screensnapshort/msmvpsmall.png"/>) from CHINA . 
<br />Email:28348092@qq.com,Site:[https://dcwriter.cn/](https://dcwriter.cn/).
<br />Online demo [https://dcsoft-yyf.github.io/DCWriterLite/index.html](https://dcsoft-yyf.github.io/DCWriterLite/index.html).
# Update log
<br/> 2025-8-18 : First publish.
<br/> 2025-8-26 : Update readme.
<br/> 2025-8-31 : Update readme.

&nbsp; &nbsp; DCWriterLite is an open source RTF-like structured document editor that benefits many different groups of people:
# For C# developers
&nbsp; &nbsp; It shows some amazing programming techniques
1. In Blazor WebAssembly programming, the use of System.Drawing.Graphics and System.Windows.Forms for control types enables developers to migrate substantial amounts of C# code that would otherwise be discarded into WASM development, thereby safeguarding industry investments in related fields.
2. It implements a complete DOM model in the most concise way possible, making it easy to learn and understand. OpenOffice also defines a similar DOM model, but it contains tens of thousands of lines of code that are beyond the comprehension of most people or teams.
3. This project demonstrates how to streamline the Blazor Assembly framework. It helps developers understand some of the underlying principles of Blazor Assembly while reducing the final software file (dotnet.wasm) size and improving loading speed.
# For toB developers
1. DCWriterLite provides a featureful structured document function. It provides free text input functionality, but also provides form areas to limit the way specific areas are entered.
2. DCWriterLite implements a customizable DOM model, offering limitless extension capabilities. Developers can create new document element types to better align software with specific business requirements. For instance, it enables developers to build template creation tools that dynamically define complex form interfaces tailored to user business needs.
3. Instead of being based on HTML DOM, DCWriterLite implements its own document formatting algorithm, which breaks through some technical limitations of HTML DOM. For example, it implements reliable Undo/Redo functions and maintains completely consistent formatting results across different browsers.
4. DCWriterLite is stored in XML format with a simple structure, which makes it easy for developers to develop background programs to process millions of documents in bulk without having to call the DCWriterLite software module.
# For web developers
&nbsp; &nbsp; DCWriterLite provides the following comfortable features:
1. It is a pure front-end component that does not rely on third-party components, supports mainstream browsers such as Chrome and Firefox, and software deployment and updates are simple.
2. It can run on Windows, Linux, Mac, Android, iOS and other operating systems, and behaves consistently across all operating systems.
3. It provides a very simple API programming interface that can easily implement the paginated tag multi-document pattern.
# For Chinese developers
&nbsp; &nbsp; DCWriterLite has passed the original factory certification of domestic Kirin, Fangde and Tongxin operating systems, and complies with the rules of Xin Chuang.
# For end user
&nbsp; &nbsp; DCWriterLite provides simple yet not simple document editing capabilities:
1. It provides a user experience similar to MS Word, which can be used without learning.
2. It can perform accurate real-time paging, achieve the function of what you see is what you get, unlimited Undo/Redo operations, to help users quickly achieve goals.
3. Provides tables, title table rows and other functions to quickly create complex typesetting documents.
# Applicable scenarios
1. Structured electronic medical record system: to help doctors write the first page of the medical record, admission record, course record, examination report, nursing record, patient informed consent, discharge record and so on.
2. Government e-document system: helps users define various documents and dynamically adjust the content and status of documents according to business processes.
3. Financial business compliance document processing: Its unique structured document technology makes it easier for the system to extract key data for inspection and maintenance.
4. Manufacturing form system: It can quickly define the complex form input interface, help the manufacturing industry to input all kinds of complex node data one by one, and facilitate the back-end to read and analyze quickly.
# DCWriter Business Edition
&nbsp; &nbsp; If you want more features and services, please learn about [[the DCWriter Business Edition]](https://github.com/dcsoft-yyf/DCWriterLite/blob/main/README-be.md)) . 

# Screen snapshort
<img src="https://raw.githubusercontent.com/dcsoft-yyf/DCWriterLite/refs/heads/main/screensnapshort/dcwl-1.png"/>
<br/><img src="https://raw.githubusercontent.com/dcsoft-yyf/DCWriterLite/refs/heads/main/screensnapshort/dcwl-2.png"/>
<br/><img src="https://raw.githubusercontent.com/dcsoft-yyf/DCWriterLite/refs/heads/main/screensnapshort/dcwl-3.png"/>
<br/><img src="https://raw.githubusercontent.com/dcsoft-yyf/DCWriterLite/refs/heads/main/screensnapshort/dcwl-4.png"/>

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


&nbsp; &nbsp; DCWriterLite是一个开源的类似RTF的结构化文档编辑器，许多不同的人群都能从中获益：
# 对于C#开发者
&nbsp; &nbsp; 它展示了以下神奇的编程技巧
1. 在Blazor Webassembly编程中使用System.Drawing.Graphics和System.Windows.Forms.Control类型。这能帮助开发者将大量原本要抛弃的C#代码移植到WASM开发中，保护业界在相关领域的投资。
2. 它以尽可能精炼的方式实现了一套完整的D OM模型，很容易学习领会。而Open Office也定义了类似的DOM模型，但它包含了上千万行代码，超出了绝大多数人或团队的理解能力。
3. 这个项目展示了如何精简Blazor Wassembly框架。这能帮助开发者理解Blazor Wassembly的部分底层原理，并能减少最终的软件文件(dotnet.wasm)大小，提高软件加载速度。
# 对于toB的开发者
1. DCWriterLite提供了特色的结构化文书功能。它提供自由文本录入的功能，同时还能提供表单区域来限制特定区域的录入方式。
2. DCWriterLite实现了自定义的DOM模型，这就提供了无限的扩展能力。开发者可以据此创建新的文档元素类型，使得软件更符合具体业务需求。比如开发者可以用它来开发模板制作工具，来动态定义用户业务所需的各种复杂的表单界面。
3. DCWriterLite不是基于HTML DOM，而是自行实现了文档排版算法，这就突破了HTML DOM的一些技术限制。比如实现了可靠的Undo/Redo功能，而且在不同的浏览器间保持完全一致的排版结果。
4. DCWriterLite采用XML格式进行存储，结构简单。这就使得开发者可以很容易开发出后台程序来批量处理数百万份文档而不必调用DCWriterLite软件模块。
# 对于Web开发者
&nbsp; &nbsp; DCWriterLite提供以下令人舒服的特性：
1. 它是一个纯前端的组件，不依赖于第三方组件，支持Chrome和Firefox等主流浏览器，软件部署和更新很简单。
2. 它能运行在Windows、Linux、Mac、安卓、iOS等多种操作系统上面，在各种操作系统下的行为是一致的。
3. 它提供很简洁的API编程接口，可以很方便的实现分页标签多文档模式。
# 对于中国开发者
&nbsp; &nbsp; DCWriterLite通过了国产的麒麟、方德、统信操作系统的原厂认证，符合信创规则。

# 对于最终使用者
&nbsp; &nbsp; DCWriterLite提供了简约而不简单的文档编辑功能：
1. 它提供类似MS Word的用户体验，用户无需学习即可上手使用。
2. 它能进行精确的实时分页，实现了所见即所得的功能，无限制的Undo/Redo操作，帮助用户快速达成目标。
3. 提供表格、标题表格行等多项功能，可以快速创建复杂排版的文档。
# 能胜任的应用场景
1. 结构化电子病历系统：帮助医生书写病案首页、入院记录、病程记录、检查检验报告单、护理记录、患者知情同意书、出院记录等等等。
2. 政府电子公文系统：帮助用户定义各种公文，并根据业务流程动态的调整公文内容和状态。
3. 金融业务合规文档处理：它特有的结构化文档技术，使得系统更方便的从中抽取关键数据进行检查和维护。
4. 制造业表单系统：可以快速定义复杂的表单录入界面，帮助制造业将各种复杂的节点数据逐一录入，方便后台快速读取和分析。
# DCWriter商业版
&nbsp; &nbsp; 您若想要更多的功能和服务，请了解[【DCWriter商业版】](https://github.com/dcsoft-yyf/DCWriterLite/blob/main/README-be.md).

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

 
# Donate 捐助
You can donate by <a href="https://www.paypal.com/paypalme/yuanyongfu">paypal</a> , by <a href="https://raw.githubusercontent.com/dcsoft-yyf/DCNETProtector/main/alipay.jpg">alipay</a> , by <a href="https://raw.githubusercontent.com/dcsoft-yyf/DCNETProtector/main/wechat_pay.png">Wechat</a>,help author to feed twins born in 2020.
