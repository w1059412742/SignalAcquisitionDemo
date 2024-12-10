using System.ComponentModel;

public class ChannelPropertyData : INotifyPropertyChanged
{
    private float _Value;

    public float Value
    {
        get => _Value;
        set
        {
            if (_Value != value)
            {
                _Value = value;
                OnPropertyChanged(nameof(_Value));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class SwitchPropertyData : INotifyPropertyChanged
{
    private bool _Value;

    public bool Value
    {
        get => _Value;
        set
        {
            if (_Value != value)
            {
                _Value = value;
                OnPropertyChanged(nameof(_Value));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
