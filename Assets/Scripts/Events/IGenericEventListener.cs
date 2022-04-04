namespace LudumDare50 {
    public interface IGenericEventListener<T> {
        void OnEventRaised(T value);
    }
}
