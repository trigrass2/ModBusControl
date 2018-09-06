﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ECInspect
{
    [ToolboxItem(false)]
    public partial class AxisDot : AxisInterface
    {
        public AxisDot()
        {
            InitializeComponent();
            WindowRefresh.Tick += new EventHandler(WindowRefresh_Tick);
        }

        private void AxisDot_Load(object sender, EventArgs e)
        {
            base.axisTrackBar.AxisMaxValue = INIFileValue.DotAxisRange.MAX * GlobalVar.ConverRate;
            base.axisTrackBar.AxisMinValue = INIFileValue.DotAxisRange.MIN * GlobalVar.ConverRate;
            this.Ref_X.Text = ((double)GlobalVar.c_Modbus.HoldingRegisters.AxisDot_MarkPoint.Value / GlobalVar.ConverRate).ToString("#0.00");
            this.Ref_Y.Text = ((double)GlobalVar.c_Modbus.HoldingRegisters.AxisY_MarkPoint.Value / GlobalVar.ConverRate).ToString("#0.00");
            
        }

        private void WindowRefresh_Tick(object sender, EventArgs e)
        {
            Console.WriteLine(this.m_AxisName.ToString() + "子窗体刷新");
            UpdateValue();
        }

        private void UpdateValue()
        {
            this.either_Mark2.LeftPress = !GlobalVar.c_Modbus.Coils.Cylinder_Dot.Value;          //打标器 上升

            double DotAxisWaitMarkPoint = GlobalVar.c_Modbus.HoldingRegisters.AxisDot_WaitMarkPoint.Value;
            DotAxisWaitMarkPoint /= GlobalVar.ConverRate;

            this.textBox_DotAxisWaitMarkPoint.Text = DotAxisWaitMarkPoint.ToString("#0.00");

            double AxisDot_ScanPoint = GlobalVar.c_Modbus.HoldingRegisters.AxisDot_ScanPoint.Value;
            AxisDot_ScanPoint /= GlobalVar.ConverRate;

            this.textBox_DotAxisWaitScanPoint.Text = AxisDot_ScanPoint.ToString("#0.00");

        }

        private void textBox_DotAxisWaitMarkPoint_Click(object sender, EventArgs e)
        {
            ChangePLCValue(sender, GlobalVar.c_Modbus.HoldingRegisters.AxisDot_WaitMarkPoint, 2, this.axisTrackBar.AxisMaxValue / GlobalVar.ConverRate, this.axisTrackBar.AxisMinValue / GlobalVar.ConverRate);
        }
        private void textBox_DotAxisWaitScanPoint_Click(object sender, EventArgs e)
        {
            //修改扫描轴位置--[2018.3.28 lqz]
            ChangePLCValue(sender, GlobalVar.c_Modbus.HoldingRegisters.AxisDot_ScanPoint, 2, this.axisTrackBar.AxisMaxValue / GlobalVar.ConverRate, this.axisTrackBar.AxisMinValue / GlobalVar.ConverRate);

        }
        private void either_Mark2_Event_BtnClick(object sender, LeftRightSide lr)
        {
            switch (lr)
            {
                case LeftRightSide.Left:
                    GlobalVar.c_Modbus.AddMsgList(GlobalVar.c_Modbus.Coils.Cylinder_Dot, false);
                    break;
                case LeftRightSide.Right:
                    GlobalVar.c_Modbus.AddMsgList(GlobalVar.c_Modbus.Coils.Cylinder_Dot, true);
                    break;
            }
        }

        private void btn_StartCali_Click(object sender, EventArgs e)
        {
            try
            {
                double x = double.Parse(Ref_X.Text);
                double y = double.Parse(Ref_Y.Text);
                double a = double.Parse(Mark_X.Text);
                double b = double.Parse(Mark_Y.Text);
                double c = double.Parse(Act_X.Text);
                double d = double.Parse(Act_Y.Text);
                double k1, k2, tanA=0;
                if (a == 0 &&c == 0)
                {
                    MsgBox("请勿使用原点校正", Color.LightYellow, MessageBoxButtons.OK);
                    return;
                }
                k1 = (b - y) / (a - x);
                k2 = (d - y) / (c - x);
                tanA = (k2 - k1) / (1 + k1 * k2);
                GlobalVar.Dot_Coordinate_Radian = Math.Atan(tanA);
                myFunction.WriteIniString(INIFileValue.gl_inisection_Tan, INIFileValue.gl_iniKey_Dot_Coordinate_Angle, GlobalVar.Dot_Coordinate_Radian.ToString());

                MsgBox("打标校准完成,补正弧度:"+Math.Atan(tanA).ToString("0.00000"), Color.LimeGreen, MessageBoxButtons.OK);
            }
            catch (System.Exception ex)
            {
                MsgBox(ex.Message, Color.Red, MessageBoxButtons.OK);
            }
        }

        private void Mark_X_Click(object sender, EventArgs e)
        {
            double d = 0d;
            if (TextClick(sender, ref d, 2, this.axisTrackBar.AxisMaxValue / GlobalVar.ConverRate, this.axisTrackBar.AxisMinValue / GlobalVar.ConverRate))
            {
                Mark_X.Text = d.ToString("0.00");
            }
        }

        private void Mark_Y_Click(object sender, EventArgs e)
        {
            double d = 0d;
            if (TextClick(sender, ref d, 2, this.axisTrackBar.AxisMaxValue / GlobalVar.ConverRate, this.axisTrackBar.AxisMinValue / GlobalVar.ConverRate))
            {
                Mark_Y.Text = d.ToString("0.00");
            }
        }

        private void Act_X_Click(object sender, EventArgs e)
        {
            double d = 0d;
            if (TextClick(sender, ref d, 2, this.axisTrackBar.AxisMaxValue / GlobalVar.ConverRate, this.axisTrackBar.AxisMinValue / GlobalVar.ConverRate))
            {
                Act_X.Text = d.ToString("0.00");
            }
        }

        private void Act_Y_Click(object sender, EventArgs e)
        {
            double d = 0d;
            if (TextClick(sender, ref d, 2, this.axisTrackBar.AxisMaxValue / GlobalVar.ConverRate, this.axisTrackBar.AxisMinValue / GlobalVar.ConverRate))
            {
                Act_Y.Text = d.ToString("0.00");
            }
        }

        private void textBox_DotAxisWaitMarkPoint_TextChanged(object sender, EventArgs e)
        {

        }

        private void ref_Y_Click(object sender, EventArgs e)
        {
            double d = 0d;
            if (TextClick(sender, ref d, 2, this.axisTrackBar.AxisMaxValue / GlobalVar.ConverRate, this.axisTrackBar.AxisMinValue / GlobalVar.ConverRate))
            {
                Ref_Y.Text = d.ToString("0.00");
            }
        }

        private void Ref_X_Click(object sender, EventArgs e)
        {
            double d = 0d;
            if (TextClick(sender, ref d, 2, this.axisTrackBar.AxisMaxValue / GlobalVar.ConverRate, this.axisTrackBar.AxisMinValue / GlobalVar.ConverRate))
            {
                Ref_X.Text = d.ToString("0.00");
            }
        }
    }
}