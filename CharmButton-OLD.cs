#region 文档说明
/* ****************************************************************************************************
 * 文档作者：无闻
 * 创建日期：2012年10月6日
 * 文档用途：通过图片框实现更友好的 Button 控件样式以及更多自定义的控件属性
 * -----------------------------------------------------------------------------------------------------
 * 修改记录：
 * 2012-10-18（无闻）：将类名从 cButton 修改为 CharmButton
 * 2012-10-19（无闻）：
 *  - 规范类字段、属性的命令及相关方法
 *  - 增加 按钮文本最大长度限制、检测及异常抛出
 * 2012-10-23（无闻）：规定鼠标按下、弹起事件只响应鼠标左键
 * 2012-11-10（无闻）：取消 按钮文本最大长度限制的检测及异常抛出
 * 2013-01-21（无闻）：取消自定义单击事件，改用默认的单击事件委托
 * 2013-01-23（无闻）：
 *  - 增加 属性 Enabled
 *  - 取消 属性 Color
 * -----------------------------------------------------------------------------------------------------
 * 参考文献：
 * C#控件自绘：http://www.3600gz.cn/thread-122184-1-2.html
 * *****************************************************************************************************/
#endregion

#region 命名空间引用
using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
#endregion

namespace CharmControlLibrary
{
    /// <summary>
    /// CharmButton：可指定标题文本，按钮样式，按钮类型，按钮状态，文本颜色等属性
    /// </summary>
    public class CharmButton : PictureBox
    {
        #region 枚举类型
        /// <summary>
        /// 按钮类型（用于指定按钮大小和按钮样式）
        /// </summary>
        public enum ButtonType : int
        {
            /// <summary>
            /// 按钮样式：大小为 69*22
            /// </summary>
            Size_06922,
            /// <summary>
            /// 按钮样式：大小为 88*23
            /// </summary>
            Size_08223,
            /// <summary>
            /// 按钮样式：大小为 124*25
            /// </summary>
            Size_12425,
            /// <summary>
            /// 用户自定义按钮四个状态的图片和大小
            /// </summary>
            Customize
        }

        /// <summary>
        /// 按钮状态：常态，鼠标悬浮态，鼠标按下态，失活态
        /// </summary>
        public enum ButtonState : int
        {
            /// <summary>
            /// 常态：正常情况下
            /// </summary>
            Normal,
            /// <summary>
            /// 鼠标悬浮态：鼠标置于控件上方时
            /// </summary>
            Hover,
            /// <summary>
            /// 鼠标按下态：鼠标按下控件时
            /// </summary>
            Down,
            /// <summary>
            /// 失活态：控件被禁止时
            /// </summary>
            Unenabled
        }
        #endregion

        #region 字段
        private string _text; // 标题文本
        private int _textLength;    // 标题文本长度
        private Point _position; // 控件位置
        private Color textColor;   // 文本颜色
        private ButtonType buttonType;  // 按钮类型
        private ButtonState buttonState; // 按钮状态
        private bool enabled = true;      // 按钮激活状态
        private Image _imgNormal; // 按钮常态样式
        private Image _imgHover;   // 按钮鼠标悬浮态样式
        private Image _imgDown;    // 按钮鼠标按下态样式
        private Image _imgUnenabled;   // 按钮失活态样式
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置 CharmControlLibrary.CharmButton 的标题文本
        /// </summary>
        public override string Text
        {
            get { return this._text; }
            set
            {
                CheckTextLength(value);
                this._text = value;
                DrawImage();
            }
        }

        /// <summary>
        /// 获取或设置 CharmControlLibrary.CharmButton 的控件位置
        /// </summary>
        public Point Position
        {
            get { return this._position; }
            set { this._position = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示控件是否可以对用户交互作出响应
        /// </summary>
        public new bool Enabled
        {
            get { return this.enabled; }
            set
            {
                this.enabled = value;
                base.Enabled = value;
                if (value)
                    buttonState = ButtonState.Normal;
                DrawImage();
            }
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 将创建一个拥有指定标题文本，控件位置，按钮类型，文本颜色的 CharmButton
        /// <param name="text">标题文本</param>
        /// <param name="point">控件位置</param>
        /// <param name="buttonType">按钮类型</param>
        /// <param name="color">按钮文本颜色</param>
        /// </summary>
        public CharmButton(
            string text,
            Point point,
            ButtonType buttonType,
            Color color)
        {
            CheckTextLength(text);
            this._text = text;
            this._position = point;
            this.buttonType = buttonType;
            // 根据按钮类型指定按钮样式
            if (buttonType == ButtonType.Size_06922)
            {
                _imgNormal = Properties.Resources.btn_06922_normal;
                _imgHover = Properties.Resources.btn_06922_hover;
                _imgDown = Properties.Resources.btn_06922_down;
                _imgUnenabled = Properties.Resources.btn_06922_unenabled;
            }
            else if (buttonType == ButtonType.Size_08223)
            {
                _imgNormal = Properties.Resources.btn_08223_normal;
                _imgHover = Properties.Resources.btn_08223_hover;
                _imgDown = Properties.Resources.btn_08223_down;
                _imgUnenabled = Properties.Resources.btn_08223_unenabled;
            }
            else if (buttonType == ButtonType.Size_12425)
            {
                _imgNormal = Properties.Resources.btn_12425_normal;
                _imgHover = Properties.Resources.btn_12425_hover;
                _imgDown = Properties.Resources.btn_12425_down;
                _imgUnenabled = Properties.Resources.btn_12425_Unenabled;
            }
            // 不指定按钮状态则默认为常态
            this.buttonState = ButtonState.Normal;
            this.textColor = color;
            InitializeSetting();
        }

        /// <summary>
        /// 将创建一个拥有指定标题文本，控件位置，按钮类型，文本颜色，按钮状态，自定义样式资源的 CharmButton
        /// </summary>
        /// <param name="text">标题文本</param>
        /// <param name="point">控件位置</param>
        /// <param name="buttonType">按钮类型</param>
        /// <param name="color">文本颜色</param>
        /// <param name="buttonState">按钮状态</param>
        /// <param name="imgNormal">常态样式资源</param>
        /// <param name="imgHover">鼠标悬浮态样式资源</param>
        /// <param name="imgDown">鼠标按下态样式资源</param>
        /// <param name="imgUnenabled">失活态样式资源</param>
        public CharmButton(
            string text,
            Point point,
            ButtonType buttonType,
            Color color,
            ButtonState buttonState,
            Image imgNormal,
            Image imgHover,
            Image imgDown,
            Image imgUnenabled)
        {
            CheckTextLength(text);
            this._text = text;
            this._position = point;
            this.buttonType = buttonType;
            this.textColor = color;
            this.buttonState = buttonState;
            this._imgNormal = imgNormal;
            this._imgHover = imgHover;
            this._imgDown = imgDown;
            this._imgUnenabled = imgUnenabled;
            InitializeSetting();
        }

        /// <summary>
        /// 初始化设定方法
        /// </summary>
        private void InitializeSetting()
        {
            // * 按钮外观样式设置 *
            base.Location = _position;
            // 控件大小设置
            switch (buttonType)
            {
                case ButtonType.Size_06922:
                    base.Size = new Size(69, 22);
                    break;
                case ButtonType.Size_08223:
                    base.Size = new Size(82, 23);
                    break;
                case ButtonType.Size_12425:
                    base.Size = new Size(124, 25);
                    break;
                case ButtonType.Customize:
                    base.Size = _imgNormal.Size;
                    break;
            }
            base.BackColor = Color.Transparent;
            DrawImage();
            // * 按钮外观样式设置 *
        }

        /// <summary>
        /// 检查文本长度符合当前按钮样式
        /// </summary>
        /// <param name="text">要检查的文本字符串</param>
        private void CheckTextLength(string text)
        {
            using (Graphics g = Graphics.FromImage(new Bitmap(this.Width, this.Height)))
            {
                SizeF sf = g.MeasureString(text, new Font("微软雅黑", 9));
                _textLength = Convert.ToInt32(sf.Width);
            }
        }
        #endregion

        #region 重载事件
        /// <summary>
        /// 鼠标进入事件
        /// </summary>
        protected override void OnMouseEnter(EventArgs e)
        {
            if (enabled)
            {
                buttonState = ButtonState.Hover;
                DrawImage();
            }
        }

        /// <summary>
        /// 鼠标离开事件
        /// </summary>
        protected override void OnMouseLeave(EventArgs e)
        {
            if (enabled)
            {
                buttonState = ButtonState.Normal;
                DrawImage();
            }
        }

        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            // 只响应左键
            if (mevent.Button == MouseButtons.Left && enabled)
            {
                buttonState = ButtonState.Down;
                DrawImage();
            }
        }

        /// <summary>
        /// 鼠标弹起事件
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            // 只响应左键
            if (mevent.Button == MouseButtons.Left && enabled)
            {
                buttonState = ButtonState.Normal;
                DrawImage();
            }
        }

        // 画图方法
        private void DrawImage()
        {
            Color color = textColor; // 文本颜色
            int offsetX = 0;  // X坐标偏移
            int offsetY = 3;  // Y坐标偏移

            // 判断按钮激活状态
            if (!enabled)
                buttonState = ButtonState.Unenabled;

            // 判断按钮状态
            switch (buttonState)
            {
                case ButtonState.Normal:
                    base.Image = new Bitmap(_imgNormal);
                    break;
                case ButtonState.Hover:
                    base.Image = new Bitmap(_imgHover);
                    break;
                case ButtonState.Down:
                    base.Image = new Bitmap(_imgDown);
                    offsetX = 1;
                    offsetY = 4;
                    break;
                case ButtonState.Unenabled:
                    base.Image = new Bitmap(_imgUnenabled);
                    color = Color.DarkGray;
                    break;
            }
            using (Graphics g = Graphics.FromImage(base.Image))
            {
                g.DrawString(_text, new Font("微软雅黑", 9), new SolidBrush(color),
                    new Point((base.Width / 2) - (_textLength / 2) + offsetX - 1, offsetY));
            }
        }

        /// <summary>
        /// 用于设置新的按钮状态和文本颜色
        /// </summary>
        /// <param name="buttonState">按钮状态</param>
        /// <param name="color">文本颜色</param>
        public void SetState(ButtonState buttonState, Color color)
        {
            this.textColor = color;
            this.buttonState = buttonState;
            DrawImage();
        }
        #endregion
    }
}
