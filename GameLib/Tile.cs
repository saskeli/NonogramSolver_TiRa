namespace GameLib
{
    internal class Tile
    {
        private int _priority;
        private bool? _state = null;

        public Tile(int prio)
        {
            _priority = prio;
        }

        public char ToChar()
        {
            if (_state.HasValue)
            {
                return _state.Value ? ' ' : '█';
            }
            return '?';
        }
    }
}