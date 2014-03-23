
using System.Drawing;

namespace AirportAssignment
{
    public class Plane
    {
        private Point pos;
        private Color colour;
        private int destination;

        public Plane(Point position, Color colour, int destination)
        {
            pos = position;
            this.colour = colour;
            this.destination = destination;
        }

        public void Move(int xDelta, int yDelta)
        {
            pos.X += xDelta;
            pos.Y += yDelta;
        }

        public void setPos(int x, int y)
        {
            pos.X = x;
            pos.Y = y;
        }

        public int getPosX()
        {
            return pos.X;
        }

        public int getPosY()
        {
            return pos.Y;
        }

        public Color getColour()
        {
            return colour;
        }

        public int getDestination()
        {
            return destination;
        }

        public void setDestination(int destination)
        {
            this.destination = destination;
        }

        public string getStringDestination()
        {
            int intTemp = this.destination;
            return intTemp.ToString();
        }
    }
}
