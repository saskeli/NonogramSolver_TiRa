namespace GameLib
{
    internal class Tile
    {
        private int _priority;
        public bool? State = null;

        public Tile(int prio)
        {
            _priority = prio;
        }

        public char ToChar()
        {
            if (State.HasValue)
            {
                return State.Value ? '█' : ' ';
            }
            return '?';
        }
    }
}