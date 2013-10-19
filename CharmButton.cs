#region 文档说明
/* ****************************************************************************************************
 * 文档作者：无闻
 * 创建日期：2013 年 2 月 13 日
 * 文档用途：CharmButton - 按钮控件
 * -----------------------------------------------------------------------------------------------------
 * 修改记录：
 * 2013-02-20：针对 CSBox 界面标准 2.0 进行升级改造
 * -----------------------------------------------------------------------------------------------------
 * 参考文献：
 * 
 * *****************************************************************************************************/
#endregion

#region 命名空间引用
using System.Drawing;

using CharmControlLibrary.Properties;
#endregion

namespace CharmControlLibrary
{
    #region 枚举类型
    /// <summary>
    /// 按钮类型（用于指定按钮大小和按钮样式）
    /// </summary>
    public enum ButtonType
    {
        /// <summary>
        /// 未指定按钮类型
        /// </summary>
        Undefined,
        /// <summary>
        /// 按钮类型：经典样式，大小为 69*22
        /// </summary>
        Classic_Size_06922,
        /// <summary>
        /// 按钮类型：经典样式，大小为 88*23
        /// </summary>
        Classic_Size_08223,
        /// <summary>
        /// 按钮类型：经典样式，大小为 124*25
        /// </summary>
        Classic_Size_12425,
        /// <summary>
        /// 按钮类型：绿色样式，大小为 102*36
        /// </summary>
        Green_Size_10236,
        /// <summary>
        /// 自定义按钮四态图片，需要手动设置控件大小
        /// </summary>
        Customize
    }
    #endregion

    /// <summary>
    /// 表示 CharmControlLibrary.CharmButton 按钮控件
    /// </summary>
    public class CharmButton : CharmControl
    {
        #region 字段
        // 控件的状态图片
        private Image[] mStatusImages;
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置 CharmControlLibrary.CharmButton 的按钮类型（自定义类型按钮大小需用户自行设定）
        /// </summary>
        public override ButtonType ButtonType
        {
            get { return base.ButtonType; }
            set
            {
                // 获取用户设置的按钮类型
                base.ButtonType = value;
                // 根据按钮类型决定四态图片及控件大小
                switch (ButtonType)
                {
                    case ButtonType.Classic_Size_06922:
                        Size = new Size(69, 22);
                        mStatusImages[0] = Resources.btn_06922_normal;
                        mStatusImages[1] = Resources.btn_06922_hover;
                        mStatusImages[2] = Resources.btn_06922_down;
                        mStatusImages[3] = Resources.btn_06922_unenabled;
                        break;
                    case ButtonType.Classic_Size_08223:
                        Size = new Size(82, 23);
                        mStatusImages[0] = Resources.btn_08223_normal;
                        mStatusImages[1] = Resources.btn_08223_hover;
                        mStatusImages[2] = Resources.btn_08223_down;
                        mStatusImages[3] = Resources.btn_08223_unenabled;
                        break;
                    case ButtonType.Classic_Size_12425:
                        Size = new Size(124, 25);
                        mStatusImages[0] = Resources.btn_12425_normal;
                        mStatusImages[1] = Resources.btn_12425_hover;
                        mStatusImages[2] = Resources.btn_12425_down;
                        mStatusImages[3] = Resources.btn_12425_Unenabled;
                        break;
                    case ButtonType.Green_Size_10236:
                        Size = new Size(102, 36);
                        mStatusImages[0] = Resources.green_10236_normal;
                        mStatusImages[1] = Resources.green_10236_hover;
                        mStatusImages[2] = Resources.green_10236_down;
                        mStatusImages[3] = Resources.green_10236_unenabled;
                        break;
                    case ButtonType.Customize:

                        break;
                }
                // 重新设置控件状态以便绘制背景图像
                ControlStatus = ControlStatus.Normal;
            }
        }

        /// <summary>
        /// 获取或设置控件的状态
        /// </summary>
        public override ControlStatus ControlStatus
        {
            get { return base.ControlStatus; }
            set
            {
                base.ControlStatus = value;
                // 根据控件状态设定背景图像
                BackgroundImage = mStatusImages[(int)ControlStatus];
            }
        }

        /// <summary>
        /// 获取或设置控件的状态图片
        /// </summary>
        public Image[] StatusImages
        {
            get { return mStatusImages; }
            set { mStatusImages = value; }
        }

        /// <summary>
        /// 获取或设置与此控件关联的文本
        /// </summary>
        public override string Text
        {
            get { return base.Text; }
            set
            {
                // 相同文本不再二次复制防止文本位置被复位
                if (string.Equals(base.Text, value))
                    return;
                base.Text = value;

                // 判断是否为自定义类型
                if (base.ButtonType == ButtonType.Customize)
                    return;

                // 根据按钮类型设置并计算文本偏移位置
                int offsetX = 0;  // X坐标偏移
                int offsetY = 0;  // Y坐标偏移
                switch (ButtonType)
                {
                    case ButtonType.Classic_Size_06922:
                        offsetX = -1;
                        offsetY = 3;
                        break;
                    case ButtonType.Classic_Size_08223:
                        offsetX = -6;
                        offsetY = 3;
                        break;
                    case ButtonType.Classic_Size_12425:
                        break;
                }
                using (Graphics g = Graphics.FromImage(mStatusImages[0]))
                {
                    SizeF sf = g.MeasureString(base.Text, base.Font);
                    TextPosition = new Point(
                        (ClientRectangle.Width / 2) - ((int)sf.Width / 2) + offsetX, offsetY);
                }
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示控件是否可以对用户交互作出响应
        /// </summary>
        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                // 根据控件激活性设置控件状态
                if (value)
                    ControlStatus = ControlStatus.Normal;
                else
                {
                    // 如果控件在按下的处理事件中被禁止激活，则需要将文本坐标归位
                    if (ControlStatus == ControlStatus.Down)
                        TextPosition = new Point(TextPosition.X - 1, TextPosition.Y - 1);
                    ControlStatus = ControlStatus.Unenabled;
                }
            }
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 初始化 CharmButton 类的新实例
        /// </summary>
        public CharmButton()
        {
            // * 初始化属性 *
            ControlType = ControlType.CharmButton;
            mStatusImages = new Image[4];
        }
        #endregion

        #region 方法
        /// <summary>
        /// 释放由 Component 使用的所有资源
        /// </summary>
        public new void Dispose()
        {
            // * 释放系统资源 *
            mStatusImages = null;

            // 调用基类方法
            base.Dispose();
        }
        #endregion
    }
}
