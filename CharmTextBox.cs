#region 文档说明
/* ****************************************************************************************************
 * 文档作者：无闻
 * 创建日期：2012年10月6日
 * 文档用途：CharmTextBox - 文本框控件
 * -----------------------------------------------------------------------------------------------------
 * 修改记录：
 * 2013-01-20（无闻）：
 *  - 将类名从 cTexxBox 修改为 CharmTextBox
 *  - 规范类字段、属性的命令及相关方法
 * 2013-01-21（无闻）：修复 边框出现白框 的问题
 * 2013-03-01：针对CSBox界面标准2.0进行升级改造
 * -----------------------------------------------------------------------------------------------------
 * 参考文献：
 * 
 * *****************************************************************************************************/
#endregion

#region 命名空间引用
using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using CharmCommonMethod;
#endregion

namespace CharmControlLibrary
{
    #region 枚举类型
    /// <summary>
    /// 文本框输入模式：通常方式，只读方式，密码输入，整数输入
    /// </summary>
    public enum InputMode : int
    {
        /// <summary>
        /// 通常方式，允许用户对文本框进行所有操作
        /// </summary>
        Normal,
        /// <summary>
        /// 只读方式，不允许用户进行任何输入
        /// </summary>
        ReadOnly,
        /// <summary>
        /// 密码输入，文本框将以密码框的形式呈现
        /// </summary>
        Password,
        /// <summary>
        /// 整数输入，只允许用户输入数字（0-9）或退格键
        /// </summary>
        Integer
    }
    #endregion

    /// <summary>
    /// 表示 CharmControlLibrary.CharmTextBox 文本框控件
    /// </summary>
    public class CharmTextBox : PictureBox
    {
        #region 字段
        // 控件的文本框输入模式
        private InputMode mInputMode;
        // 文本框控件
        private TextBox mTextbox;
        // 控件的状态
        private ControlStatus mControlStatus;
        // 控件的状态图片
        private Image[] mStatusImages;
        // 控件的水印文本
        private string mWatermark;
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置控件的宽度
        /// </summary>
        public new int Width
        {
            get { return base.Width; }
            set
            {
                base.Width = value;
                // 重新设置文本框大小
                mTextbox.Size = new Size(base.Width - 4, mTextbox.Height);
                mStatusImages[0] = ImageOperation.ResizeImageWithoutBorder(mStatusImages[0], 2, 1, 2, 1, base.Size);
                mStatusImages[1] = ImageOperation.ResizeImageWithoutBorder(mStatusImages[1], 2, 2, 2, 2, base.Size);
            }
        }

        /// <summary>
        /// 获取或设置控件中文本的水平对齐方式
        /// </summary>
        public HorizontalAlignment TextAlign
        {
            get { return mTextbox.TextAlign; }
            set { mTextbox.TextAlign = value; }
        }

        /// <summary>
        /// 获取或设置控件的文本框输入模式
        /// </summary>
        public InputMode TextInputMode
        {
            get { return mInputMode; }
            set
            {
                mInputMode = value;

                // 如果输入类型为只读类型或密码输入则无水印效果
                if (mInputMode == InputMode.ReadOnly || mInputMode == InputMode.Password)
                {
                    this.mWatermark = null;
                    if (mInputMode == InputMode.Password)
                        mTextbox.PasswordChar = '●';     // 密码输入
                }
            }
        }

        /// <summary>
        /// 获取或设置控件的水印文本
        /// </summary>
        public string Watermark
        {
            get { return this.mWatermark; }
            set
            {
                this.mWatermark = value;
                // 判断文本是否为空
                if (string.Equals(mTextbox.Text, string.Empty))
                {
                    mTextbox.ForeColor = Color.DarkGray;
                    mTextbox.Text = mWatermark;
                }
            }
        }

        /// <summary>
        /// 获取或设置用户可在控件内输入的最大字符数
        /// </summary>
        public int MaxLength
        {
            get { return mTextbox.MaxLength; }
            set
            {
                // 判断是否为非负整数
                if (value >= 0)
                    mTextbox.MaxLength = value;
                else
                    throw (new ArgumentException("MaxLength:文本框最大可输入字符数必须是非负整数."));
            }
        }

        /// <summary>
        /// 获取或设置文本框的内容
        /// </summary>
        public new string Text
        {
            get { return mTextbox.Text; }
            set { mTextbox.Text = value; }
        }
        #endregion

        #region 重载事件
        /// <summary>
        /// 控件重绘事件
        /// </summary>
        protected override void OnPaint(PaintEventArgs pe)
        {
            // 获取绘制对象
            Graphics g = pe.Graphics;

            // 判断控件状态，绘制背景
            switch (mControlStatus)
            {
                case ControlStatus.Normal:
                    g.DrawImage(mStatusImages[0], new Rectangle(new Point(0, 0), mStatusImages[0].Size));
                    break;
                case ControlStatus.Hover:
                    g.DrawImage(mStatusImages[1], new Rectangle(new Point(0, 0), mStatusImages[1].Size));
                    break;
            }
        }
        #endregion

        #region 自定义事件
        /// <summary>
        /// 定义文本框双击事件委托
        /// </summary>
        public delegate void DoubleClickEventHandler(object sender, EventArgs e);
        /// <summary>
        /// 当双击控件时发生
        /// </summary>
        public new event DoubleClickEventHandler DoubleClick;

        /// <summary>
        /// 定义文本框文本改变事件委托
        /// </summary>
        public delegate void TextChangedEventHandler(object sender, EventArgs e);
        /// <summary>
        /// 当控件文本改变时发生
        /// </summary>
        public new event TextChangedEventHandler TextChanged;

        // 鼠标进入事件
        private void TextBox_MouseEnter(object sender, EventArgs e)
        {
            // 修改背景
            this.mControlStatus = ControlStatus.Hover;
            this.Invalidate();

            // 判断是否有水印
            if (mWatermark != null)
            {
                // 判断文本是否为默认文本
                if (string.Equals(mTextbox.Text, mWatermark))
                {
                    mTextbox.Text = string.Empty;
                    mTextbox.ForeColor = Color.Black;
                }
            }
        }

        // 鼠标离开事件
        private void TextBox_MouseLeave(object sender, EventArgs e)
        {
            // 修改背景
            this.mControlStatus = ControlStatus.Normal;
            this.Invalidate();

            // 判断是否拥有焦点且有水印
            if (!mTextbox.Focused)
            {
                if (mWatermark != null)
                {
                    // 判断文本是否为空
                    if (string.Equals(mTextbox.Text, string.Empty))
                    {
                        mTextbox.ForeColor = Color.DarkGray;
                        mTextbox.Text = mWatermark;
                    }
                }
            }
        }

        // 键盘按键事件
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 判断输入类型
            switch (mInputMode)
            {
                case InputMode.Normal:
                    break;
                case InputMode.ReadOnly:
                    e.Handled = true;
                    break;
                case InputMode.Password:
                    break;
                case InputMode.Integer:
                    if (!(char.IsDigit(e.KeyChar) | e.KeyChar == '\b'))
                    {
                        e.Handled = true;
                    }
                    break;
            }
        }

        // 获得焦点事件
        private void TextBox_GotFocus(object sender, EventArgs e)
        {
            // 判断是否有水印
            if (mWatermark != null)
            {
                // 判断文本是否为默认文本
                if (string.Equals(mTextbox.Text, mWatermark))
                {
                    mTextbox.Text = string.Empty;
                    mTextbox.ForeColor = Color.Black;
                }
            }
        }

        // 失去焦点事件
        private void TextBox_LostFocus(object sender, EventArgs e)
        {
            // 判断是否有水印
            if (mWatermark != null)
            {
                // 判断文本是否为空
                if (string.Equals(mTextbox.Text, string.Empty))
                {
                    mTextbox.ForeColor = Color.DarkGray;
                    mTextbox.Text = mWatermark;
                }
            }
        }
        #endregion

        #region 控件事件
        // 文本框被双击事件
        private void mTextbox_DoubleClick(object sender, EventArgs e)
        {
            if (this.DoubleClick != null)
                this.DoubleClick(sender, e);
        }

        // 文本改变事件
        private void mTextbox_TextChanged(object sender, EventArgs e)
        {
            if (this.TextChanged != null)
                this.TextChanged(sender, e);
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 初始化 CharmTextBox 类的新实例
        /// </summary>
        public CharmTextBox()
            : base()
        {
            // * 初始化属性 *
            this.Size = new Size(160, 26);
            this.BackColor = Color.White;
            this.mStatusImages = new Image[2];
            this.mStatusImages[0] = Properties.Resources.textbox_bkg_normal;
            // 去除常态背景空白部分
            this.mStatusImages[0] =
                ImageOperation.GetPartOfImage(mStatusImages[0], mStatusImages[0].Width, mStatusImages[0].Height - 2, 0, 1);
            this.mStatusImages[1] = Properties.Resources.textbox_bkg_hover;
            // 重新设置文本框大小
            mStatusImages[0] = ImageOperation.ResizeImageWithoutBorder(mStatusImages[0], 2, 1, 2, 1, base.Size);
            mStatusImages[1] = ImageOperation.ResizeImageWithoutBorder(mStatusImages[1], 2, 2, 2, 2, base.Size);

            // * 文本框外观样式设置 *
            mTextbox = new TextBox();
            mTextbox.BorderStyle = BorderStyle.None; // 无边框
            mTextbox.Font = new Font("微软雅黑", 9.75F);   // 字体
            mTextbox.Size = new Size(156, 20);    // 大小
            mTextbox.Location = new Point(2, (this.Height - mTextbox.Height) / 2);   // 位置
            mTextbox.ImeMode = ImeMode.NoControl; // 输入法设置
            // 设置水印
            if (mWatermark != null)
            {
                mTextbox.ForeColor = Color.DarkGray;
                mTextbox.Text = mWatermark;
            }

            // * 控件属性设置 *
            mInputMode = InputMode.Normal;   // 输入模式

            // * 关联控件事件 *
            mTextbox.MouseEnter += new EventHandler(TextBox_MouseEnter);
            mTextbox.MouseLeave += new EventHandler(TextBox_MouseLeave);
            mTextbox.KeyPress += new KeyPressEventHandler(TextBox_KeyPress);
            mTextbox.LostFocus += new EventHandler(TextBox_LostFocus);
            mTextbox.GotFocus += new EventHandler(TextBox_GotFocus);
            mTextbox.DoubleClick += new EventHandler(mTextbox_DoubleClick);
            mTextbox.TextChanged += new EventHandler(mTextbox_TextChanged);

            // * 加载控件到控件集合中 *
            this.Controls.Add(mTextbox);
        }
        #endregion

        #region 方法
        /// <summary>
        /// 设置文本框文本，用于有水印时正常显示内容
        /// </summary>
        /// <param name="text">文本内容</param>
        public void SetText(string text)
        {
            mTextbox.ForeColor = Color.Black;
            mTextbox.Text = text;
        }
        #endregion
    }
}
