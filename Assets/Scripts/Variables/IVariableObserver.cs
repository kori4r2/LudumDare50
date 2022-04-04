namespace LudumDare50 {
    public interface IVariableObserver<T> {
        void OnValueChanged(T newValue);
    }
}
