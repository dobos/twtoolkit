using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Elte.GeoVisualizer.Lib
{
    public class FillStyle
    {
        public enum FillMethod
        {
            Empty,
            Random,
            Constant,
            Field
        }

        public FillMethod Method { get; set; }
        public Brush Constant { get; set; }
        public Brush[] RandomSet { get; set; }
        public string Field { get; set; }

        public bool IsVisible
        {
            get { return Method != Lib.FillStyle.FillMethod.Empty; }
        }

        public Brush GetBrush(RenderingContext context)
        {
            switch (Method)
            {
                case FillMethod.Constant:
                    return Constant;
                case FillMethod.Random:
                    return RandomSet[context.Random.Next(RandomSet.Length)];
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
