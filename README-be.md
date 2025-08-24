# DCWriter for business edition
August 18, 2025
<img src="https://raw.githubusercontent.com/dcsoft-yyf/DCWriterLite/refs/heads/main/screensnapshort/dc-be.jpg"/>
(Support document comments ,water mark ,gutter,td-barcode .)
# DCWriter Business Edition Feature List 
 DCWriter Business Edition adds the following features on the basis of the open-source version:
## Runtime Environment
- Supports Blazor WASM 7.0/8.0/9.0 to support lower-version browsers.
## File Operations
- Open files, supporting RTF, JSON, OFD, and HTML formats.
- Save files, supporting RTF, JSON, OFD, PDF, and HTML formats.
- Print Settings
- Enable or disable headers and footers
- Continue printing
- Selective area printing
- Text and image watermarks
- Blank marks at the bottom of each page (text, slash, and S-line)
- Binding margin
- Alternate margins for odd and even pages
- Different headers and footers for the first page
- Binding margin (left, top, and right)
- Do not print background color
- Print text and lines in pure black to adapt to black-and-white printers.
## View Modes
- Form view mode
- Can be set to form view mode, where user operations are restricted to input field elements. Content outside the input fields cannot be selected or edited.
- Reading view mode. Document content is read-only, similar to print preview, but text selection and copying are allowed.
- Long document view mode. No pagination processing, the document body is displayed and edited continuously without interruption. 
## Four-level Permission Content Control
- Users can log in and have four permission levels.
- Users with higher permissions can modify or delete content created by users with lower permissions.
- Users with lower permissions cannot modify content created by users with higher permissions.
- Logical deletion is supported.
- Different permission levels can be set with different text styles, including the color of strikethrough and underline.
- Logically deleted content can be displayed or hidden.
- Document content can be displayed in trace mode or clean mode.
## Document Annotation
- Supports adding and deleting document annotations.
- Supports user-defined annotations through programming.
## Multi-language Support
- Supports right-to-left languages such as Arabic and Hebrew.
- Supports Tibetan.
- Supports rare characters.
## Layout
- Supports right-to-left layout.
- Input fields support single-line font size reduction for automatic filling.
- Table cells support single-line or multi-line font size reduction for automatic filling.
- HTML code can be used to customize the display and behavior of document annotations.
## Data Source Binding
- Supports data source binding, with data sources in formats such as XML.
- Supports dynamic updates of data sources.
## Numerical Operation Formulas
- Supports numerical operation formulas for calculations within input field elements.
- Supports formula calculations, which can be simple arithmetic operations or complex mathematical formulas. Formula syntax follows Excel.
- Formulas can set the text content and visibility of document elements.
## Text Styles
- Supports setting the color of underlines separately.
- Supports wavy underlines.
- Adds emphasis dots under characters.
- Character borders, including circular and square.
## Document Area Content Protection
- Can set parts of a text as read-only, prohibiting editing and deletion.
- Can set tables, table rows, cells, input field elements, checkboxes, etc. as read-only. The read-only status can be inherited in the DOM tree.
## Enhanced DOM Structure
Compared to the open-source version, the commercial version enhances the DOM model:
- XTextImageElement Image Element
  - Can set the image to float above or below the text.
  - Can set text to wrap around the image.
- XTextLabelElement Text Label Element
  - Can be set to automatically calculate size.
  - Can be set as multi-line text.
  - Text alignment (left, center, right)
- XTextButtonElement Button Element
  - Can set the button text.
  - Can set the button color.
  - Can set the button size.
- XTextHorizontalLineElement Horizontal Line Element
  - Can set the thickness and color of the line.
- XTextContainerElement Container Element
  - Can set the maximum allowed length of input text.
  - Can set the allowed character set (e.g., only allowing numbers, letters, etc.).
- XTextInputFieldElement Input Field Element
  - Set border text
  - Set the label text before and the unit text after
  - Set the color of the background text.
  - Set the text color.
  - Display small buttons based on data type.
  - Display a small square in the lower right corner to indicate the status.
  - XTextButtonElement Button Element
  - Can set different images to be displayed when the mouse is pressed and released.
- XTextPageInfoElement Page Number Element
  - Page numbers can be formatted (Arabic numerals, Roman numerals, uppercase Roman numerals).
  - The starting value of page numbers can be set.
  - A formatted string can be set to display the page number and total number of pages in a page number element.
- XTextCheckBoxElement/XTextRadioElement: checkbox/radio button elements.
  - Text can be set to flow layout.
  - Text can be placed to the left or right of the checkbox.
  - Data source binding is supported.
  - Check history can be recorded.
- XTextImageElement: image element.
  - More image formats are supported.
  - Images can be loaded from URLs.
- XTextNewBarcodeElement: one-dimensional barcode element.
  - Text content can be displayed below the barcode.
- XTextSectionElement: document section element.
- XTextSubDocumentElement: subdocument element.
- XTextTDBarcodeElement: two-dimensional barcode element.
  - The content of the QR code can be set.
  - The size of the QR code can be set.
  - The error correction level of the QR code can be set (L, M, Q, H).
- XTextChartElement: chart element.
  - The type of the chart can be set (bar chart, line chart, pie chart, etc.).
  - The data source of the chart can be set.
  - The style and color of the chart can be set.
- XTextPieElement: pie chart element.
  - The sector colors of the pie chart can be set.
  - The label text of the pie chart can be set.
- XTextDirectoryFieldElement: directory field element.
  - The title text of the directory can be set.
  - The depth of the directory hierarchy can be set.
- XTextControlHostElement: control host element.
  - User-defined HTML elements can be embedded.
- XTextMediaElement: media element.
  - Video and audio files can be embedded.
- XTextNewBarcodeElement: one-dimensional barcode element.
  - The type of the barcode can be set (Code128, Code39, EAN13, UPC, etc.).
  - The content of the barcode can be set.
  - The size of the barcode can be set.

=====================================

#  DCWriter商业版功能清单 

<br/>  DCWriter商业版在开源版的基础上增加了以下功能：
## 运行环境
  - 支持Blazor WASM 7.0/8.0/9.0，用于支持低版本的浏览器。
## 文件操作
  - 打开文件，支持RTF、JSON、OFD、HTML格式。
  - 保存文件，支持RTF、JSON、OFD、PDF、HTML格式。
  - 打印设置
     - 是否启用禁用页眉页脚
     - 续打
     - 区域选择打印
     - 文字水印和图片水印
     - 每页最下面的空白标记（文字、斜线和S线）
     - 装订线
     - 奇偶页边距交替
     - 首页页眉页脚不同
     - 装订线（靠左、上、右三种）
     - 不打印背景色
     - 以纯黑色打印文字和线条，以适应黑白打印机。
## 视图模式
  - 表单视图模式
     - 可以设置为表单视图模式，用户操作只能限制在输入域元素中。输入域之外的内容
       不能被选中和编辑。
  - 阅读视图模式。文档内容只读，和打印预览类似，但可以进行文本选择和复制。
  - 长文档视图模式。不进行分页处理，文档正文不中断的显示和编辑。

## 四级权限内容管控
  - 可以进行用户登录操作，用户具有四个权限等级。
  - 高用户权限可以修改删除低用户权限的内容。
  - 低用户权限不能修改高权限用户输入的内容。
  - 支持逻辑删除。
  - 不同的权限等级可以设置不同的文本样式。包括删除线的颜色，输入内容的下划线颜色。
  - 可以显示和隐藏逻辑删除的内容。
  - 可以用留痕模式显示文档内容。
  - 可以用清洁模式显示文档内容。
## 文档批注
  - 支持添加和删除文档批注。
  - 支持用户编程自定义文档批注。
## 多语言支持
  - 支持阿拉伯语、希伯来语等从右到左的语言。
  - 支持藏文。
  - 支持生僻字。
## 排版
  - 支持从右到左排版。
  - 输入域支持单行缩小字体自动填充。
  - 表格单元格支持单行或多行缩小字体自动填充。
  - 可以使用HTML代码来自定义文档批注的显示和行为。
## 数据源绑定
  - 支持数据源绑定，数据源可以是XML等格式。
  - 支持数据源的动态更新。
## 数值运算公式
  - 支持数值运算公式，可以在输入域元素中进行数值计算。
  - 支持公式计算，公式可以是简单的加减乘除，也可以是复杂的数学公式。公式语法参考EXCEL。
  - 公式可以设置文档元素的文本内容和可见性。
## 文本样式
  - 支持单独设置下划线的颜色
  - 支持设置波浪形下划线
  - 字符下面添加着重圆点
  - 字符边框，包括圆形和方形
## 文档区域内容保护
  - 可以在一段文本中设置部分内容为只读状态，禁止编辑和删除。
  - 可以设置表格、表格行、单元格、输入域元素、勾选框等为只读状态。只读状态可以在DOM树中进行继承判断。
## 增强DOM结构
<br/>  相对于开源版本，商业版增强了DOM模型：
 - XTextImageElement 图片元素
      - 可以设置图片悬浮在文本上面或者下面。
      - 可以设置文字环绕图片。
 - XTextLabelElement 文本标签元素
      - 可以设置为自动计算大小
      - 可以设置为多行文本
      - 文本对齐方式（靠左、居中、靠右）
 - XTextButtonElement 按钮元素
      - 可以设置按钮的文本
      - 可以设置按钮的颜色
      - 可以设置按钮的大小
 - XTextHorizontalLineElement 横线元素
      - 可以设置横线的粗细和颜色
 - XTextContainerElement 容器元素
      - 可以设置最大允许的输入的文本长度。
      - 可以设置允许输入的字符集（如仅允许输入数字、字母等）。
 - XTextInputFieldElement 输入域元素
      - 设置边框文本
      - 设置前置的标签文本和后置的单位文本
      - 设置背景文本的颜色
      - 设置文本颜色
      - 根据数据类型显示小按钮
      - 右小角显示表示状态的小方块
 - XTextButtonElement 按钮元素
      - 可以设置鼠标按下和鼠标松开时显示不同的图片
 - XTextPageInfoElement 页码元素
      - 可以设置页码的格式（阿拉伯数字、罗马数字、大写罗马数字）
      - 可以设置页码的起始值
      - 可以设置格式化字符串，在一个页码元素中显示页码和总页数
 - XTextCheckBoxElement/XTextRadioElement 单选框/单选按钮元素
      - 可以将文本设置为流式排版
      - 文本可以设置为放在勾选框的左边或者右边
      - 可以数据源绑定
      - 可以记录勾选历史记录。
 - XTextImageElement 图片元素
      - 支持更多的图片格式
      - 支持从URL加载图片。
 - XTextNewBarcodeElement 一维码元素
      - 可以在条码下面显示文本内容
 - XTextSectionElement 文档节元素
 - XTextSubDocumentElement 子文档元素
 - XTextTDBarcodeElement 二维码元素
      - 可以设置二维码的内容
      - 可以设置二维码的大小
      - 可以设置二维码的纠错级别（L、M、Q、H）
 - XTextChartElement 图表元素
      - 可以设置图表的类型（柱状图、折线图、饼图等）
      - 可以设置图表的数据源
      - 可以设置图表的样式和颜色
 - XTextPieElement 饼图元素
      - 可以设置饼图的扇区颜色
      - 可以设置饼图的标签文本
 - XTextDirectoryFieldElement 目录域元素
      - 可以设置目录的标题文本
      - 可以设置目录的层级深度
 - XTextControlHostElement 控件宿主元素
      - 可以嵌入用户自定义的HTML元素
 - XTextMediaElement 媒体元素
      - 可以嵌入视频和音频文件
 - XTextNewBarcodeElement 一维码元素
      - 可以设置条形码的类型（Code128、Code39、EAN13、UPC等）
      - 可以设置条形码的内容
      - 可以设置条形码的大小
