public partial class AppMain
{
    public class Reference<T>
    {
        private T target_;

        public Reference(T target)
        {
            this.target_ = target;
        }

        public T Target
        {
            get => this.target_;
            set => this.target_ = value;
        }
    }
}
