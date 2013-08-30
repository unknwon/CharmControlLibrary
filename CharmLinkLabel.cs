#region 文档说明
/* ****************************************************************************************************
 * 文档作者：无闻
 * 创建日期：2013年2月8日
 * 文档用途：CharmLinkLabel - 链接标签控件
 * -----------------------------------------------------------------------------------------------------
 * 修改记录：
 * 2013-03-03：针对CSBox界面标准2.0进行升级改造
 * -----------------------------------------------------------------------------------------------------
 * 参考文献：
 * 
 * *****************************************************************************************************/
#endregion

#region 命名空间引用
using System;
using System.Drawing;
#endregion

namespace CharmControlLibrary
{
    /// <summary>
    /// 表示 CharmControlLibrary.CharmLinkLabel 链接标签控件
    /// </summary>
    public class CharmLinkLabel : CharmControl
    {
        #region 属性
        /// <summary>
        /// 获取或设置控件的状态
        /// </summary>
        public override ControlStatus ControlStatus
        {
            get { return base.ControlStatus; }
            set
            {
                // 获取控件状态
                base.ControlStatus = value;
                // 判断字体是否加粗
                bool isBold = this.Font.Bold;
                // 根据控件状态判断是否需要设置下划线
                if (base.ControlStatus == ControlStatus.Normal)
                {
                    // 常态，不需要设置
                    if (isBold)  // 判断字体是否加粗
                        this.Font = new Font(this.Font.Name, this.Font.Size, FontStyle.Bold);
                    else
                        this.Font = new Font(this.Font.Name, this.Font.Size);
                }
                else
                {
                    // 非常态，需要设置
                    if (isBold)  // 判断字体是否加粗
                        this.Font = new Font(this.Font.Name, this.Font.Size, FontStyle.Underline | FontStyle.Bold);
                    else
                        this.Font = new Font(this.Font.Name, this.Font.Size, FontStyle.Underline);
                }
            }
        }

        /// <summary>
        /// 获取或设置与此控件关联的文本
        /// </summary>
        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                // 重新计算工作区矩形
                SizeF sf;
                using (Graphics g = Graphics.FromImage(new Bitmap(100, 100)))
                {
                    sf = g.MeasureString(base.Text, this.Font);
                }
                // 设置新的工作区矩形参数
                base.Width = (int)sf.Width;
                base.Height = (int)sf.Height;
            }
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 初始化 CharmLinkLabel 类的新实例
        /// </summary>
        public CharmLinkLabel()
            : base()
        {
            // * 初始化属性 *
            this.ControlType = ControlType.CharmLinkLabel;
        }
        #endregion
    }
}
