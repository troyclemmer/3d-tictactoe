using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeniorProject
{
    class Coordinate
    {
        int[] value;

        public Coordinate(int x, int y)
        {
            value = new int[2] { x, y };
        }

        public int[] getValue()
        {
            return value;
        }

        public int getX()
        {
            return value[0];
        }

        public int getY()
        {
            return value[1];
        }
    }
}
