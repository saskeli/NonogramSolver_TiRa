namespace GameLib
{
    internal class Number
    {
        private readonly int _value;
        private bool _done = false;

        public Number(int value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}