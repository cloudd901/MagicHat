using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace RandomTool
{
    public partial class Hat
    {
        public void Start(int animDirection = (int)Direction.Clockwise, int randPowerType = (int)PowerType.Random, int randStrength = 5)
        {
            if (IsBusy) { if (AllowExceptions) { throw new InvalidOperationException("Hat is currently busy."); } else { return; } }
            if (ToolProperties.objectImage == null) { if (AllowExceptions) { throw new InvalidOperationException("Please initialize hat using Draw()."); } else { return; } }
            if (EntryList.Count <= 0) { if (AllowExceptions) { throw new IndexOutOfRangeException("Must have more than zero Entries to Animate."); } else { return; } }

            int manualStrength = 0;
            if (randPowerType == (int)PowerType.Infinite) { randStrength = -1; }
            else if (randPowerType != (int)PowerType.Manual) { randStrength = (int)UpdateRandStrength(randPowerType); }
            else { manualStrength = randStrength; }// randStrength = randStrength > 11 ? 11 : randStrength < 1 ? randStrength == -1 ? -1 : 1 : randStrength; }
            
            if (ToolProperties.currentArrowDirection != ToolProperties.ArrowPosition)
            {
                if (_ControlHat.InvokeRequired)
                {
                    _ControlHat.Invoke((MethodInvoker)delegate
                    {
                        Refresh();
                    });
                }
                else
                {
                    Refresh();
                }
            }

            Random randomPick = new Random();
            Entry currentEntry = EntryList[randomPick.Next(0, EntryList.Count)];
            //Color c = Color.White;
            IsBusy = true;

            Bitmap newImage = new Bitmap((int)(ToolSize.Diameter + ToolSize.Left + 2), (int)(ToolSize.Diameter + ToolSize.Top + 2));
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                Matrix animationMatrix = new Matrix();
                float animationAngle = 0;
                Random r = new Random();

                float randomCount = 360;

                if (manualStrength > 0)
                {
                    randomCount = manualStrength;
                }
                else
                {
                    if (randStrength == -1)
                    { randomCount = -1; }
                    else if (randStrength < 5)
                    { randomCount = r.Next(360, 360 * randStrength); }
                    else if (randStrength < 7)
                    { randomCount = r.Next(720, 360 * randStrength); }
                    else if (randStrength < 10)
                    { randomCount = r.Next(1080, 360 * (randStrength * 2)); }
                    else if (randStrength == 10)
                    { randomCount = r.Next(1440, 360 * (randStrength * 3)); }
                    else if (randStrength > 10)
                    { randomCount = r.Next(1800, 360 * (randStrength * 5)); }
                
                    if (randomCount != -1) { randomCount = randomCount / 360; }
                }
                // Keep same premise as using the Wheel.
                // But use one full rotation as 4 shakes of the hat.
                float originalRandomCount = randomCount;

                while ((randomCount <= -1 || randomCount > 0))
                {
                    if (randomCount > -1 && !IsBusy)
                    {
                        break;
                    }
                    else if (randomCount <= -1 && !IsBusy)
                    {
                        IsBusy = true;
                        randomCount = r.Next(2, 5);
                    }

                    randomCount--;
                    animationAngle = 0;
                    float scaling = 1;
                    float translatingX = 0;
                    float translatingY = 0;

                    int toggle = 0;
                    while (animationAngle <= 360)
                    {
                        bool breakfree = false;
                        animationMatrix = new Matrix();
                        float originalAnimationAngle = animationAngle;
                        float newAnimationAngle = animationAngle;

                        //Slow down the animation without using Sleep.
                        //Causes image to be redrawn in same position.
                        if (toggle >= 1) { if (toggle == 2) { toggle = 0; } else { toggle++; } }
                        else
                        {
                            toggle++;
                            animationAngle++;
                            newAnimationAngle = animationAngle;

                            if (newAnimationAngle <= 45) { scaling -= 0.01f; translatingY += 2f; }
                            else if (newAnimationAngle > 45 && newAnimationAngle <= 90) { newAnimationAngle = 90 - newAnimationAngle; scaling += 0.01f; translatingY -= 2f; }
                            else if (newAnimationAngle > 90 && newAnimationAngle <= 135) { newAnimationAngle = 90 - newAnimationAngle; scaling -= 0.01f; translatingX += 4f; translatingY += 2f; }
                            else if (newAnimationAngle > 135 && newAnimationAngle <= 180) { newAnimationAngle = newAnimationAngle - 180; scaling += 0.01f; translatingX -= 4f; translatingY -= 2f; }
                            else if (newAnimationAngle > 180 && newAnimationAngle <= 225) { newAnimationAngle = newAnimationAngle - 180; scaling -= 0.01f; translatingY += 2f; }
                            else if (newAnimationAngle > 225 && newAnimationAngle <= 270) { newAnimationAngle = 270 - newAnimationAngle; scaling += 0.01f; translatingY -= 2f; }
                            else if (newAnimationAngle > 270 && newAnimationAngle <= 315) { newAnimationAngle = 270 - newAnimationAngle; scaling -= 0.01f; translatingX += 4f; translatingY += 2f; }
                            else if (newAnimationAngle > 315 && newAnimationAngle <= 360) { newAnimationAngle = newAnimationAngle - 360; scaling += 0.01f; translatingX -= 4f; translatingY -= 2f; }

                            if (newAnimationAngle < 2.5f && newAnimationAngle > -2.5f)
                            {
                                scaling = 1f; translatingX = 0f; translatingY = 0f;
                                ShuffleEntries();

                                currentEntry = EntryList[randomPick.Next(0, EntryList.Count)];

                                DrawHat(currentEntry);
                            }
                            animationMatrix.Scale(scaling, scaling);
                            animationMatrix.Translate(translatingX, translatingY);
                            animationMatrix.RotateAt(newAnimationAngle, ToolSize.Center);
                            if (animDirection == (int)Direction.CounterClockwise) { newAnimationAngle *= -1; }
                            graphics.Transform = animationMatrix;

                            if (randomCount > -1 && !IsBusy)
                            {
                                if (newAnimationAngle < 2.5f && newAnimationAngle > -2.5f) { breakfree = true; }
                            }
                        }

                        graphics.Clear(_ControlHat.BackColor);
                        graphics.DrawImage(ToolProperties.objectImage, 0, 0);
                        try { _ControlHat.Image = newImage; }
                        catch (InvalidOperationException)
                        {
                            try { _ControlHat.Image = newImage; }
                            catch (InvalidOperationException) { }
                        }

                        ToolActionCall?.Invoke(currentEntry, new string[] { randPowerType.ToString() + "|" + randStrength.ToString(), (randomCount+0.4f).ToString("0"), (animationAngle - originalAnimationAngle).ToString(), originalRandomCount.ToString("0") });
                        try
                        {
                            if (_ControlHat3D.Parent.InvokeRequired)
                            {
                                _ControlHat3D.Parent.Invoke((MethodInvoker)delegate
                                {
                                    _ControlHat.Update();
                            });
                            }
                            else
                            {
                                _ControlHat.Update();
                            }
                        }
                        catch { break; }
                        if (breakfree) { break; }
                        animationAngle += 1;
                    }
                }
            }
            IsBusy = false;
            ToolStopCall?.Invoke(currentEntry);
        }
        public void Stop()
        {
            IsBusy = false;
        }

        private int UpdateRandStrength(int randPowerType)
        {
            int randStrength = 5;
            Random r = new Random();
            if (randPowerType == (int)PowerType.Weak) { randStrength = r.Next(1, 3 + 1); }
            else if (randPowerType == (int)PowerType.Average) { randStrength = r.Next(4, 8 + 1); }
            else if (randPowerType == (int)PowerType.Strong) { randStrength = r.Next(9, 10 + 1); }
            else if (randPowerType == (int)PowerType.Super) { randStrength = 11; }
            else if (randPowerType == (int)PowerType.Random) { randStrength = RandomRandStrength(); }
            return randStrength;
        }
        private int RandomRandStrength()
        {
            Random r = new Random();
            int ss = r.Next(1, 10 + 1);
            if (ss < 4) { ss = r.Next(1, 10 + 1); }
            if (ss == 1) { ss = r.Next(1, 10 + 1); }
            else if (ss == 10) { ss = r.Next(1, 10 + 1); }
            return ss;
        }
    }
}
