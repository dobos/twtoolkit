using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Elte.GeoVisualizer.Lib
{
    public class LineStyle
    {
        public enum DrawMethod
        {
            Empty,
            Random,
            Constant,
            Field
        }

        public DrawMethod Method { get; set; }
        public Pen Constant { get; set; }
        public Pen[] RandomSet { get; set; }
        public string Field { get; set; }

        public bool IsVisible
        {
            get { return Method != DrawMethod.Empty; }
        }

        public Pen GetPen(RenderingContext context)
        {
            switch (Method)
            {
                case DrawMethod.Constant:
                    return Constant;
                case DrawMethod.Random:
                    return RandomSet[context.Random.Next(RandomSet.Length)];
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
