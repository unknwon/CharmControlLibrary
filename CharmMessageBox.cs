#region 文档说明
/* *****************************************************************************************************
 * 文档作者：无闻
 * 创建日期：2012年10月19日
 * 文档用途：CharmMessageBox - 消息框控件
 * -----------------------------------------------------------------------------------------------------
 * 修改记录：
 * 2012-10-20：使用GDI+技术实现文本自动换行，智能调整框窗口宽度以及所有控件位置
 * 2013-02-20：针对CSBox界面标准2.0进行升级改造
 * -----------------------------------------------------------------------------------------------------
 * 参考文献：
 * 
 * *****************************************************************************************************/
#endregion

#region 命名空间引用
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using CharmCommonMethod;
#endregion

namespace CharmControlLibrary
{
    #region 枚举类型
    /// <summary>
    /// 消息框类型：蓝色天空
    /// </summary>
    public enum MessageBoxType : int
    {
        /// <summary>
        /// 消息框类型：蓝色天空
        /// </summary>
        BlueSky,
        /// <summary>
        /// 自定义消息框类型，需指定背景图像资源
        /// </summary>
        Customize
    }

    /// <summary>
    /// 消息框图标：询问，错误，信息，正常，警告
    /// </summary>
    public enum CharmMessageBoxIcon : int
    {
        /// <summary>
        /// 没有消息框图标
        /// </summary>
        None,
        /// <summary>
        /// 消息框图标：询问图标
        /// </summary>
        Question,
        /// <summary>
        /// 消息框图标：错误图标
        /// </summary>
        Error,
        /// <summary>
        /// 消息框图标：信息图标
        /// </summary>
        Infomation,
        /// <summary>
        /// 消息框图标：正常图标
        /// </summary>
        Ok,
        /// <summary>
        /// 消息框图标：警告图标
        /// </summary>
        Warning
    }
    #endregion

    /// <summary>
    /// 表示 CharmControlLibrary.CharmMessageBox 消息框控件
    /// </summary>
    public partial class CharmMessageBox : Form
    {
        #region 字段
        // 消息框类型
        private MessageBoxType mMessageBoxType;
        // 自定义的背景图像资源
        private Image mCustomizeBackgourndImage;
        // 消息框的标题栏文本颜色
        private Color mTitleColor;
        // 消息框的消息文本颜色
        private Color mTextColor;
        // 一个 MessageBoxResult 值，用于指定用户单击了哪个消息框按钮
        private DialogResult mDialogResult;
        // 消息框的按钮类型
        private MessageBoxButtons mButtonType;
        // 消息框的按钮
        private CharmButton[] mButtons;
        // 消息框的检查框
        private CharmCheckBox mCheckBox;
        // Charm控件集合
        private List<CharmControl> CharmControls;
        // 工具提示文本控件
        private ToolTip mToolTip = new ToolTip();
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置 CharmControlLibrary.CharmMessageBox 的消息框类型
        /// </summary>
        public MessageBoxType MessageBoxType
        {
            get { return this.mMessageBoxType; }
            set { this.mMessageBoxType = value; }
        }

        /// <summary>
        /// 获取或设置自定义的背景图像资源
        /// </summary>
        public Image CustomizeBackgourndImage
        {
            get { return this.mCustomizeBackgourndImage; }
            set { this.mCustomizeBackgourndImage = value; }
        }

        /// <summary>
        /// 获取或设置消息框的标题栏文本颜色
        /// </summary>
        public Color TitleColor
        {
            get { return this.mTitleColor; }
            set { this.mTitleColor = value; }
        }

        /// <summary>
        /// 获取或设置消息框的消息文本颜色
        /// </summary>
        public Color TextColor
        {
            get { return this.mTextColor; }
            set { this.mTextColor = value; }
        }

        /// <summary>
        /// 获取或设置消息框的检查框
        /// </summary>
        public CharmCheckBox CheckBox
        {
            get { return this.mCheckBox; }
            set { this.mCheckBox = value; }
        }
        #endregion

        #region 窗体事件
        /// <summary>
        /// 用默认设置初始化 CharmMessageBox 类的新实例
        /// </summary>
        public CharmMessageBox()
        {
            InitializeComponent();
            // 设置窗体阴影特效
            APIOperation.FormBorderShadow(this.Handle);
            // * 设置双缓冲模式 *
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | //不擦除背景 ,减少闪烁
                               ControlStyles.OptimizedDoubleBuffer | //双缓冲
                               ControlStyles.UserPaint, //使用自定义的重绘事件,减少闪烁
                               true);
            this.UpdateStyles();

            // * 初始化属性 *
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Width = 350;

            this.mTitleColor = Color.White;
            this.mTextColor = Color.Black;

            // 创建控件集合
            CharmControls = new List<CharmControl>();

            // 创建用户按钮
            this.mButtons = new CharmButton[3];
            for (int i = 0; i < mButtons.Length; i++)
            {
                mButtons[i] = new CharmButton();
                mButtons[i].ButtonType = ButtonType.Classic_Size_06922;
                mButtons[i].Visible = false;
                mButtons[i].MouseClick += new MouseEventHandler(ButtonMouseClick);
                CharmControls.Add(mButtons[i]);
            }

            // 创建检查框
            this.mCheckBox = new CharmCheckBox();
            this.mCheckBox.Visible = false;
            CharmControls.Add(mCheckBox);

            // 创建关闭按钮
            CharmSysButton btnClose = new CharmSysButton();
            btnClose.SysButtonType = SysButtonType.Close;
            btnClose.Location = new Point(this.Width - 46, 1);
            btnClose.MouseClick += new MouseEventHandler(btnClose_MouseClick);
            CharmControls.Add(btnClose);
        }

        // 窗体鼠标单击事件
        private void frmMain_MouseClick(object sender, MouseEventArgs e)
        {
            CharmControl.MouseClickEvent(e, CharmControls);
        }

        // 窗体鼠标按下事件
        private void frmMain_MouseDown(object sender, MouseEventArgs e)
        {
            // 调用事件
            if (!CharmControl.MouseDownEvent(e, CharmControls, this))
                APIOperation.MoveNoBorderForm(this, e);
        }

        // 窗体鼠标移动事件
        private void frmMain_MouseMove(object sender, MouseEventArgs e)
        {
            CharmControl.MouseMoveEvent(e, CharmControls, this, mToolTip);
        }

        // 窗体鼠标弹起事件
        private void frmMain_MouseUp(object sender, MouseEventArgs e)
        {
            CharmControl.MouseUpEvent(e, CharmControls, this);
        }
        #endregion

        #region  重载事件
        /// <summary>
        /// 引发 Paint 事件
        /// </summary>
        /// <param name="e">包含事件数据的 PaintEventArgs</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // 设置按钮全不可见
            for (int i = 0; i < 3; i++)
                CharmControls[i].Visible = false;

            // 设置消息框按钮
            switch (mButtonType)
            {
                case MessageBoxButtons.AbortRetryIgnore:
                    CharmControls[0].Text = "终止";
                    CharmControls[0].Visible = true;
                    CharmControls[1].Text = "重试";
                    CharmControls[1].Visible = true;
                    CharmControls[2].Text = "忽略";
                    CharmControls[2].Visible = true;
                    break;
                case MessageBoxButtons.OK:
                    CharmControls[2].Text = "确定";
                    CharmControls[2].Visible = true;
                    break;
                case MessageBoxButtons.OKCancel:
                    CharmControls[1].Text = "确定";
                    CharmControls[1].Visible = true;
                    CharmControls[2].Text = "取消";
                    CharmControls[2].Visible = true;
                    break;
                case MessageBoxButtons.RetryCancel:
                    CharmControls[1].Text = "重试";
                    CharmControls[1].Visible = true;
                    CharmControls[2].Text = "取消";
                    CharmControls[2].Visible = true;
                    break;
                case MessageBoxButtons.YesNo:
                    CharmControls[1].Text = "是";
                    CharmControls[1].Visible = true;
                    CharmControls[2].Text = "否";
                    CharmControls[2].Visible = true;
                    break;
                case MessageBoxButtons.YesNoCancel:
                    CharmControls[0].Text = "是";
                    CharmControls[0].Visible = true;
                    CharmControls[1].Text = "否";
                    CharmControls[1].Visible = true;
                    CharmControls[2].Text = "取消";
                    CharmControls[2].Visible = true;
                    break;
            }

            CharmControl.PaintEvent(e.Graphics, CharmControls);
        }
        #endregion

        #region 控件事件
        // 关闭按钮鼠标单击事件
        private void btnClose_MouseClick(object sender, MouseEventArgs e)
        {
            // 设置对话框结果
            mDialogResult = DialogResult.Cancel;
            // 关闭窗口并返回结果
            this.Close();
        }

        // 按钮鼠标单击事件
        private void ButtonMouseClick(object sender, MouseEventArgs e)
        {
            // 根据按钮标题设置对话框结果
            switch (((CharmControl)sender).Text)
            {
                case "确定":
                    mDialogResult = DialogResult.OK;
                    break;
                case "取消":
                    mDialogResult = DialogResult.Cancel;
                    break;
                case "终止":
                    mDialogResult = DialogResult.Abort;
                    break;
                case "重试":
                    mDialogResult = DialogResult.Retry;
                    break;
                case "忽略":
                    mDialogResult = DialogResult.Ignore;
                    break;
                case "是":
                    mDialogResult = DialogResult.Yes;
                    break;
                case "否":
                    mDialogResult = DialogResult.No;
                    break;
            }

            // 关闭窗口并返回结果
            this.Close();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 显示一个消息框，该消息框包含消息和标题栏标题，并且返回结果
        /// </summary>
        /// <param name="messageBoxText">一个 String，用于指定要显示的文本</param>
        /// <param name="caption">一个 String，用于指定要显示的标题栏标题</param>
        /// <returns>一个 DialogResult 值，用于指定用户单击了哪个消息框按钮</returns>
        public DialogResult Show(
            string messageBoxText,
            string caption)
        {
            return this.Show(messageBoxText, caption, MessageBoxButtons.OK, CharmMessageBoxIcon.Infomation, DialogResult.OK);
        }

        /// <summary>
        /// 显示一个消息框，该消息框包含消息、标题栏标题和按钮，并且返回结果
        /// </summary>
        /// <param name="messageBoxText">一个 String，用于指定要显示的文本</param>
        /// <param name="caption">一个 String，用于指定要显示的标题栏标题</param>
        /// <param name="button">一个 MessageBoxButton 值，用于指定要显示哪个按钮或哪些按钮</param>
        /// <returns>一个 DialogResult 值，用于指定用户单击了哪个消息框按钮</returns>
        public DialogResult Show(
            string messageBoxText,
            string caption,
            MessageBoxButtons button)
        {
            return this.Show(messageBoxText, caption, button, CharmMessageBoxIcon.Infomation, DialogResult.OK);
        }

        /// <summary>
        /// 显示一个消息框，该消息框包含消息、标题栏标题、按钮和图标，并且返回结果
        /// </summary>
        /// <param name="messageBoxText">一个 String，用于指定要显示的文本</param>
        /// <param name="caption">一个 String，用于指定要显示的标题栏标题</param>
        /// <param name="button">一个 MessageBoxButton 值，用于指定要显示哪个按钮或哪些按钮</param>
        /// <param name="icon">一个 CharmMessageBoxIcon 值，用于指定要显示的图标</param>
        /// <returns>一个 DialogResult 值，用于指定用户单击了哪个消息框按钮</returns>
        public DialogResult Show(
            string messageBoxText,
            string caption,
            MessageBoxButtons button,
            CharmMessageBoxIcon icon)
        {
            return this.Show(messageBoxText, caption, button, icon, DialogResult.OK);
        }

        /// <summary>
        /// 显示一个消息框，该消息框包含消息、标题栏标题、按钮和图标，并接受默认消息框结果和返回结果
        /// </summary>
        /// <param name="messageBoxText">一个 String，用于指定要显示的文本</param>
        /// <param name="caption">一个 String，用于指定要显示的标题栏标题</param>
        /// <param name="button">一个 MessageBoxButton 值，用于指定要显示哪个按钮或哪些按钮</param>
        /// <param name="icon">一个 CharmMessageBoxIcon 值，用于指定要显示的图标</param>
        /// <param name="defaultResult">一个 DialogResult 值，用于指定消息框的默认结果</param>
        /// <returns>一个 DialogResult 值，用于指定用户单击了哪个消息框按钮</returns>
        public DialogResult Show(
            string messageBoxText,
            string caption,
            MessageBoxButtons button,
            CharmMessageBoxIcon icon,
            DialogResult defaultResult)
        {
            #region 旧版的文本换行写法
            //using (Graphics g = Graphics.FromImage(base.BackgroundImage))
            //{
            //    byte[] data = Encoding.Default.GetBytes(text);
            //    int index = 0;
            //    byte[] cache;
            //    int minLength;

            //    while (data.Length >= (index + 1) * _textWidth)
            //    {
            //        cache = new byte[_textWidth];
            //        minLength = data.Length > (index + 1) * _textWidth ? (index + 1) * _textWidth : data.Length - 1;
            //        for (int i = index * _textWidth; i < minLength; i++)
            //        {
            //            cache[i - index * _textWidth] = data[i];
            //        }
            //        g.DrawString(Encoding.Default.GetString(cache), new Font("微软雅黑", 12), Brushes.Black,
            //            new Point(_textPoint.X, _textPoint.Y + index * 15));
            //        index++;
            //    }

            //    cache = new byte[_textWidth];
            //    minLength = data.Length > (index + 1) * _textWidth ? (index + 1) * _textWidth : data.Length - 1;
            //    for (int i = index * _textWidth; i <= minLength; i++)
            //    {
            //        cache[i - index * _textWidth] = data[i];
            //    }
            //    g.DrawString(Encoding.Default.GetString(cache), new Font("微软雅黑", 12), Brushes.Black,
            //        new Point(_textPoint.X, _textPoint.Y + index * 15));
            //}
            #endregion

            // 计算需要绘制的文本高度
            Graphics g = CreateGraphics();
            SizeF sf = g.MeasureString(messageBoxText, new Font("微软雅黑", 9), this.Width - 90);
            // 修改窗体高度
            this.Height = (int)sf.Height + 120;
            // 根据消息框类型设置背景图像资源并修改尺寸
            switch (MessageBoxType)
            {
                case MessageBoxType.BlueSky:
                    this.BackgroundImage = Properties.Resources.messagebox_bluesky;
                    this.BackgroundImage =
                        ImageOperation.ResizeImageWithoutBorder(this.BackgroundImage, 2, 30, 2, 2, this.Size);
                    break;
                case MessageBoxType.Customize:
                    if (mCustomizeBackgourndImage == null)  // 检查用户是否完成对消息框的初始化
                        throw new ArgumentNullException(
                            "CharmControlLibrary.CharmMessageBox：未指定自定义消息框类型的背景图像资源.\n" +
                            "Name: " + this.Name);
                    this.BackgroundImage = new Bitmap(mCustomizeBackgourndImage, this.Size);
                    break;
            }

            using (g = Graphics.FromImage(this.BackgroundImage))
            {
                // 绘制标题栏文本
                g.DrawString(caption, new Font("微软雅黑", 10, FontStyle.Bold), new SolidBrush(mTitleColor), new Point(8, 6));
                // 绘制消息框图标
                switch (icon)
                {
                    case CharmMessageBoxIcon.None:
                        break;
                    case CharmMessageBoxIcon.Question:
                        g.DrawImage(Properties.Resources.messageboxicon_question,
                            new Rectangle(new Point(27, (int)sf.Height / 2 + 50), Properties.Resources.messageboxicon_question.Size));
                        break;
                    case CharmMessageBoxIcon.Error:
                        g.DrawImage(Properties.Resources.messageboxicon_error,
                            new Rectangle(new Point(27, (int)sf.Height / 2 + 50), Properties.Resources.messageboxicon_error.Size));
                        break;
                    case CharmMessageBoxIcon.Infomation:
                        g.DrawImage(Properties.Resources.messageboxicon_info,
                            new Rectangle(new Point(27, (int)sf.Height / 2 + 50), Properties.Resources.messageboxicon_info.Size));
                        break;
                    case CharmMessageBoxIcon.Ok:
                        g.DrawImage(Properties.Resources.messageboxicon_ok,
                            new Rectangle(new Point(27, (int)sf.Height / 2 + 50), Properties.Resources.messageboxicon_ok.Size));
                        break;
                    case CharmMessageBoxIcon.Warning:
                        g.DrawImage(Properties.Resources.messageboxicon_warning,
                            new Rectangle(new Point(27, (int)sf.Height / 2 + 50), Properties.Resources.messageboxicon_warning.Size));
                        break;
                }
                // 绘制消息文本
                RectangleF rf = new RectangleF(75, 60, sf.Width, sf.Height);
                g.DrawString(messageBoxText, new Font("微软雅黑", 9), new SolidBrush(mTextColor), rf);
            }

            // 设置按钮类型及坐标
            mButtonType = button;
            CharmControls[0].Location = new Point(89, (int)sf.Height + 85);
            CharmControls[1].Location = new Point(178, (int)sf.Height + 85);
            CharmControls[2].Location = new Point(265, (int)sf.Height + 85);
            // 设置检查框坐标
            CharmControls[3].Location = new Point(30, (int)sf.Height + 84);

            // 显示消息框
            this.ShowDialog();
            return mDialogResult;
        }
        #endregion
    }
}
