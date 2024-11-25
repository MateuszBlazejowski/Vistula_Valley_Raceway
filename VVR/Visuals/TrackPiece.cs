using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVR.Visuals {
    internal class TrackPiece //one row piece of track 
    {
        public int leftBorder, rightBorder; // position of the trackborders, counting from the left 
        public char leftBorderSign, rightBorderSign; // sign representing left and right border 

        public TrackPiece(int leftBorder, int rightBorder, char leftBorderSign, char rightBorderSign)
        {
            this.leftBorder = leftBorder;
            this.rightBorder = rightBorder;
            this.leftBorderSign = leftBorderSign;
            this.rightBorderSign = rightBorderSign;
        }

        // example :
        // leftBorder = 0, rightBorder = 20 
        // leftBorderSign = |, rightBorderSign = \  
        // 
        // piece of track look: 
        // 
        //|                   \
    }
}
