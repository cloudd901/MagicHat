using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace RandomTool
{
    public partial class Hat
    {
        public void Draw(PointF center, float radius)
        {
            if (radius < 1) { if (AllowExceptions) { throw new ArgumentOutOfRangeException ("Radius must be greater than 1"); } else { return; } }
            ToolSize.Left = center.X - radius;
            ToolSize.Top = center.Y - radius;
            ToolSize.Radius = radius;
            ToolSize.Diameter = (int)(radius * 2f);
            ToolSize.Center = center;
            ToolSize.initialized = true;
            Refresh();
        }
        public void Draw(int left, int top, float radius)
        {
            if (radius < 1) { if (AllowExceptions) { throw new ArgumentOutOfRangeException("Radius must be greater than 1"); } else { return; } }
            ToolSize.Left = left;
            ToolSize.Top = top;
            ToolSize.Radius = radius;
            ToolSize.Diameter = (int)(radius * 2f);
            ToolSize.Center = new PointF(radius, radius);
            ToolSize.initialized = true;
            Refresh();
        }
        private void DrawHat(Entry e = null)
        {
            if (!ToolSize.initialized) { if (AllowExceptions) { throw new InvalidOperationException("Please initialize hat using Draw()."); } else { return; } }
            if (e == null) { SetPictureBoxes(); }

            int shadowLength = ToolProperties.ShadowVisible ? ToolProperties.ShadowLength : 0;
            ToolProperties.objectImage = new Bitmap(ToolSize.Diameter + shadowLength, ToolSize.Diameter + shadowLength);

            using (Graphics graphics = Graphics.FromImage(ToolProperties.objectImage))
            {
                Pen pen = new Pen(ToolProperties.LineColor, ToolProperties.LineWidth);

                Image namesImage = new Bitmap(ToolSize.Diameter, ToolSize.Diameter);
                Random r = new Random();
                Brush brush1;
                Brush brush2;
                Brush brush3;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                float imgSize = ToolSize.Radius * 2;
                float imgHeight = imgSize * 0.81f;
                float imgHeightAdj = (imgSize - imgHeight) / 2f;
                //Radius = 200
                SizeF sizeBase = new SizeF(imgSize, imgSize * 0.35f);
                PointF pointBase = new PointF(0, imgHeightAdj);

                SizeF sizeInner = new SizeF(imgSize / 2f, imgSize * .10f);
                PointF pointInner = new PointF(imgSize * 0.25f, (imgSize * 0.125f) + imgHeightAdj);//25

                SizeF sizeTop = new SizeF(imgSize * 0.62f, imgSize * 0.25f);
                PointF pointTop = new PointF(imgSize * 0.19f, (imgSize * 0.63f) + imgHeightAdj);


                PointF[] pa1 = new PointF[] { new PointF(imgSize * 0.25f, imgSize * 0.175f), new PointF(imgSize * 0.235f, imgSize * 0.375f), new PointF(imgSize * 0.22f, imgSize * 0.5f), new PointF(imgSize * 0.21f, imgSize * 0.6f), new PointF(imgSize * 0.19f, imgSize * 0.75f) };
                PointF[] pa2 = new PointF[] { new PointF(imgSize * 0.75f, imgSize * 0.175f), new PointF(imgSize * 0.765f, imgSize * 0.375f), new PointF(imgSize * 0.78f, imgSize * 0.5f), new PointF(imgSize * 0.79f, imgSize * 0.6f), new PointF(imgSize * 0.81f, imgSize * 0.75f) };
                PointF[] pa3 = new PointF[10]; pa1.CopyTo(pa3, 0);
                int i = 9;
                foreach (PointF p in pa2)
                {
                    pa3[i] = p;
                    i--;
                }
                i = 0;
                foreach (PointF p in pa3)
                {
                    pa3[i].Y += imgHeightAdj;
                    i++;
                }

                PointF[] pa4 = new PointF[] { pa1[1], new PointF(imgSize * 0.275f, imgSize * 0.385f), new PointF(imgSize * 0.375f, imgSize * 0.395f), new PointF(imgSize * 0.5f, imgSize * 0.4f), new PointF(imgSize * 0.625f, imgSize * 0.395f), new PointF(imgSize * 0.725f, imgSize * 0.385f), pa2[1] };
                PointF[] pa5 = new PointF[] { pa1[2], new PointF(imgSize * 0.255f, imgSize * 0.508f), new PointF(imgSize * 0.36375f, imgSize * 0.52f), new PointF(imgSize * 0.4925f, imgSize * 0.525f), new PointF(imgSize * 0.62125f, imgSize * 0.52f), new PointF(imgSize * 0.745f, imgSize * 0.508f), pa2[2] };
                PointF[] pa6 = new PointF[14]; pa4.CopyTo(pa6, 0);
                i = 13;
                foreach (PointF p in pa5)
                {
                    pa6[i] = p;
                    i--;
                }
                i = 0;
                foreach (PointF p in pa6)
                {
                    pa6[i].Y += imgHeightAdj;
                    i++;
                }

                brush1 = new SolidBrush(Color.Black);
                brush2 = new SolidBrush(Color.FromArgb(55, 55, 55));
                brush3 = new SolidBrush(ToolProperties.CenterColor);
                pen.Width++;
                graphics.DrawPolygon(pen, pa3);
                graphics.DrawEllipse(pen, pointTop.X, pointTop.Y, sizeTop.Width, sizeTop.Height);
                graphics.FillPolygon(brush2, pa3);
                if (ToolProperties.CenterVisible) { graphics.FillPolygon(brush3, pa6); }
                graphics.FillEllipse(brush2, pointTop.X, pointTop.Y, sizeTop.Width, sizeTop.Height);
                pen.Width--;
                graphics.FillEllipse(brush2, pointBase.X, pointBase.Y, sizeBase.Width, sizeBase.Height);
                graphics.DrawEllipse(pen, pointBase.X, pointBase.Y, sizeBase.Width, sizeBase.Height);
                graphics.FillEllipse(brush1, pointInner.X, pointInner.Y, sizeInner.Width, sizeInner.Height);
                graphics.DrawEllipse(pen, pointInner.X, pointInner.Y, sizeInner.Width, sizeInner.Height);

                if (ToolProperties.CenterVisible && e != null)
                {
                    if (ToolProperties.TextToShow != TextType.None)
                    {
                        using (Graphics graphicsNames = Graphics.FromImage(ToolProperties.objectImage))
                        {
                            graphicsNames.SmoothingMode = SmoothingMode.AntiAlias;
                            graphicsNames.CompositingQuality = CompositingQuality.HighQuality;
                            graphicsNames.InterpolationMode = InterpolationMode.HighQualityBicubic;

                            brush3 = new SolidBrush(e.Aura);
                            if (ToolProperties.TextColorAuto)
                            {
                                if (!IsReadable(e.Aura, Color.Black)) { brush1 = Brushes.White; }
                                else { brush1 = Brushes.Black; }
                            }
                            else
                            {
                                brush1 = new SolidBrush(ToolProperties.TextColor);
                            }
                            graphics.FillPolygon(brush3, pa6);

                            Font drawFont = new Font(ToolProperties.TextFontFamily, GetCorrectSize(e.Name), ToolProperties.TextFontStyle);
                            string drawText = "";
                            if (ToolProperties.TextToShow == TextType.NameAndID) { drawText = $"{e.Name}_{e.UniqueID}"; }
                            else if (ToolProperties.TextToShow == TextType.Name) { drawText = $"{e.Name}"; }
                            else if (ToolProperties.TextToShow == TextType.ID) { drawText = $"{e.UniqueID}"; }
                            StringFormat format = new StringFormat();
                            format.LineAlignment = StringAlignment.Center;
                            format.Alignment = StringAlignment.Center;
                            graphicsNames.DrawString(drawText, drawFont, brush1, pa6[10].X, (pa6[10].Y + pa6[5].Y) / 2f, format);

                            graphicsNames.DrawImage(namesImage, new Point(0, 0));
                        }
                    }
                }
                graphics.DrawImage(ToolProperties.objectImage, new Point(0, 0));
                _ControlHat.Image = ToolProperties.objectImage;

                try
                {
                    if (_ControlHat.Parent.InvokeRequired)
                    {
                        _ControlHat.Parent.Invoke((MethodInvoker)delegate
                        {
                            _ControlHat.Refresh();
                        });
                    }
                    else
                    {
                        _ControlHat.Refresh();
                    }
                }
                catch
                { }
            }
        }

        public void Refresh()
        {
            if (IsBusy) { if (AllowExceptions) { throw new InvalidOperationException("Hat is currently busy."); } else { return; } }
            DrawHat();
        }

        private PointF CalculateMidPoint(PointF p1, PointF p2)
        {
            p1.X -= ToolSize.Left; p2.X += ToolSize.Left;
            return new PointF((float)((p1.X + p2.X) / 2f), (float)((p1.Y + p2.Y) / 2f));
        }

        private int GetCorrectSize(string word)
        {
            int size = 13;
            if (ToolSize.Radius <= 70)
            { size = 9; }

            else if(ToolSize.Radius < 150 && word.Length <= 5)
            { size = 11; }
            else if (ToolSize.Radius < 150 && word.Length > 5 && word.Length <= 15)
            { size = 10; }
            else if (ToolSize.Radius < 150 && word.Length >= 15)
            { size = 9; }

            else if (ToolSize.Radius >= 150 && ToolSize.Radius < 250 && word.Length <= 5)
            { size = 14; }
            else if (ToolSize.Radius >= 150 && ToolSize.Radius < 250 && word.Length > 5 && word.Length <= 15)
            { size = 13; }
            else if (ToolSize.Radius >= 150 && ToolSize.Radius < 250 && word.Length > 15)
            { size = 12; }

            else if (ToolSize.Radius >= 250 && ToolSize.Radius < 350 && word.Length <= 5)
            { size = 20; }
            else if (ToolSize.Radius >= 250 && ToolSize.Radius < 350 && word.Length > 5 && word.Length <= 15)
            { size = 17; }
            else if (ToolSize.Radius >= 250 && ToolSize.Radius < 350 && word.Length > 15)
            { size = 15; }

            else if (ToolSize.Radius >= 350 && ToolSize.Radius < 500 && word.Length <= 5)
            { size = 20; }
            else if (ToolSize.Radius >= 350 && ToolSize.Radius < 500 && word.Length > 5 && word.Length <= 15)
            { size = 19; }
            else if (ToolSize.Radius >= 350 && ToolSize.Radius < 500 && word.Length > 15)
            { size = 18; }

            else if (ToolSize.Radius >= 500)
            { size = 20; }
            return size;
        }
        public bool IsReadable(Color color1, Color color2)
        {
            return Math.Abs(color1.GetBrightness() - color2.GetBrightness()) >= 0.5f;
        }
    }
}
